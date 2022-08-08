
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Items.com
{


    public class PlayerInventory : MonoBehaviour, IInventoryUser, ISubscribeEvents
    {
        [SerializeField]
        protected PlayerInventoryEvents inventoryEvents = new PlayerInventoryEvents();
        [SerializeField]
        protected ActorInventory inventoryTemplate;
        protected ActorInventory runtimeCopy_inv;

        IAttributeUser statUser = null;
        #region private
        void Awake()
        {
            statUser = GetComponent<IAttributeUser>();
            ActorInventory temp = Instantiate(inventoryTemplate);
            SetRuntimeInventory(temp);
        }

        #endregion

        #region public
        public void SetRuntimeInventory(ActorInventory newInv)
        {
            if (runtimeCopy_inv != null)
            {
                UnSubscribeEvents();
            }
            runtimeCopy_inv = newInv;

            if (runtimeCopy_inv != null)
            {
                SubscribeEvents();
            }
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

        void OnItemAdded(Item item)
        {
            inventoryEvents.SceneEvents.OnItemAdded.Invoke(item);
        }
        void OnItemRemoved(Item item)
        {
            inventoryEvents.SceneEvents.OnItemRemoved.Invoke(item);
        }
        void OnEquip(Equipment equipped)
        {
            inventoryEvents.SceneEvents.OnEquipped.Invoke(equipped);
        }
        void OnUnEquip(Equipment unequipped)
        {
            inventoryEvents.SceneEvents.OnUnEquip.Invoke(unequipped);

        }

        public void ReApplyTraits(int level)
        {
            GetInventoryRuntime().ReApplyAllTraits(statUser);
        }
       
        public void SubscribeEvents()
        {

            GetInventoryRuntime().OnEquip += OnEquip;
            GetInventoryRuntime().OnUnEquip += OnUnEquip;
            GetInventoryRuntime().OnAddItem += OnItemAdded;
            GetInventoryRuntime().OnRemoveItem += OnItemRemoved;
        }

        public void UnSubscribeEvents()
        {


            GetInventoryRuntime().OnEquip -= OnEquip;
            GetInventoryRuntime().OnUnEquip -= OnUnEquip;
            GetInventoryRuntime().OnAddItem -= OnItemAdded;
            GetInventoryRuntime().OnRemoveItem -= OnItemRemoved;



        }

        #endregion
    }
}