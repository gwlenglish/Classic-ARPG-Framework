
using GWLPXL.ARPG._Scripts.Attributes.com;
using UnityEngine;
using GWLPXL.ARPGCore.Types.com;

using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Auras.com
{
  

    public class PlayerAuraReceiver : MonoBehaviour, ITakeAura
    {
        [SerializeField]
        protected PlayerAuraReceiveEvents receiveEvents = new PlayerAuraReceiveEvents();
        [SerializeField]
        protected AuraTargetGroup[] mygroups = new AuraTargetGroup[1] { AuraTargetGroup.Friendly };


        protected IActorHub hub = null;
       
        public virtual void AuraModifyCurrentResource(int resourceType, int byAmount)
        {
            if (hub.MyHealth != null)
            {
                if (hub.MyHealth.IsDead()) return;
            }
            hub.MyStats.GetRuntimeAttributes().ModifyNowResource((ResourceType)resourceType, byAmount);
            receiveEvents.SceneEvents.OnReceiveResourceCurrent.Invoke((ResourceType)resourceType, byAmount);
        }

        public virtual void AuraModifyMaxResource(int resourceType, int byAmount)
        {
            if (hub.MyHealth != null)
            {
                if (hub.MyHealth.IsDead()) return;
            }
            hub.MyStats.GetRuntimeAttributes().ModifyMaxResource((ResourceType)resourceType, byAmount);
            receiveEvents.SceneEvents.OnReceiveResourceAuraMax.Invoke((ResourceType)resourceType, byAmount);
        }

        public virtual void AuraBuffSat(int statType, int byAmount)
        {
            if (hub.MyHealth != null)
            {
                if (hub.MyHealth.IsDead()) return;
            }
            hub.MyStats.GetRuntimeAttributes().ModifyBaseStatValue((StatType)statType, byAmount);
            receiveEvents.SceneEvents.OnReceiveStatAura.Invoke((StatType)statType, byAmount);

        }

        // Todo thinking about events with modifiers. Maybe not by amount, just push NowValue or save old value and subtract
        public virtual void AuraApplyModifierResource(int resourceType, AttributeModifier modifier)
        {
            if (hub.MyHealth != null)
            {
                if (hub.MyHealth.IsDead()) return;
            }
            hub.MyStats.GetRuntimeAttributes().AddModifierResource((ResourceType)resourceType, modifier);
        }

        public virtual void AuraRemoveModifierResource(int resourceType, AttributeModifier modifier)
        {
            if (hub.MyHealth != null)
            {
                if (hub.MyHealth.IsDead()) return;
            }
            hub.MyStats.GetRuntimeAttributes().RemoveModifierResource((ResourceType)resourceType, modifier);
        }

        public virtual void AuraRemoveSourceModifierResource(int resourceType, object source)
        {
            if (hub.MyHealth != null)
            {
                if (hub.MyHealth.IsDead()) return;
            }
            hub.MyStats.GetRuntimeAttributes().RemoveSourceModifierResource((ResourceType)resourceType, source);
        }
        public virtual void AuraApplyModifierStat(int statType, AttributeModifier modifier)
        {
            if (hub.MyHealth != null)
            {
                if (hub.MyHealth.IsDead()) return;
            }
            hub.MyStats.GetRuntimeAttributes().AddModifierStat((StatType)statType, modifier);
        }

        public virtual void AuraRemoveModifierStat(int statType, AttributeModifier modifier)
        {
            if (hub.MyHealth != null)
            {
                if (hub.MyHealth.IsDead()) return;
            }
            hub.MyStats.GetRuntimeAttributes().RemoveModifierStat((StatType)statType, modifier);
        }

        public virtual void AuraRemoveSourceModifierStat(int statType, object source)
        {
            if (hub.MyHealth != null)
            {
                if (hub.MyHealth.IsDead()) return;
            }
            hub.MyStats.GetRuntimeAttributes().RemoveSourceModifierStat((StatType)statType, source);
        }

        public virtual AuraTargetGroup[] GetAuraGroups()
        {
            return mygroups;
        }

        public virtual GameObject GetGameObjectInstance()
        {
            return this.gameObject;
        }

        public virtual void SetActorHub(IActorHub newHub) => hub = newHub;

    }
}