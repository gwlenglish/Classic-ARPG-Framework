using GWLPXL.ARPGCore.Types.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.Factions.com
{
    [System.Serializable]
    public class FactionRequirementData
    {
        public FactionTypes Faction = FactionTypes.None;
        public int MinRelationshipValue = 0;
        [TextArea(3,5)]
        public string Description = "Null";
    }
}