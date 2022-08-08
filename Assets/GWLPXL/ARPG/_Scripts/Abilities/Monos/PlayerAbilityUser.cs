using GWLPXL.ARPGCore.Animations.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.com
{

    /// <summary>
    /// experimental, no animator link. Animator link found in the state machine.
    /// </summary>
    public class PlayerAbilityUser : MonoBehaviour, IAbilityUser
    {
        [SerializeField]
        [Tooltip("Scene specific events")]
        protected ActorAbilityEvents abilityEvents = new ActorAbilityEvents();

        [SerializeField]
        protected AbilityController abilityControllerTemplate = null;

        [SerializeField]
        protected bool syncAnimationSpeedWithAbility = true;

        [SerializeField]
        [Tooltip("If using the Player State Machine 2D or another animation system, turn this off.")]
        protected bool triggerAnimations = true;
        protected AbilityController runtime = null;
        protected Ability lastIntended = null;
        protected Transform me;

        protected IActorHub actorhub = null;
        protected Ability charged = null;
        protected virtual void Awake()
        {
            me = GetComponent<Transform>();

            if (abilityControllerTemplate == null)
            {
                Debug.LogError("No ability controller, can't use abilities", this);
                return;
            }
            SetRuntimeController(Instantiate(GetTemplate()));


          
        }


       
        //subscribe events
        public virtual void SubscribeEvents()
        {
            GetRuntimeController().OnAbilityStart += AbilityStartEvent;
            GetRuntimeController().OnLearnedAbility += AbilityLearnEvent;
            GetRuntimeController().OnForgetAbility += AbilityForgetEvent;
            GetRuntimeController().OnAbilityEnd += AbiltiyEndEvent;

        }

        public virtual void UnSubscribeEvents()
        {
            GetRuntimeController().OnAbilityStart -= AbilityStartEvent;
            GetRuntimeController().OnLearnedAbility -= AbilityLearnEvent;
            GetRuntimeController().OnForgetAbility -= AbilityForgetEvent;
            GetRuntimeController().OnAbilityEnd -= AbiltiyEndEvent;

        }

        protected virtual void AbiltiyEndEvent(Ability ability)
        {
            abilityEvents.SceneEvents.OnAbilityEnd.Invoke(ability);
        }
        protected virtual void AbilityForgetEvent(Ability ability, int slot)
        {
            abilityEvents.SceneEvents.OnAbilityForgot.Invoke(ability);
        }
        protected virtual void AbilityLearnEvent(Ability ability, int slot)
        {
            abilityEvents.SceneEvents.OnAbilityLearned.Invoke(ability);
        }
        protected virtual void AbilityStartEvent(Ability ability)
        {
            abilityEvents.SceneEvents.OnAbilityTriggered.Invoke(ability);
        }
        public Transform GetParentTransform() => me;



        public void SetTemplate(AbilityController newTemplate)
        {
            abilityControllerTemplate = newTemplate;
        }

        public bool TryCastAbility(Ability toCast)
        {
            if (actorhub.MyHealth != null)
            {
                if (actorhub.MyHealth.IsDead())
                {
                    //we dead
                    return false;
                }
            }

            if (toCast.Data.ResourceCost > 0 && toCast.Data.ResourceType != Types.com.ResourceType.None)//only check if we have resource cost
            {
                if (actorhub.MyStats == null)
                {
                    ARPGDebugger.DebugMessage("Ability has a COST, but gameobject doesn't have a stat user. Can't cast", this);
                    return false;
                }


                if (actorhub.MyStats.GetRuntimeAttributes().DoWeHaveResources(toCast.Data.ResourceType, Mathf.FloorToInt(toCast.Data.ResourceCost)) == false)
                {
                    ARPGDebugger.DebugMessage("Ability has a COST, but gameobject doesn't have enough resources. Can't cast", this);
                    abilityEvents.SceneEvents.OnAbilityFailedCost.Invoke();
                    return false;
                }
            }




            bool success = GetRuntimeController().TryCastAbility(actorhub, toCast);

            ARPGDebugger.DebugMessage(this.gameObject.name + " " + success, this);

            if (success)
            {
                if (toCast.Data.ResourceCost > 0)
                {
                    actorhub.MyStats.GetRuntimeAttributes().ModifyNowResource(toCast.Data.ResourceType, -Mathf.FloorToInt(toCast.Data.ResourceCost));
                }
                TriggerAbilityAnimation(toCast);

                if (syncAnimationSpeedWithAbility)
                {
                    if (actorhub.MyAnimator)
                    {
                        actorhub.MyAnimator.speed = toCast.Duration / GetRuntimeController().GetClampedAbilitySpeedCooldown(toCast);
                        GetRuntimeController().OnAbilityEnd += ResetAnimator;
                    }
                    else
                    {
                        Debug.LogWarning("Can't sync animator without an animator on the object.");
                    }

                }
            }
            return success;

        }

        protected virtual void TriggerAbilityAnimation(Ability toCast)
        {
            if (actorhub.MyAnimator != null && triggerAnimations == true)
            {
                actorhub.MyAnim.SetAnimatorState(toCast.Data.AnimationTrigger, toCast.Data.AnimBlending, toCast.Data.AnimationIndex);
            }

           
        }
        protected virtual void ResetAnimator(Ability ability)
        {
            actorhub.MyAnimator.speed = 1;
            GetRuntimeController().OnAbilityEnd -= ResetAnimator;
        }


        public bool GetInCooldown()
        {
            AbilityDurationTimer timer = GetRuntimeController().GetTimer(GetLastIntendedAbility());
            return timer != null;
        }
        public Ability GetLastIntendedAbility()
        {
            return lastIntended;
        }

        public void SetIntendedAbility(Ability ability)
        {
            if (ability == null)
            {
                Ability previous = GetLastIntendedAbility();
               
            }

            for (int i = 0; i < GetRuntimeController().BasicAttacks.Length; i++)
            {
                if (ability == GetRuntimeController().BasicAttacks[i])
                {

                    break;
                }
            }


            lastIntended = ability;
        }

        public void SetIntendedAbility(int equippedAbilitySlot)
        {
            Ability ability = runtime.GetEquippedAbility(equippedAbilitySlot);
            if (ability == GetLastIntendedAbility()) return;//same thing, dont bother looking it up
            if (ability != null)
            {


                if (GetInCooldown() == false)
                {
                    SetIntendedAbility(ability);
                }
            }
            else
            {
                ARPGDebugger.DebugMessage("Ability at slot " + equippedAbilitySlot + " is NULL. Can't set as intended ability", this);
            }

        }

        public AbilityController GetRuntimeController()
        {
            return runtime;
        }

        public AbilityController GetTemplate()
        {
            return abilityControllerTemplate;
        }

        public void SetRuntimeController(AbilityController abilityController)
        {
            if (runtime != null)
            {
                UnSubscribeEvents();
            }
            runtime = abilityController;

            if (runtime != null)
            {
                SubscribeEvents();
            }
        }

        public void SetIntendedBasicAttack()
        {
            Ability ability = GetRuntimeController().GetBasicAttack(actorhub);
            if (ability == GetLastIntendedAbility()) return;//same thing, dont bother looking it up
            if (ability != null)
            {


                if (GetInCooldown() == false)
                {
                    SetIntendedAbility(ability);
                }


            }
            else
            {
                //  ARPGDebugger.DebugMessage("Ability at slot " + equippedAbilitySlot + " is NULL. Can't set as intended ability", this);
            }
        }

        /// <summary>
        /// call into to modify the ability speed.
        /// </summary>
        /// <param name="byAmount"></param>
        public void ModifyAbilityMulti(float byAmount)//modifiables
        {

            GetRuntimeController().ModifyAbilityMulti(byAmount);

        }

        public IActorHub GetActorHub() => actorhub;
        

        public void SetActorHub(IActorHub newhub) => actorhub = newhub;


        public Ability GetChargedAbility() => charged;


        public void SetChargedAbility(Ability ability) => charged = ability;
    }


}