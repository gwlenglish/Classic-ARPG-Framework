
using System.Collections.Generic;
using GWLPXL.ARPG._Scripts.Attributes.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Traits.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Equipment/Traits/NEW_ElementResistTraitModifier")]

    [System.Serializable]
    public class ElementResistTraitModifier : EquipmentTrait
    {
        public AttributeModifierManaged attributeModifier;
        public ElementType Type;

        protected TraitType type = TraitType.ElementResist;
        
        public override void ApplyTrait(IAttributeUser toActor)
        {
            toActor.GetRuntimeAttributes().AddModifierElementResist(Type, attributeModifier.Convert(GetLeveledValue()));
        }

        public override TraitType GetTraitType()
        {
            return type;
        }
        
        public override void RemoveTrait(IAttributeUser toActor)
        {
            toActor.GetRuntimeAttributes().RemoveModifierElementResist(Type, attributeModifier.Convert(GetLeveledValue()));
        }
    }
}