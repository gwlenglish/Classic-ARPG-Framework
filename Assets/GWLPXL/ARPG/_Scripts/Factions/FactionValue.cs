
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Factions.com
{
    [System.Serializable]
    public class FactionValue
    {
        public RelationshipTierData RelationTier { get; set; }
        public string DescriptiveName = string.Empty;
        public FactionTypes Faction = FactionTypes.None;
        [Tooltip("This is the starting value. At runtime, changes will be made on the runtime version")]
        public int Value = 0;//relationship value
    }
}