
using UnityEngine;
using GWLPXL.ARPGCore.Types.com;

using System.Collections.Generic;
using GWLPXL.ARPG._Scripts.Attributes.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Auras.com
{


    public class EnemyAuraReceiver : MonoBehaviour, ITakeAura
    {
        [SerializeField]
        protected EnemyAuraReceiveEvents receiveEvents = new EnemyAuraReceiveEvents();
        [SerializeField]
        protected AuraTargetGroup[] mygroups = new AuraTargetGroup[1] { AuraTargetGroup.Enemies };

        protected IActorHub hub = null;
        protected List<Aura> affectedList = new List<Aura>();
 
     
        public virtual void AuraModifyCurrentResource(int resourceType, int byAmount)
        {
            hub.MyStats.GetRuntimeAttributes().ModifyNowResource((ResourceType)resourceType, byAmount);
            receiveEvents.SceneEvents.OnReceiveResourceCurrent.Invoke((ResourceType)resourceType, byAmount);
        }

        public virtual void AuraModifyMaxResource(int resourceType, int byAmount)
        {
            hub.MyStats.GetRuntimeAttributes().ModifyMaxResource((ResourceType)resourceType, byAmount);
            receiveEvents.SceneEvents.OnReceiveResourceAuraMax.Invoke((ResourceType)resourceType, byAmount);

        }
        public virtual void AuraBuffSat(int statType, int byAmount)
        {
            hub.MyStats.GetRuntimeAttributes().ModifyBaseStatValue((StatType)statType, byAmount);
            receiveEvents.SceneEvents.OnReceiveStatAura.Invoke((StatType)statType, byAmount);


        }
        
        // Todo thinking about events with modifiers. Maybe not by amount, just push NowValue
        public virtual void AuraApplyModifierResource(int resourceType, AttributeModifier modifier)
        {
            hub.MyStats.GetRuntimeAttributes().AddModifierResource((ResourceType)resourceType, modifier);
        }

        public virtual void AuraRemoveModifierResource(int resourceType, AttributeModifier modifier)
        {
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

       

      

        public virtual void SceneCleanUp()
        {
            for (int i = 0; i < affectedList.Count; i++)
            {
                affectedList[i].Remove(this);
            }
        }

        public virtual void SetActorHub(IActorHub newHub)
        {
            hub = newHub;
        }
    }
}