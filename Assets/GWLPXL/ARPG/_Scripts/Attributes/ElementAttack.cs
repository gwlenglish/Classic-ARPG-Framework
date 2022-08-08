using GWLPXL.ARPGCore.Types.com;

namespace GWLPXL.ARPGCore.Attributes.com
{
    /// <summary>
    /// element attack attribute
    /// </summary>
    [System.Serializable]
    public class ElementAttack : Attribute
    {
        public ElementType Type;
        protected string attack = " Attack: ";
        public ElementAttack(ElementType type)
        {
            Type = type;
        }
        public override AttributeType GetAttributeType()
        {
            return AttributeType.ElementAttack;
        }

        public override string GetDescriptiveName()
        {
            return Type.ToString();
        }

        public override string GetFullDescription()
        {
            UpdateValue();
            return GetDescriptiveName() + " Attack " + NowValue.ToString();
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