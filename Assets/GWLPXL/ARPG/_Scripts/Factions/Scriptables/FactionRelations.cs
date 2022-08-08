using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GWLPXL.ARPGCore.Statics.com;

namespace GWLPXL.ARPGCore.Factions.com
{
    [CreateAssetMenu(menuName =("GWLPXL/ARPG/Factions/NEW Faction Relations"))]
    public class FactionRelations : ScriptableObject, ISaveJsonConfig
    {
        [SerializeField]
        TextAsset config = null;
        public bool MatchNameWithType = true;
        [Tooltip("This is the starting template.")]
        public Relations[] Relations = new Relations[0];
        [Tooltip("These define static tier levels")]
        public RelationshipTier[] Tiers = new RelationshipTier[0];
        public FactionInformation[] Information = new FactionInformation[0];

        public Object GetObject() => this;


        public TextAsset GetTextAsset() => config;


        public void SetTextAsset(TextAsset textAsset) => config = textAsset;
      








#if UNITY_EDITOR
        private void OnValidate()
        {
            if (MatchNameWithType)
            {
                for (int i = 0; i < Relations.Length; i++)
                {
                    Relations[i].DescriptiveName = Relations[i].Primary.ToString();
                    for (int j = 0; j < Relations[i].Values.Length; j++)
                    {
                        Relations[i].Values[j].DescriptiveName = Relations[i].Values[j].Faction.ToString();
                    }
                }
            }
        }

#endif
    }
}