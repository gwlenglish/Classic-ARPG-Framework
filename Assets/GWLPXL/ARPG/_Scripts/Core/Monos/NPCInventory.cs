
using UnityEngine;

namespace GWLPXL.ARPGCore.Items.com
{

    public class NPCInventory : MonoBehaviour, IInventoryUser
    {
        [SerializeField]
        ActorInventory template = null;
        ActorInventory runtime = null;


        public ActorInventory GetInventoryRuntime()
        {
            if (runtime == null)
            {
                SetRuntimeInventory(Instantiate(template));
                
            }
            return runtime;
        }

        public ActorInventory GetInvtemplate()
        {
            return template;
        }

        public GameObject GetMyInstance()
        {
            return this.gameObject;
        }

        public void SetInventoryTemplate(ActorInventory newTemplate)
        {
            template = newTemplate;
        }

        public void SetRuntimeInventory(ActorInventory newInv)
        {
            runtime = newInv;
            runtime.InitialSetup();
            runtime.EquipStarting();
        }
    }
}