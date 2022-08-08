using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Factions.com
{
    [System.Serializable]
    public class RelationshipTierData
    {
        public string TierName = "Default";
        public int MinValue;
        public int MaxValue;
    }
    [CreateAssetMenu(menuName = ("GWLPXL/ARPG/Factions/NEW Relationship Tier"))]

    public class RelationshipTier : ScriptableObject
    {
        public RelationshipTierData Data = new RelationshipTierData();
    
    }
}
