using GWLPXL.ARPG._Scripts.Attributes.com;
using GWLPXL.ARPGCore.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.StatusEffects.com
{
   [System.Serializable]
    public class ModStatStatus
    {
        public Types.com.StatType Type = Types.com.StatType.None;
        public AttributeModifier Modifier = new AttributeModifier();
    }
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/StatusEffects/Logic/Stat_Modifier")]

    public class ModifyStat : StatusEffectLogic//how can we get it to stack? add something that tracks move multi on the mover?
    {

        public ModStatStatus Vars = new ModStatStatus();

        public override void DoLogic(IActorHub target)
        {
            target.MyStats.GetRuntimeAttributes().AddModifierStat(Vars.Type, Vars.Modifier);
   

        }

        public override void UnDoLogic(IActorHub target)
        {
            target.MyStats.GetRuntimeAttributes().AddModifierStat(Vars.Type, Vars.Modifier);

        }
    }
}