
using GWLPXL.ARPG._Scripts.Attributes.com;
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;
using GWLPXL.ARPGCore.Attributes.com;

namespace GWLPXL.ARPGCore.Traits.com
{
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Equipment/Traits/NEW_AbilityModTraitModifier")]
    public class AbilityModTraitModifier : EquipmentTrait
    {
        public AttributeModifierManaged attributeModifier;
        public Ability AbilityToMod;

        protected TraitType type = TraitType.AbilityMod;
        public override void ApplyTrait(IAttributeUser toActor)
        {
            toActor.GetRuntimeAttributes().AddModifierAbilityMod(AbilityToMod, attributeModifier.Convert(GetLeveledValue()));
        }
        
        public override TraitType GetTraitType()
        {
            return type;
        }
        
        public override void RemoveTrait(IAttributeUser toActor)
        {
            toActor.GetRuntimeAttributes().RemoveModifierAbilityMod(AbilityToMod, attributeModifier.Convert(GetLeveledValue()));
        }

        public override string GetTraitPrefix()
        {
            string prefix = "EMPTY";
            if (AbilityToMod != null)
            {
                return AbilityToMod.Data.Name;
            }
            return prefix;
        }

        public override string GetTraitSuffix()
        {
            string suffix = "EMPTY";
            if (AbilityToMod != null)
            {
                return "of " + AbilityToMod.Data.Name;
            }
            return suffix;
        }
    }
}