
using GWLPXL.ARPGCore.Auras.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Movement.com;
using GWLPXL.ARPGCore.PlayerInput.com;

using UnityEngine.UI;
using UnityEngine;
using GWLPXL.ARPGCore.Abilities.com;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using GWLPXL.ARPGCore.Animations.com;

namespace GWLPXL.ARPGCore.States.com
{

    /// <summary>
    /// works well, but really needs to be cleaned up.
    /// </summary>
    public class MouseMoveAbility : IState
    {
        MouseNavMeshVars player = null;
        IAbilityPlayerInput playerAbilityInput = null;
        IPlayerMouseInput playerInput = null;
        IPlayerAuraInput playerAuraInput = null;
        IAbilityUser abilityUser = null;
        IUseAura auras = null;
        IActorHub actorHub = null;
        IReceiveDamage myDamage = null;
        Camera main = null;
        Vector3 mousePosition;
        //detect ui
        PointerEventData pointerEventData;
        List<RaycastResult> results = new List<RaycastResult>();
        List<IReceiveDamage> _hittemp = new List<IReceiveDamage>();
        bool blockingCanvases = false;
        public MouseMoveAbility(IActorHub _mover, MouseNavMeshVars _vars, Camera _main)
        {
            actorHub = _mover;
            main = _main;
            player = _vars;

        }
        public void Enter()
        {
            abilityUser = actorHub.MyAbilities;
            myDamage = actorHub.MyHealth;
            auras = actorHub.MyAuraUser;

            IPlayerInputHub input = actorHub.InputHub;

            playerAuraInput = input.AuraInputs;
            playerAbilityInput = input.AbilityInputs;
            playerInput = input.MouseInputs;
           

            if (player.EventSystem != null && player.GraphicRayers.Length > 0) blockingCanvases = true;
            if (blockingCanvases) pointerEventData = new PointerEventData(player.EventSystem);
        }

        public void Tick()
        {
            if (myDamage.IsDead()) return;
            mousePosition = Input.mousePosition;
            if (main == null)
            {
                main = Camera.main;
            }
            #region blocking canvases
            //these canvases will block our raycasts
            if (blockingCanvases)
            {
                pointerEventData.position = mousePosition;
                results.Clear();

                for (int i = 0; i < player.GraphicRayers.Length; i++)
                {
                    if (player.GraphicRayers[i] == null) continue;
                    player.GraphicRayers[i].Raycast(pointerEventData, results);
                }
                if (results.Count > 0)
                {
                    return;//do nothing since a ui element is blocking the ray
                }
            }
            #endregion


            #region aura input
            //aura input
            if (auras != null && playerAuraInput != null)
            {
                Aura aura = playerAuraInput.GetFirstAuraToggle();
                if (aura != null)
                {
                    auras.ToggleAura(aura);
                }
            }
            #endregion

            #region ability input

            bool attackmove = TryAttackMove(playerInput.GetMousePosition());//moves in range of the attack. if in range, attacks
            if (attackmove == true) return;//we entered atttacking, always prioritze

            #endregion



            #region basic moving input
            if (playerInput.GetMouseButtoneOne() || playerInput.GetMouseButtonOneDown())
            {
                bool hit = TryGroundMove(playerInput.GetMousePosition());
            }

            if (playerInput.GetMouseButtonOneDown())
            {
                bool hit = TryInteractionMove(playerInput.GetMousePosition());
            }
            #endregion
        }

        public void Exit()
        {

        }




        bool TryGroundMove(Vector3 atPosition)
        {
            RaycastHit groundHit;
            bool hitGround = Physics.Raycast(main.ScreenPointToRay(atPosition), out groundHit, player.RayLength, player.Ground);

            if (hitGround)
            {
                actorHub.MyMover.SetDesiredDestination(groundHit.point, 1f);
                player.LastHit = groundHit.point;
            }
            return hitGround;

        }

