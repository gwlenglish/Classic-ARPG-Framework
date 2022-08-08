
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Traits.com
{


    /// <summary>
    /// Obsolete, use ElementAttackTraitModifier
    /// </summary>
    [System.Obsolete]
    [System.Serializable]
    
    public class ElementAttackTrait : EquipmentTrait
    {
        public ElementType Type;
        protected TraitType type = TraitType.ElementAttack;
        int addedvalue = 0;
        public override void ApplyTrait(IAttributeUser toActor)
        {
            addedvalue = GetLeveledValue();
            toActor.GetRuntimeAttributes().ModifyElementAttackBaseValue(Type, addedvalue);

        }

        public override void RemoveTrait(IAttributeUser toActor)
        {
            toActor.GetRuntimeAttributes().ModifyElementAttackBaseValue(Type, -addedvalue);
            addedvalue = 0;

        }



        public override TraitType GetTraitType()
        {
            return type;
        }


       

    }


}