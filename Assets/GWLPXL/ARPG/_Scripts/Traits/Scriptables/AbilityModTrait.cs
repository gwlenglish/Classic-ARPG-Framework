
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;
using GWLPXL.ARPGCore.Attributes.com;
using System;

namespace GWLPXL.ARPGCore.Traits.com
{


    /// <summary>
    /// obsolute, use the AbilityModTraitModifier
    /// </summary>
    /// 
    [Obsolete]
    public class AbilityModTrait : EquipmentTrait
    {
        public Ability AbilityToMod;

        protected TraitType type = TraitType.AbilityMod;
        public override void ApplyTrait(IAttributeUser toActor)
        {
            toActor.GetRuntimeAttributes().ModifyAbilityMod(AbilityToMod, GetLeveledValue());
        }



        public override TraitType GetTraitType()
        {
            return type;
        }



        public override void RemoveTrait(IAttributeUser toActor)
        {
            toActor.GetRuntimeAttributes().ModifyAbilityMod(AbilityToMod, -GetLeveledValue());
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