        bool TryInteractionMove(Vector3 atPosition)
        {

            RaycastHit interactableHit;
            bool hitInteractable = Physics.Raycast(main.ScreenPointToRay(atPosition), out interactableHit, player.RayLength, player.Interactable);
            if (hitInteractable)
            {

                IInteract _interactable = interactableHit.collider.GetComponent<IInteract>();
                if (_interactable == null) return false;

                if (_interactable.IsInRange(actorHub.MyTransform.gameObject))
                {
                    _interactable.DoInteraction(actorHub.MyTransform.gameObject);
                }
                else
                {
                    actorHub.MyMover.SetDesiredDestination(interactableHit.point, 1f);
                }
                player.LastHit = interactableHit.point;

            }
            return hitInteractable;
        }

        bool TryAttackMove(Vector3 atPosition)
        {



            if (abilityUser == null || playerAbilityInput == null) return false;

            //here we check if we have a skill to use and the player has hit the input
            Ability activeSkill = playerAbilityInput.GetFirstBasicAttack();
            if (activeSkill == null)//if no basic one, check others
            {
                activeSkill = playerAbilityInput.GetFirstAbilityInput();

            }


            if (activeSkill == null)
            {
                abilityUser.SetIntendedAbility(null);
                return false;
            }

            if (playerAbilityInput.GetForceAbility())
            {
                RaycastHit groundHit;
                bool hitGround = Physics.Raycast(main.ScreenPointToRay(atPosition), out groundHit, player.RayLength, player.Ground);
                if (hitGround)
                {
                    abilityUser.SetIntendedAbility(activeSkill);
                    Vector3 lookRotation = groundHit.point - actorHub.MyTransform.position;
                    lookRotation.y = 0;
                    actorHub.MyTransform.rotation = Quaternion.LookRotation(lookRotation);
                    abilityUser.TryCastAbility(activeSkill);
                    return true;//maybe in the futrue we dont do the first attack when toggling on
                }


            }

            //we have an ability, so let's see if we have something to use it on
            RaycastHit[] hitAttackable = Physics.RaycastAll(main.ScreenPointToRay(atPosition), player.RayLength, player.Attackable);
            Vector3 point = Vector3.zero;
            _hittemp.Clear();
            for (int i = 0; i < hitAttackable.Length; i++)
            {
                IActorHub attackable = hitAttackable[i].collider.GetComponent<IActorHub>();
                if (attackable == null) continue;
                if (attackable.MyHealth != myDamage && attackable.MyHealth.IsDead() == false)//not me and not dead
                {
                    point = hitAttackable[i].point;
                    point.y = actorHub.MyTransform.position.y;
                    player.LastHit = hitAttackable[i].point;
                    _hittemp.Add(attackable.MyHealth);
                }

            }

            if (_hittemp.Count == 0)
            {
                abilityUser.SetIntendedAbility(null);
                return false;
            }


            //find the closest to the player
            float closest = Mathf.Infinity;
            Vector3 playerPos = actorHub.MyTransform.position;
            for (int i = 0; i < _hittemp.Count; i++)
            {
                Vector3 dir = _hittemp[i].GetInstance().position - playerPos;
                float sqrdmag = dir.sqrMagnitude;
                if (sqrdmag < closest)
                {
                    closest = sqrdmag;
                    point = _hittemp[i].GetInstance().position;
                }
            }

            //we get the distance, and then the squared magnitude. The square magnitude is more performant.
            point.y = actorHub.MyTransform.position.y;
            Vector3 distance = point - actorHub.MyTransform.position;
            float sqrMag = distance.sqrMagnitude;
            //ARPGDebugger.DebugMessage(sqrMag.ToString());
            if (sqrMag <= (activeSkill.Data.Range * activeSkill.Data.Range))
            {
                abilityUser.SetIntendedAbility(activeSkill);
                float desiredRange = activeSkill.Data.Range;
                //now we can attack. So don't set the destination, just try the attack and rotate us towards the target
                Vector3 lookRotation = point - actorHub.MyTransform.position;
                lookRotation.y = 0;
                actorHub.MyTransform.rotation = Quaternion.LookRotation(lookRotation);
                //playerMover.SetDesiredDestination(point, desiredRange);
                abilityUser.TryCastAbility(activeSkill);
                return true;
            }
            float stoppingDistance = activeSkill.GetRangeWithBuffer();//hook to active skill being tried
                                                                  //if we made it here, move us closer so we can attack next time.
            actorHub.MyMover.SetDesiredDestination(point, stoppingDistance);




            return false;
            //in all cases that we aren't close enough to attack, we return false

        }


    }
}

