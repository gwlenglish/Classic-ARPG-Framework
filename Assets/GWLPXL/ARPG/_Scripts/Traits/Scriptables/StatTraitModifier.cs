
using GWLPXL.ARPG._Scripts.Attributes.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Traits.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Equipment/Traits/NEW_StatTraitModifier")]

    [System.Serializable]
    public class StatTraitModifier : EquipmentTrait
    {
        public AttributeModifierManaged attributeModifier;
        public StatType Type;

        protected TraitType type = TraitType.Stat;

        public override void ApplyTrait(IAttributeUser toActor)
        {
            toActor.GetRuntimeAttributes().AddModifierStat(Type, attributeModifier.Convert(GetLeveledValue()));
        }
        
        public override TraitType GetTraitType()
        {
            return type;
        }

        public override void RemoveTrait(IAttributeUser toActor)
        {
            toActor.GetRuntimeAttributes().RemoveModifierStat(Type, attributeModifier.Convert(GetLeveledValue()));
        }
    }
}