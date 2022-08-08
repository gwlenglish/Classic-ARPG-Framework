
using GWLPXL.ARPGCore.Types.com;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.Attributes.com;

namespace GWLPXL.ARPGCore.GameEvents.com
{
    public struct LevelUpEventVars
    {
        [System.NonSerialized]
        public Dictionary<AttributeType, Attribute[]> PreviousValues;
        [System.NonSerialized]
        public Dictionary<AttributeType, Attribute[]> NowValues;
        public int OldLevel;
        public int NowLevel;

        public LevelUpEventVars(Dictionary<AttributeType, Attribute[]> _previous, Dictionary<AttributeType, Attribute[]> _now, int oldLeve, int nowLevel)
        {
            PreviousValues = _previous;
            NowValues = _now;
            OldLevel = oldLeve;
            NowLevel = nowLevel;
        }
    }
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/GameEvents/NEW_LevelUp")]
    public class LevelUpEvent : GameEvent
    {
        public LevelUpEventVars EventVars;
        //[System.NonSerialized]
        //public Dictionary<AttributeType, Attribute[]> PreviousValues = new Dictionary<AttributeType, Attribute[]>();
        //[System.NonSerialized]
        //public Dictionary<AttributeType, Attribute[]> NowValues = new Dictionary<AttributeType, Attribute[]>();
        //public int OldLevel;
        //public int NewLevel;

    }
}
