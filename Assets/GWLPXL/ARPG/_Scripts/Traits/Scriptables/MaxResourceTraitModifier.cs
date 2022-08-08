using GWLPXL.ARPG._Scripts.Attributes.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Traits.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Equipment/Traits/NEW_ResourceTraitModifier")]

    [System.Serializable]
    public class MaxResourceTraitModifier : EquipmentTrait
    {
        public AttributeModifierManaged attributeModifier;
        public ResourceType Type;

        protected TraitType type = TraitType.Resource;

        public override void ApplyTrait(IAttributeUser toActor)
        {
            toActor.GetRuntimeAttributes().AddModifierResource(Type, attributeModifier.Convert(GetLeveledValue()));
        }

        public override TraitType GetTraitType()
        {
            return type;
        }
        
        public override void RemoveTrait(IAttributeUser toActor)
        {
            toActor.GetRuntimeAttributes().RemoveModifierResource(Type, attributeModifier.Convert(GetLeveledValue()));
        }
    }
}