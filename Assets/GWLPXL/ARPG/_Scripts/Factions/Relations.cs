using GWLPXL.ARPGCore.Types.com;
using UnityEngine;
namespace GWLPXL.ARPGCore.Factions.com
{
    [System.Serializable]
    public class Relations
    {
        public string DescriptiveName = string.Empty;
        [Tooltip("What's the relationship from Primary -> Values.")]
        public FactionTypes Primary = FactionTypes.None;
        public FactionValue[] Values = new FactionValue[0];
    }
}