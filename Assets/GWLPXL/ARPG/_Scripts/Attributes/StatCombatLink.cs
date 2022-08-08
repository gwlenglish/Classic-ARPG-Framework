using GWLPXL.ARPGCore.Types.com;
using UnityEngine;
namespace GWLPXL.ARPGCore.Attributes.com
{
    [System.Serializable]
    public class StatCombatLink
    {
        [HideInInspector]
        public string StatName;
        public StatType Stat;
        [HideInInspector]
        public string CombatName;
        public CombatStatType Combat;
        public int ValuePerStat;
    }

}