
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Attributes.com
{



    public class PlayerAttributes : MonoBehaviour, IAttributeUser
    {
        [SerializeField]
        protected ActorAttributes attributeTemplate;
        protected ActorAttributes runtimeCopy_attributes;
        Transform me = null;
        #region private
        void Awake()
        {
            me = GetComponent<Transform>();
            ActorAttributes temp = Instantiate(attributeTemplate);
            SetRuntimeAttributes(temp);

            foreach (StatType pieceType in System.Enum.GetValues(typeof(StatType)))
            {
                GetRuntimeAttributes().GetStatNowValue(pieceType);
            }
            GetRuntimeAttributes().LevelUp(GetRuntimeAttributes().MyLevel);
        }
        #endregion

        #region public
        public ActorAttributes GetRuntimeAttributes()
        {
            return runtimeCopy_attributes;
        }

      
        public bool DoWeHaveResources(ResourceType type, int costAmount)
        {
            float current = GetRuntimeAttributes().GetResourceNowValue(type);
            return current >= costAmount;
        }

        public void ModifyResource(ResourceType typeToModify, int byAmount)
        {
            GetRuntimeAttributes().ModifyNowResource(typeToModify, byAmount);
        }

        public ActorAttributes GetAttributeTemplate()
        {
            return attributeTemplate;
        }
        public void SetRuntimeAttributes(ActorAttributes newStats)
        {
            runtimeCopy_attributes = newStats;
        }

        public Transform GetInstance() => me;
     

        public void SetAttributeTemplate(ActorAttributes newTemplate)
        {
            attributeTemplate = newTemplate;
        }
        #endregion
    }
}