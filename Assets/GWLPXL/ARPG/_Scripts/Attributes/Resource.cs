using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Attributes.com
{
    /// <summary>
    /// In resources NowValue - is just Cap for resource. We can buff with modifiers only CAP value
    /// example: 38/120 hp, 38 - ResourceNowValue, 120 - NowValue (can be buffed, instead of ResourceNowValue)
    /// </summary>
    [System.Serializable]
    public class Resource : Attribute
    {
        public ResourceType Type;
        public int ResourceNowValue { get; set; }
        readonly string colon = ": ";
        readonly string divisor = " / ";
        
        public override void Level(int newLevel, int maxLevel)
        {
            base.Level(newLevel, maxLevel);
            ResourceNowValue = Mathf.Clamp(ResourceNowValue, 0, NowValue);
        }
        public override void ModifyBaseValue(int byHowMuch)
        {
            base.ModifyBaseValue(byHowMuch);
            ResourceNowValue = Mathf.Clamp(ResourceNowValue, 0, NowValue);
        }
        public override AttributeType GetAttributeType()
        {
            return AttributeType.Resource;
        }

        public void ModifyResourceValue(int byHowMuch)
        {
            ResourceNowValue = Mathf.Clamp(ResourceNowValue + byHowMuch, 0, NowValue);
        }

        public override string GetDescriptiveName()
        {
            return Type.ToString();
        }

        public override string GetFullDescription()
        {
            UpdateValue();
            return GetDescriptiveName() + colon + ResourceNowValue.ToString() + divisor + NowValue.ToString();
        }

        public override int GetSubType() => (int)Type;
    }
}