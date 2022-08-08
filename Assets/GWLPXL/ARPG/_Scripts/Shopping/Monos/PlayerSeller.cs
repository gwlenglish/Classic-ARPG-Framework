using GWLPXL.ARPGCore.CanvasUI.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;

using GWLPXL.ARPGCore.Types.com;

using System.Collections.Generic;

using UnityEngine;
namespace GWLPXL.ARPGCore.Shopping.com
{
    
    [RequireComponent(typeof(IInventoryUser))]
    public class PlayerSeller : MonoBehaviour, ISeller, IUseCanvas
    {
        [SerializeField]
        PlayerSellerEvents sellerEvents = new PlayerSellerEvents();
        [SerializeField]
        bool freezeMover = true;
        [SerializeField]
        ItemType[] typesToSell = new ItemType[2] { ItemType.Equipment, ItemType.Potions };
        [SerializeField]
        GameObject sellerCanvasPrefab = null;

        ISellerCanvasUI canvas = null;
        IActorHub actorhub = null;
       

        public bool GetCanvasEnabled()
        {
            if (canvas == null) return false;
            return canvas.GetCanvasEnabled();
        }

        public bool GetFreezeMover()
        {
            return freezeMover;
        }

        public void SetUserToCanvas()
        {
            if (sellerCanvasPrefab == null) return;
            GameObject obj = Instantiate(sellerCanvasPrefab, transform);
                canvas = obj.GetComponent<ISellerCanvasUI>();
            
            canvas.SetUser(this);

        }

        public void ToggleCanvas()
        {
            if (canvas == null) return;
            canvas.ToggleUI();
        }

        public bool TrySell(int itemStackID)
        {
            //need to determine how to sell and how to sell stacks by slot. 
            ItemStack stack = GetSellerInventory().GetInventoryRuntime().GetItemStackBySlot(itemStackID);
            if (stack.CurrentStackSize <= 0)
            {
                //we have none
                return false;
            }
            GetSellerInventory().GetInventoryRuntime().ModifyCurrency(stack.Item.GetSellCost());
            GetSellerInventory().GetInventoryRuntime().RemoveItemFromInventory(stack.SlotID);

            RaiseSceneEvents(stack.Item);

            if (canvas != null)
            {
                canvas.DisplaySellerCurrency(GetSellerInventory().GetInventoryRuntime().GetCurrency());
            }
            return true;

        }

        private void RaiseSceneEvents(Item sold)
        {
            sellerEvents.SceneEvents.OnSoldComplete.Invoke(sold);
        }

        public bool TrySell(Equipment equipment)
        {
            GetSellerInventory().GetInventoryRuntime().UnEquip(equipment);
            List<ItemStack> stacks = GetSellerInventory().GetInventoryRuntime().GetAllItemStacks(equipment);//should onyly ever return 1 with equipment, watch carefully...
            if (stacks == null || stacks.Count == 0)
            {
                //somehoe we made it her ebut we have nothing to sell
                return false;
            }
            GetSellerInventory().GetInventoryRuntime().RemoveItemFromInventory(stacks[0].SlotID);
            GetSellerInventory().GetInventoryRuntime().ModifyCurrency(equipment.GetSellCost());

            RaiseSceneEvents(stacks[0].Item);

            if (canvas != null)
            {
                canvas.DisplaySellerCurrency(GetSellerInventory().GetInventoryRuntime().GetCurrency());
            }
            return true;
            //give money

        }

        public IInventoryUser GetSellerInventory()
        {
            return actorhub.MyInventory;
        }

        public ItemType[] GetTypesToSell()
        {
            return typesToSell;
        }

        public List<ItemStack> GetSellerItems()
        {
            ///items stacks
            ///
            return GetSellerInventory().GetInventoryRuntime().GetAllUniqueStacks();
            

        }

        public List<Equipment> GetSellerEquipped()
        {
            List<Equipment> wearing = new List<Equipment>();
            Dictionary<EquipmentSlotsType, EquipmentSlot> temp = GetSellerInventory().GetInventoryRuntime().GetEquippedEquipment();
            foreach (var kvp in temp)
            {
                wearing.Add(kvp.Value.EquipmentInSlots);
            }
            return wearing;
        }

        public bool GetCanvasActive()
        {
            if (canvas == null) return false;
            return canvas.GetCanvasEnabled();
        }

        public void SetCanvasPrefab(GameObject newprefab) => sellerCanvasPrefab = newprefab;

        public void SetActorHub(IActorHub hub)
        {
            actorhub = hub;
        }
    }
}