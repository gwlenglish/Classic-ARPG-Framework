using GWLPXL.ARPGCore.Types.com;

namespace GWLPXL.ARPGCore.Attributes.com
{
    [System.Serializable]
    public class Stat : Attribute
    {
        public StatType Type;
        readonly string colon = ": ";
        public override AttributeType GetAttributeType()
        {
            return AttributeType.Stat;
        }

        public override string GetDescriptiveName()
        {
            return Type.ToString();
        }

        public override string GetFullDescription()
        {
            UpdateValue();
            return GetDescriptiveName() + colon + NowValue.ToString();
        }

        public override int GetSubType() => (int)Type;
       
    }


}