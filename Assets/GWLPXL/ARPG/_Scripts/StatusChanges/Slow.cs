using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Statics.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.StatusEffects.com
{
    [System.Serializable]
    public class SlowVars
    {
        [Range(0, 1f)]
        [Tooltip("Will slow the target by %. 1 = 100%, which is a freeze.")]
        public float SlowPercent = .5f;
        [HideInInspector]
        public float Savedspeed = 0;
        [System.NonSerialized]
        public Dictionary<IActorHub, bool> Dicvalue = new Dictionary<IActorHub, bool>();

        public SlowVars(float slowPercent)
        {
            SlowPercent = slowPercent;
            Dicvalue = new Dictionary<IActorHub, bool>();
            Savedspeed = 0;
        }
    }
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/StatusEffects/Logic/Slow")]

    public class Slow : StatusEffectLogic//how can we get it to stack? add something that tracks move multi on the mover?
    {
        public SlowVars Vars = new SlowVars(0.5f);

        public override void DoLogic(IActorHub target)
        {
            StatusEffectHelper.InflictSlow(target, Vars);

        }

        public override void UnDoLogic(IActorHub target)
        {
            StatusEffectHelper.RemoveSlow(target, Vars);
     
        }
    }
}