using GWLPXL.ARPGCore.Types.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.Attributes.com
{


    public class NPCAttributes : MonoBehaviour, IAttributeUser
    {
        [SerializeField]
        ActorAttributes template = null;
        ActorAttributes runtime = null;

        private void Awake()
        {
            ActorAttributes temp = Instantiate(template);
            SetRuntimeAttributes(temp);

            foreach (StatType pieceType in System.Enum.GetValues(typeof(StatType)))
            {
                GetRuntimeAttributes().GetStatNowValue(pieceType);
            }
            GetRuntimeAttributes().LevelUp(GetRuntimeAttributes().MyLevel);
        }

        public bool DoWeHaveResources(ResourceType type, int costAmount)
        {
            return false;
        }

        public Transform GetInstance()
        {
            return this.transform;
        }

        public ActorAttributes GetRuntimeAttributes()
        {
            return runtime;
        }

        public ActorAttributes GetAttributeTemplate()
        {
            return template;
        }

        public void SetRuntimeAttributes(ActorAttributes newStats)
        {
            runtime = newStats;
        }

        public void SetAttributeTemplate(ActorAttributes newTemplate)
        {
            template = newTemplate;
        }

        void IAttributeUser.SetAttributeTemplate(ActorAttributes newTemplate)
        {
            template = newTemplate;
        }
    }
}