using GWLPXL.ARPG._Scripts.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Statics.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.StatusEffects.com
{
   [System.Serializable]
    public class ModifyOTher
    {
        public Types.com.OtherAttributeType Type = Types.com.OtherAttributeType.None;
        public AttributeModifier Modifier = new AttributeModifier();
    }
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/StatusEffects/Logic/OtherModifier")]

    public class ModifyOtherStat : StatusEffectLogic//how can we get it to stack? add something that tracks move multi on the mover?
    {

        public ModifyOTher Vars = new ModifyOTher();

        public override void DoLogic(IActorHub target)
        {
            target.MyStats.GetRuntimeAttributes().AddModifierOther(Vars.Type, Vars.Modifier);
   

        }

        public override void UnDoLogic(IActorHub target)
        {
            target.MyStats.GetRuntimeAttributes().RemoveModifierOther(Vars.Type, Vars.Modifier);

        }
    }
}