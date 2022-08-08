
using GWLPXL.ARPGCore.Auras.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Movement.com;
using GWLPXL.ARPGCore.PlayerInput.com;
using GWLPXL.ARPGCore.Abilities.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.States.com
{
    //delete, deprecated

    //public class MouseMove : IState
    //{
    //    MouseNavMeshVars player;
    //    ISkillUser combatSkills;
    //    IUseAura auras;
    //    IMover playerMover;
    //    IPlayerInput playerInput;
    //    IReceiveDamage myDamage;
    //    Camera main;

    //    public MouseMove(IMover _mover, MouseNavMeshVars _player, IPlayerInput _playerInpt, Camera _main, ISkillUser _combatSkills, IReceiveDamage _myDamage, IUseAura _auras)
    //    {
    //        combatSkills = _combatSkills;
    //        playerMover = _mover;
    //        playerInput = _playerInpt;
    //        main = _main;
    //        myDamage = _myDamage;
    //        player = _player;
    //        auras = _auras;
    //    }
    //    public void Enter()
    //    {

    //    }



    //    public void Exit()
    //    {

    //    }




    //    bool TryGroundMove(Vector3 atPosition)
    //    {
    //        RaycastHit groundHit;
    //        bool hitGround = Physics.Raycast(main.ScreenPointToRay(atPosition), out groundHit, player.RayLength, player.Ground);

    //        if (hitGround)
    //        {
    //            playerMover.SetDesiredDestination(groundHit.point, 1f);
    //            player.LastHit = groundHit.point;
    //        }
    //        return hitGround;

    //    }

    //    bool TryInteractionMove(Vector3 atPosition)
    //    {

    //        RaycastHit interactableHit;
    //        bool hitInteractable = Physics.Raycast(main.ScreenPointToRay(atPosition), out interactableHit, player.RayLength, player.Interactable);
    //        if (hitInteractable)
    //        {

    //            IInteract _interactable = interactableHit.collider.GetComponent<IInteract>();
    //            if (_interactable == null) return false;

    //            if (_interactable.IsInRange(playerMover.GetGameObject()))
    //            {
    //                _interactable.DoInteraction(playerMover.GetGameObject());
    //            }
    //            else
    //            {
    //                playerMover.SetDesiredDestination(interactableHit.point, 1f);
    //            }
    //            player.LastHit = interactableHit.point;

    //        }
    //        return hitInteractable;
    //    }

    //    bool TryAttackMove(Vector3 atPosition)
    //    {
    //        //combat skills can navigate this
    //        //RaycastHit[] hitAttackable = Physics.SphereCastAll(main.ScreenPointToRay(atPosition), playerMover.SphereCastRadius, playerMover.RayLength, playerMover.Attackable);
    //        RaycastHit[] hitAttackable = Physics.RaycastAll(main.ScreenPointToRay(atPosition), player.RayLength, player.Attackable);

    //        Vector3 point = Vector3.zero;
    //        bool hitEnemy = false;
    //        for (int i = 0; i < hitAttackable.Length; i++)
    //        {
    //            IReceiveDamage attackable = hitAttackable[i].collider.GetComponent<IReceiveDamage>();
    //            if (attackable != myDamage)
    //            {
    //                point = hitAttackable[i].point;
    //                point.y = playerMover.GetGameObject().transform.position.y;
    //                player.LastHit = hitAttackable[i].point;
    //                hitEnemy = true;
    //                break;
    //            }

    //        }

    //        if (hitEnemy)
    //        {
    //            Ability activeSkill = combatSkills.GetActiveSkill();
    //            if (activeSkill == null) return false;

    //            float stoppingDistance = activeSkill.GetRange() * .9f;//hook to active skill being tried

    //            //we get the distance, and then the squared magnitude. The square magnitude is more performant.
    //            point.y = playerMover.GetGameObject().transform.position.y;
    //            Vector3 distance = point - playerMover.GetGameObject().transform.position;
    //            float sqrMag = distance.sqrMagnitude;
    //            //ARPGDebugger.DebugMessage(sqrMag.ToString());
    //            if (sqrMag <= (stoppingDistance * stoppingDistance))
    //            {
    //                //now we can attack. So don't set the destination, just try the attack and rotate us towards the target
    //                Vector3 lookRotation = point - playerMover.GetGameObject().transform.position;
    //                lookRotation.y = 0;
    //                playerMover.GetGameObject().transform.rotation = Quaternion.LookRotation(lookRotation);
    //                playerMover.SetDesiredDestination(point, stoppingDistance);
    //                combatSkills.DoSkill();
    //                return true;
    //            }

    //            playerMover.SetDesiredDestination(point, stoppingDistance);


    //        }

    //        return false;
    //        //in all cases that we aren't close enough to attack, we return false

    //    }

    //    public void Tick()
    //    {

    //        if (myDamage.IsDead()) return;

    //        if (combatSkills != null)
    //        {
    //            if (playerInput.GetAuraKeyDown(0))
    //            {
    //                //toggle auras
    //                auras.ToggleAura(0);
    //            }
    //            if (playerInput.GetAuraKeyDown(1))
    //            {
    //                //toggle auras
    //                auras.ToggleAura(1);

    //            }

    //            if (combatSkills.GetIsUsingSkill())
    //            {
    //                return;
    //            }

    //            if (playerInput.GetButtonOne() || playerInput.GetButtonOneDown())
    //            {
    //                combatSkills.SetActiveSkill(0);
    //            }
    //            if (playerInput.GetButtonTwo() || playerInput.GetButtonTwoDown())
    //            {
    //                combatSkills.SetActiveSkill(1);
    //            }


    //            bool left = false;
    //            if (playerInput.GetButtonOne() || playerInput.GetButtonOneDown())
    //            {
    //                left = TryAttackMove(playerInput.GetDesiredDestination());

    //            }

    //            if (left)
    //            {
    //                return;
    //            }



    //            bool right = false;
    //            if (playerInput.GetButtonTwo() || playerInput.GetButtonTwoDown())
    //            {

    //                right = TryAttackMove(playerInput.GetDesiredDestination());

    //            }

    //            if (right)
    //            {
    //                return;
    //            }

    //        }





    //        if (playerInput.GetButtonOne() || playerInput.GetButtonOneDown())
    //        {
    //            bool hit = TryGroundMove(playerInput.GetDesiredDestination());
    //        }

    //        if (playerInput.GetButtonOneDown())
    //        {
    //            bool hit = TryInteractionMove(playerInput.GetDesiredDestination());
    //        }

    //    }
    //}
}
