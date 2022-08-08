
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Traits.com
{

    /// <summary>
    /// use MaxResourceTraitModifier
    /// </summary>
    [System.Obsolete]
    [System.Serializable]
    public class MaxResourceTrait : EquipmentTrait
    {
        public ResourceType Type;

        protected TraitType type = TraitType.Resource;

        public override void ApplyTrait(IAttributeUser toActor)
        {

            toActor.GetRuntimeAttributes().ModifyMaxResource(Type, GetLeveledValue());
            //ARPGDebugger.DebugMessage(GetTraitUIDescription());
        }




        public override TraitType GetTraitType()
        {
            return type;
        }



        public override void RemoveTrait(IAttributeUser toActor)
        {

            toActor.GetRuntimeAttributes().ModifyMaxResource(Type, -GetLeveledValue());

        }


    }
}