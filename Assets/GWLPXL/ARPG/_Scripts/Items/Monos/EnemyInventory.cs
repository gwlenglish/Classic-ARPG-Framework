
using UnityEngine;

namespace GWLPXL.ARPGCore.Items.com
{


    public class EnemyInventory : MonoBehaviour, IInventoryUser
    {
        [SerializeField]
        protected ActorInventory inventoryTemplate;
        protected ActorInventory runtimeCopy_inv;

        #region private
        void Awake()
        {
            ActorInventory temp = Instantiate(inventoryTemplate);
            SetRuntimeInventory(temp);
        }

        #endregion

        #region public
        public void SetRuntimeInventory(ActorInventory newInv)
        {
            runtimeCopy_inv = newInv;
            runtimeCopy_inv.InitialSetup();
        }
        public ActorInventory GetInventoryRuntime()
        {
            return runtimeCopy_inv;
        }
        public ActorInventory GetInvtemplate()
        {
            return inventoryTemplate;
        }

        public GameObject GetMyInstance()
        {
            return this.gameObject;
        }

        public void SetInventoryTemplate(ActorInventory newTemplate)
        {
            inventoryTemplate = newTemplate;
        }

        #endregion

    }
}