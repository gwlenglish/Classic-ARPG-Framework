
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;
namespace GWLPXL.ARPGCore.Items.com

{


    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Potions/NEW_RestoreResource")]

    public class RestoreResource : Potion
    {
        [Header("Potion Unique")]
        [SerializeField]
        protected ResourceType resource;
        [SerializeField]
        protected int restoreAmount;
        protected ItemType itemType = ItemType.Potions;
        protected PotionType potType = PotionType.RestoreResource;
        protected bool stacking = true;
        public virtual ResourceType GetResourceType()
        {
            return resource;
        }
        public virtual int GetRestoreAmount()
        {
            return restoreAmount;
        }
        public override ItemType GetItemType()
        {
            return itemType;
        }

        public override PotionType GetPotionType()
        {
            return potType;
        }

        public override string GetUserDescription()
        {
            return GetGeneratedItemName() + ":" + "\n" + restoreAmount.ToString();
        }


        public override void UsePotion(IAttributeUser onStatUser, IInventoryUser usersInventory, int inventorySlot)
        {
            onStatUser.GetRuntimeAttributes().ModifyNowResource(resource, restoreAmount);
           // onStatUser.GetRuntimeAttributes().ModifyMaxResource(ResourceType.Health, 15);
            RemoveItemFromInventory(this, usersInventory.GetInventoryRuntime(), inventorySlot);
        }

        public override bool IsStacking()
        {
            return stacking;
        }
    }
}