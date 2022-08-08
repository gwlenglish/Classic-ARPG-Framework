
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Traits.com
{

    /// <summary>
    /// use StatTraitModifier
    /// </summary>
    [System.Obsolete]
    [System.Serializable]
    public class StatTrait : EquipmentTrait
    {
        public StatType Type;

        protected TraitType type = TraitType.Stat;

        public override void ApplyTrait(IAttributeUser toActor)
        {
            toActor.GetRuntimeAttributes().ModifyBaseStatValue(Type, GetLeveledValue());
            //ARPGDebugger.DebugMessage(GetTraitUIDescription());

        }



        public override TraitType GetTraitType()
        {
            return type;
        }



        public override void RemoveTrait(IAttributeUser toActor)
        {
            toActor.GetRuntimeAttributes().ModifyBaseStatValue(Type, -GetLeveledValue());
        }


    }
}