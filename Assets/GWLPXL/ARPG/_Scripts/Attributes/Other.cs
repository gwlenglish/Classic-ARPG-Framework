using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Attributes.com
{
    [System.Serializable]
    public class Other : Attribute
    {
        public OtherAttributeType Type;
        [Tooltip("Used to display to the player")]
        public string DescriptiveName;
        readonly string colon = ": ";
        readonly string percent = "%";
        public override AttributeType GetAttributeType()
        {
            return AttributeType.Other;
        }

        public override string GetDescriptiveName()
        {
            if (string.IsNullOrEmpty(DescriptiveName))
            {
                return Type.ToString();
            }
            return DescriptiveName;
        }

        public override string GetFullDescription()
        {
            UpdateValue();
            return GetDescriptiveName() + colon + NowValue + percent;
        }

        public override int GetSubType() => (int)Type;
      
    }

}