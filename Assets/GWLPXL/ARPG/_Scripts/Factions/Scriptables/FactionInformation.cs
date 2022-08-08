using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Factions.com
{
    [System.Serializable]
    public class FactionInformation
    {
        public string DescriptiveName;
        public FactionTypes Faction;
        public Sprite FactionLogo;
        [TextArea(3, 5)]
        public string FactionBio;
    }
}