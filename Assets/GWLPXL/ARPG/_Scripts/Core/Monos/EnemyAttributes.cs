
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Attributes.com
{


    public class EnemyAttributes : MonoBehaviour, IAttributeUser
    {

        [SerializeField]
        protected ActorAttributes statsTemplate;
        protected ActorAttributes runtimeCopy_stats;

        protected virtual void Awake()
        {
            ActorAttributes temp = Instantiate(statsTemplate);
            SetRuntimeAttributes(temp);

            foreach (StatType pieceType in System.Enum.GetValues(typeof(StatType)))
            {
                GetRuntimeAttributes().GetStatNowValue(pieceType);
            }
        }


        public virtual ActorAttributes GetAttributeTemplate()
        {
            return statsTemplate;
        }
        public virtual void SetRuntimeAttributes(ActorAttributes newStats)
        {
            runtimeCopy_stats = newStats;
        }
        public virtual ActorAttributes GetRuntimeAttributes()
        {
            return runtimeCopy_stats;
        }

        public virtual Transform GetInstance()
        {
            return transform;
        }

        public virtual void SetAttributeTemplate(ActorAttributes newTemplate)
        {
            statsTemplate = newTemplate;
        }
    }
}