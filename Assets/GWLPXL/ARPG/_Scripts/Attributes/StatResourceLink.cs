using GWLPXL.ARPGCore.Types.com;
using UnityEngine;



namespace GWLPXL.ARPGCore.Attributes.com
{
    [System.Serializable]

    public class StatResourceLink
    {
        [HideInInspector]
        public string StatName;
        public StatType Stat;
        [HideInInspector]
        public string ResourceName;
        public ResourceType Resource;
        [Tooltip("For every 1 pt of stat, increase the resource by the below amount. For instance, with a value of 10 below, a resource of health, and a link to vitality, 1 stat of vitality will increase health by 10.")]
        public int ResourcePerStatValue;

    }

}