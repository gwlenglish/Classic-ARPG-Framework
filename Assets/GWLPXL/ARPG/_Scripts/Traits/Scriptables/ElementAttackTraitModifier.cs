
using GWLPXL.ARPG._Scripts.Attributes.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Traits.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Equipment/Traits/NEW_ElementAttackTraitModifier")]

    [System.Serializable]
    public class ElementAttackTraitModifier : EquipmentTrait
    {
        public AttributeModifierManaged attributeModifier;
        public ElementType Type;
        
        protected TraitType type = TraitType.ElementAttack;
        private int addedvalue = 0;
        
        public override void ApplyTrait(IAttributeUser toActor)
        {
            addedvalue = GetLeveledValue();
            toActor.GetRuntimeAttributes().AddModifierElementAttack(Type, attributeModifier.Convert(addedvalue));
        }

        public override void RemoveTrait(IAttributeUser toActor)
        {
            toActor.GetRuntimeAttributes().RemoveModifierElementAttack(Type, attributeModifier.Convert(addedvalue));
            addedvalue = 0;
        }
        
        public override TraitType GetTraitType()
        {
            return type;
        }
    }
}