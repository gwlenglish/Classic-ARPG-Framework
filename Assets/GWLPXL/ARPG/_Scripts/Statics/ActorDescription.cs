using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
namespace GWLPXL.ARPGCore.Statics.com
{


    public static class ActorDescription
    {
        static StringBuilder sb = new StringBuilder();
       public static string GetResourceTotalDescription(IAttributeUser forUser, ResourceType type)
        {
            sb.Clear();
            sb.Append(forUser.GetRuntimeAttributes().GetResourceNowValue(type) + " / " + forUser.GetRuntimeAttributes().GetResourceMaxValue(type));
            return sb.ToString();
        }
        public static string GetStatDescription(IAttributeUser forUser, StatType type)
        {
            sb.Clear();
            sb.Append(forUser.GetRuntimeAttributes().GetStatNowValue(type));
            return sb.ToString();
        }
        public static string GetOtherDescription(IAttributeUser forUser, OtherAttributeType type)
        {
            sb.Clear();
            sb.Append(forUser.GetRuntimeAttributes().GetOtherAttributeNowValue(type));
            return sb.ToString();
        }
    }

    public class AttributeDescriptions
    {
        StringBuilder sb = new StringBuilder();
        ActorAttributes mine = null;
        public AttributeDescriptions(ActorAttributes myatt)
        {
            mine = myatt;
        }
        public virtual string GetResourceTotalDescription(ResourceType type)
        {
            sb.Clear();
            sb.Append(mine.GetResourceNowValue(type) + " / " + mine.GetResourceMaxValue(type));
            return sb.ToString();
        }
        public virtual string GetStatDescription(StatType type)
        {
            sb.Clear();
            sb.Append(mine.GetStatNowValue(type));
            return sb.ToString();
        }
        public virtual string GetOtherDescription( OtherAttributeType type)
        {
            sb.Clear();
            sb.Append(mine.GetOtherAttributeNowValue(type));
            return sb.ToString();
        }
    }

    public class AttributePercents
    {

        ActorAttributes mine = null;
        public AttributePercents(ActorAttributes myatt)
        {
            mine = myatt;
        }
        public virtual float GetResourcePercent(ResourceType type)
        {
            return (float)mine.GetResourceNowValue(type) / (float)mine.GetResourceMaxValue(type);
        }
       
    }

    public class AttributeConditions
    {

        ActorAttributes mine = null;
        public AttributeConditions(ActorAttributes myatt)
        {
            mine = myatt;
        }
        public virtual bool IsDead(ResourceType healthResource)
        {
            return mine.GetResourceNowValue(healthResource) <= 0;
        }
        public virtual bool HasResource(ResourceType resource)
        {
            return mine.GetResourceNowValue(resource) > 0;
        }


    }
}