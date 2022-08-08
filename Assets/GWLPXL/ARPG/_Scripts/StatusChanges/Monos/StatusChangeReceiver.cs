
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.CanvasUI.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.StatusEffects.com
{
    /// <summary>
    /// deprecated, use either the Player or Enemy variant. PlayerStatusChangeReceiver / EnemyStatusChangeReceiver
    /// not linked to the combat handlers, will eventually delete
    /// </summary>

    public class StatusChangeReceiver : MonoBehaviour, IRecieveStatusChanges
    {

        [SerializeField]
        ActorReceiverStatusEffects receiverEvents = new ActorReceiverStatusEffects();
        [SerializeField]
        [Tooltip("Enabling will stop the display of the regen UI text when at full resource. " +
    "Disabling will always show the regen UI text.")]
        bool stopRegenDisplayAtFull = true;
        [SerializeField]
        [Tooltip("Enabling will factor in an actor's resists before applying a DOT, i.e. a DoT of 25 fire damage will be reduced by the resist amount on the actor. " +
            "Disable to allow the full damage to always pass through.")]
        bool allowDoTResists = true;

        IAttributeUser statUser = null;
        IUseFloatingText dungeonUser = null;
        IReceiveDamage damageReceiver = null;
        IActorHub actorhub = null;
        List<IDoT> appliedDots = new List<IDoT>();


        private void Awake()
        {
            dungeonUser = GetComponent<IUseFloatingText>();
            statUser = GetComponent<IAttributeUser>();
            damageReceiver = GetComponent<IReceiveDamage>();
        }


        public IAttributeUser GetStatUser()
        {
            return statUser;
        }
        public Transform AtLocation()
        {
            return this.transform;
        }

        public void RegenResource(int healAmount, ResourceType type)
        {
            RegenResource(healAmount, type, ElementType.None);
        }
        public void RegenResource(int healAmount, ResourceType type, ElementType elementRegen)
        {
            //do something with element type
            if (damageReceiver.IsDead()) return;

            healAmount = Mathf.Abs(healAmount);
            ModifyResource(type, healAmount);
            CreateUIRegenText(healAmount, type);
            receiverEvents.SceneEvent.OnRegenResource.Invoke(healAmount, type, elementRegen);
        }

        public void ReduceResource(int damageAmount, ResourceType type)
        {
            ReduceResource(damageAmount, type, ElementType.None);
            
        }
        public void ReduceResource(int damageAmount, ResourceType type, ElementType elementDamage)
        {
            if (damageReceiver.IsDead()) return;

            //since this is a reduce call, ensure that we are passing a negative
            if (damageAmount > 0)
            {
                damageAmount *= -1;
            }
            
            ModifyResource(type, damageAmount);
            CreateUIReduceText(damageAmount, elementDamage);
            receiverEvents.SceneEvent.OnReduceResource.Invoke(damageAmount, type, elementDamage);
        }



        void CreateUIRegenText(int healAmount, ResourceType type)
        {
            if (dungeonUser == null) return;
            if (stopRegenDisplayAtFull == true)
            {
                if (statUser.GetRuntimeAttributes().GetResourceNowValue(type) < statUser.GetRuntimeAttributes().GetResourceMaxValue(type))//check if resource is full
                {
                    dungeonUser.CreateUIRegenText(healAmount.ToString(), type);//if not full, show
                }
                else
                {
                    //if full, don't show
                }
            }
            else
            {
                dungeonUser.CreateUIRegenText(healAmount.ToString(), type);//show
            }


        }
        void CreateUIReduceText(int damageAmount, ElementType eleType)
        {
            if (dungeonUser == null) return;
            dungeonUser.CreateUIDamageText(damageAmount.ToString(), eleType);
          
        }

        void ModifyResource(ResourceType type, int byAmount)
        {
            
            GetStatUser().GetRuntimeAttributes().ModifyNowResource(type, byAmount);//we dont call into take damage because we dont want to trigger the iframe cooldown due to a DOT
            damageReceiver.CheckDeath();

        }

        public void AddDoT(IDoT newDOt)
        {
            appliedDots.Add(newDOt);

        }

        public void RemoveDot(IDoT dot)
        {
            appliedDots.Remove(dot);

        }

        public IActorHub GetActorHub()
        {
            return actorhub;
        }

        public void SetActorHub(IActorHub newHub)
        {
            actorhub = newHub;
        }

      

        public void AddDoT(ModifyResourceVars vars)
        {
         
        }

        public void RemoveDot(ModifyResourceVars vars)
        {

        }


        public void AddStatusEffect(StatusEffectVars newEffect)
        {
            //currentStatusEffects.TryGetValue(newEffect, out int value);
            //value += 1;
            //value = Mathf.Clamp(value, 0, newEffect.MaxStacks);
            //currentStatusEffects[newEffect] = value;
        }

        public void RemoveStatusEffect(StatusEffectVars effect)
        {
            //currentStatusEffects.TryGetValue(effect, out int value);
            //value -= 1;
            //value = Mathf.Clamp(value, 0, effect.MaxStacks);
            //currentStatusEffects[effect] = value;
        }


        public List<StatusEffectVars> GetCurrentAppliedStatuses() => StatusEffectHelper.GetAllActiveEffects(GetActorHub());

        public List<ModifyResourceDoTState> GetCurrentlyAppliedDoTs() => SoTHelper.GetAllAppliedDots(GetActorHub());
       
    }
}