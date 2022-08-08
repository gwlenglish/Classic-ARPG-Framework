using UnityEngine;

using GWLPXL.ARPGCore.Types.com;
using GWLPXL.ARPGCore.Abilities.com;

namespace GWLPXL.ARPGCore.Attributes.com
{
    [System.Serializable]
    public class AbilityMod : Attribute
    {
        public Ability Ability;
        [Tooltip("Used to display to the player")]
        public string DescriptiveName;
        readonly string colon = ": ";
        public override AttributeType GetAttributeType()
        {
            return AttributeType.AbilityMod;
        }

        public override string GetDescriptiveName()
        {
            return GetAttributeType().ToString() + " " + Ability.GetName();
          
        }

        public override string GetFullDescription()
        {
            return GetDescriptiveName() + colon + NowValue.ToString();
        }

        public override int GetSubType() => -1;
       
    }


}