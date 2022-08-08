


using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.com
{
    /// <summary>
    /// Displays a configurable health bar for any object with a Damageable as a parent
    /// </summary>
    public class HealthBar : MonoBehaviour, IResourceBar
    {
        public GameObject ActorHub = null;
        MaterialPropertyBlock matBlock;
        MeshRenderer meshRenderer;
        IActorHub hub = null;
        ResourceType type;
        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            matBlock = new MaterialPropertyBlock();
            // get the damageable parent we're attached to
            // Cache since Camera.main is super slow
        }


        void UpdateParams()
        {

            meshRenderer.GetPropertyBlock(matBlock);
            matBlock.SetFloat("_Fill", hub.MyStats.GetRuntimeAttributes().Percents.GetResourcePercent(hub.MyHealth.GetHealthResource()));
            meshRenderer.SetPropertyBlock(matBlock);
        }




        public void UpdateBar()
        {
            if (hub == null) return;
            // Only display on partial health
            if (hub.MyStats.GetRuntimeAttributes().Conditions.IsDead(hub.MyHealth.GetHealthResource()))
            {
                meshRenderer.enabled = false;
               
            }
            else
            {
                meshRenderer.enabled = true;
                UpdateParams();
            }


        }


        public void SetActorHub(IActorHub newHub)
        {
            hub = newHub;
        }

        public void SetResource(ResourceType type)
        {
            this.type = type;
        }
    }
}