using GWLPXL.ARPGCore.Types.com;
namespace GWLPXL.ARPGCore.Attributes.com
{
    /// <summary>
    /// element resist
    /// </summary>
    [System.Serializable]
    public class ElementResist : Attribute
    {
        public ElementType Type;
        readonly string resist = " Resist: ";
        public override AttributeType GetAttributeType()
        {
            return AttributeType.ElementResist;
        }

        public override string GetDescriptiveName()
        {
            return Type.ToString();
        }

        public override string GetFullDescription()
        {
            UpdateValue();
            return GetDescriptiveName() + resist + NowValue.ToString();
        }


        public override void Level(int newLevel, int maxLevel)
        {
            base.Level(newLevel, maxLevel);

        }

        protected override int GetLeveledValue(int forLevel, int myMaxLevel)
        {
            return base.GetLeveledValue(forLevel, myMaxLevel);
        }

        public override int GetSubType() => (int)Type;
        
    }


}