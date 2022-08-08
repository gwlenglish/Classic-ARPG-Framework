
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
   /// Status effects wont carry over between scenes at the moment, would need to hold a status effect controller but meh, seems unnecessary
   /// </summary>

    public class PlayerStatusChangeReceiver : MonoBehaviour, IRecieveStatusChanges
    {

        [SerializeField]
        protected ActorReceiverStatusEffects receiverEvents = new ActorReceiverStatusEffects();
        [SerializeField]
        [Tooltip("Enabling will stop the display of the regen UI text when at full resource. " +
    "Disabling will always show the regen UI text.")]
        protected bool stopRegenDisplayAtFull = true;
        [SerializeField]
        [Tooltip("Enabling will factor in an actor's resists before applying a DOT, i.e. a DoT of 25 fire damage will be reduced by the resist amount on the actor. " +
            "Disable to allow the full damage to always pass through.")]
        protected bool allowDoTResists = true;

        protected IActorHub actorhub = null;

        protected List<StatusEffectVars> effects = new List<StatusEffectVars>();
        protected List<ModifyResourceVars> dots = new List<ModifyResourceVars>();
        
        protected virtual void OnEnable()
        {
            StatusEffectHelper.OnEffectAdded += AddedStatusEffect;
            StatusEffectHelper.OnEffectRemoved += RemovedStatusEffect;
            SoTHelper.OnDotApplied += AddedDoT;
            SoTHelper.OnDoTRemoved += RemovedDot;
            SoTHelper.OnRegenResource += AddedRegenResourceSoT;
            SoTHelper.OnReduceResource += AddedReduceResourceSoT;
        }

        protected virtual void OnDisable()
        {
            StatusEffectHelper.OnEffectAdded -= AddedStatusEffect;
            StatusEffectHelper.OnEffectRemoved -= RemovedStatusEffect;
            SoTHelper.OnDotApplied -= AddedDoT;
            SoTHelper.OnDoTRemoved -= RemovedDot;
            SoTHelper.OnRegenResource -= AddedRegenResourceSoT;
            SoTHelper.OnReduceResource -= AddedReduceResourceSoT;
        }





        protected virtual void CreateUIRegenText(int healAmount, ResourceType type)
        {
            if (actorhub.DungUser == null) return;
            if (stopRegenDisplayAtFull == true)
            {
                if (actorhub.MyStats.GetRuntimeAttributes().GetResourceNowValue(type) < actorhub.MyStats.GetRuntimeAttributes().GetResourceMaxValue(type))//check if resource is full
                {
                    actorhub.DungUser.CreateUIRegenText(healAmount.ToString(), type);//if not full, show
                }
                else
                {
                    //if full, don't show
                }
            }
            else
            {
                actorhub.DungUser.CreateUIRegenText(healAmount.ToString(), type);//show
            }


        }
        protected virtual void CreateUIReduceText(int damageAmount, ElementType eleType)
        {
            if (actorhub.DungUser == null) return;
            //do anything special if we want to consider 0
            actorhub.DungUser.CreateUIDoTText(damageAmount.ToString(), eleType);

        }



       


        protected virtual void AddedRegenResourceSoT(IActorHub hub, int amount, ResourceType type, ElementType element)
        {
            if (GetActorHub() != hub) return;
            CreateUIRegenText(amount, type);
            receiverEvents.SceneEvent.OnRegenResource.Invoke(amount, type, element);
        }
        protected virtual void AddedReduceResourceSoT(IActorHub hub, int amount, ResourceType type, ElementType element)
        {
            if (GetActorHub() != hub) return;
            CreateUIReduceText(amount, element);
            receiverEvents.SceneEvent.OnReduceResource.Invoke(amount, type, element);
        }

        protected virtual void AddedStatusEffect(IActorHub hub, StatusEffectVars newEffect)
        {
            if (GetActorHub() != hub) return;
            effects.Add(newEffect);
            receiverEvents.SceneEvent.OnStatusEffectApplied.Invoke(newEffect);

        }
        protected virtual void RemovedStatusEffect(IActorHub hub, StatusEffectVars newEffect)
        {
            if (GetActorHub() != hub) return;
            effects.Remove(newEffect);
            receiverEvents.SceneEvent.OnStatusEffectRemoved.Invoke(newEffect);

        }

        protected virtual void AddedDoT(IActorHub hub, ModifyResourceVars newEffect)
        {
            if (GetActorHub() != hub) return;
            if (dots.Contains(newEffect) == false)
            {
                dots.Add(newEffect);
            }


        }
        protected virtual void RemovedDot(IActorHub hub, ModifyResourceVars newEffect)
        {
            if (GetActorHub() != hub) return;
            if (dots.Contains(newEffect))
            {
                dots.Remove(newEffect);
            }


        }


        public List<StatusEffectVars> GetCurrentAppliedStatuses() => StatusEffectHelper.GetAllActiveEffects(GetActorHub());
        public Transform AtLocation()
        {
            return this.transform;
        }
        public IActorHub GetActorHub()
        {
            return actorhub;
        }

        public void SetActorHub(IActorHub newHub)
        {
            actorhub = newHub;
        }

        public List<ModifyResourceDoTState> GetCurrentlyAppliedDoTs() => SoTHelper.GetAllAppliedDots(GetActorHub());
       
    }
}