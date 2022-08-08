using UnityEngine;


namespace GWLPXL.ARPGCore.Combat.com
{
    [System.Serializable]
   public class DamageOptions
    {
        public bool InflictPhysicalDmg => physdmg;
        public bool InfictElementalDmg => elemdg;
        public bool InflictSoT => sotmod;

        [SerializeField]
        [Tooltip("Should we inflict physical damage?")]
        bool physdmg = true;
        [Tooltip("Should we inflict elemental damage?")]
        [SerializeField]
        bool elemdg = true;
        [SerializeField]
        [Tooltip("Should we inflict Status Over Time (SOT)?")]
        bool sotmod = true;
        public DamageOptions(bool phys, bool ele, bool sot)
        {
            physdmg = phys;
            elemdg = ele;
            sotmod = sot;
        }
        
       
    }
}