using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Shopping.com
{
    public interface ISellerCanvasUI
    {
        void Close();
        bool GetCanvasEnabled();
        void SetUser(ISeller user);
        void ToggleUI();
        void RefreshShop();
        void DisplayItems(ItemType[] ofTypes);
        void EnableHoverOverInstance(Transform atPos, Item item, bool comparisonEnabled);
        void DisableHover();
        void SetToSell(ItemStack fromStack);
        void SetToSell(Equipment equippedEquipment);
        void SellItem();
        void DisplaySellerCurrency(int displayAmount);

    }

    public class SellCanvas_UI : MonoBehaviour, ISellerCanvasUI
    {
        [Header("Options")]
        [SerializeField]
        bool freezeDungeon = true;
        [SerializeField]
        bool useConfirmPanel = true;
        [SerializeField]
        bool includeSellerEquipped = true;

        [Space(5)]
        [Header("Main")]
        [Header("Required")]
        [SerializeField]
        GameObject mainPanel = default;
        [SerializeField]
        GameObject choicesPanel = null;
        [Header("Tab Info")]
        [SerializeField]
        Selltab sellTabPrefab = default;
        [SerializeField]
        Transform panelTab = default;
        [Header("Item Info")]
        [SerializeField]
        SellItem sellItemPrefab = default;
        [SerializeField]
        SellEquipped sellEquippedPrefab = default;  
        [SerializeField]
        Transform content = default;
        [Header("Hover")]
        [SerializeField]
        GameObject hoverOverPrefab = default;
        [SerializeField]
        Transform hoverOverParent = default;
        [Header("Misc")]
        [SerializeField]
        TMPro.TextMeshProUGUI currencyText = null;

        GameObject hoverOverInstance = null;
        ISeller seller = null;
        Dictionary<ItemType, List<GameObject>> itemInstancesDic = new Dictionary<ItemType, List<GameObject>>();
        List<Selltab> tabs = new List<Selltab>();
        ItemStack itemToSell = null;
        Equipment equippedToSell = null;
        ItemType[] lastdisplay = new ItemType[0];

        private void Awake()
        {
            mainPanel.SetActive(false);
            choicesPanel.SetActive(false);
        }
        public bool GetCanvasEnabled()
        {
            return mainPanel.activeInHierarchy;
        }
        private void OnDestroy()
        {
            if (seller == null) return;

        }
        public void SetUser(ISeller user)
        {
            seller = user;
         
            RefreshShop();
        }

        public void ToggleUI()
        {
            mainPanel.SetActive(!mainPanel.activeInHierarchy);
            if (freezeDungeon)
            {
                if (mainPanel.activeInHierarchy)
                {
                    DungeonMaster.Instance.GetDungeonUISceneRef().SetDungeonState(0);
                    RefreshShop();
                    seller.GetSellerInventory().GetInventoryRuntime().OnGoldChanged += DisplaySellerCurrency;
             
                }
                else
                {
                    DungeonMaster.Instance.GetDungeonUISceneRef().SetDungeonState(1);
                    seller.GetSellerInventory().GetInventoryRuntime().OnGoldChanged -= DisplaySellerCurrency;


                }
            }
           
        }

        public void RefreshShop()
        {
            //clear old ones
            foreach (var kvp in itemInstancesDic)
            {
                for (int i = 0; i < kvp.Value.Count; i++)
                {
                    Destroy(kvp.Value[i].gameObject);
                }
            }
            for (int i = 0; i < tabs.Count; i++)
            {
                Destroy(tabs[i].gameObject);
            }

            itemInstancesDic.Clear();
            tabs.Clear();


            ItemType[] shopTypes = seller.GetTypesToSell();
            for (int i = 0; i < shopTypes.Length; i++)
            {
                Selltab instance = Instantiate(sellTabPrefab, panelTab);
                instance.SetTypes(shopTypes[i], this);
                tabs.Add(instance);
            }

            if (includeSellerEquipped)
            {
                List<Equipment> equipment = new List<Equipment>();
                equipment = seller.GetSellerEquipped();
                for (int i = 0; i < equipment.Count; i++)
                {
                    if (equipment[i] == null) continue;
                    SellEquipped instance = Instantiate(sellEquippedPrefab, content);
                    instance.SetShopItem(equipment[i], this);
                    itemInstancesDic.TryGetValue(equipment[i].GetItemType(), out List<GameObject> value);
                    if (value == null)
                    {
                        value = new List<GameObject>();
                    }
                    value.Add(instance.gameObject);
                    itemInstancesDic[equipment[i].GetItemType()] = value;
                }
            }

            //make new ones
            List<ItemStack> allItems = seller.GetSellerItems();
                
            for (int i = 0; i < allItems.Count; i++)
            {

                SellItem instance = Instantiate(sellItemPrefab, content);
                instance.SetShopItem(allItems[i], this);
                itemInstancesDic.TryGetValue(allItems[i].Item.GetItemType(), out List<GameObject> value);
                if (value == null)
                {
                    value = new List<GameObject>();
                }
                value.Add(instance.gameObject);
                itemInstancesDic[allItems[i].Item.GetItemType()] = value;
                //make a button to register the event
            }
            if (lastdisplay.Length == 0)
            {
                lastdisplay = new ItemType[1] { seller.GetTypesToSell()[0] };
            }
            DisplayItems(lastdisplay);//default to first

            DisplaySellerCurrency(seller.GetSellerInventory().GetInventoryRuntime().GetCurrency());

        }

        public void DisplayItems(ItemType[] ofTypes)
        {

            foreach (var kvp in itemInstancesDic)
            {
                for (int i = 0; i < ofTypes.Length; i++)
                {
                    if (kvp.Key == ofTypes[i])
                    {
                        for (int j = 0; j < kvp.Value.Count; j++)
                        {
                            kvp.Value[j].gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < kvp.Value.Count; j++)
                        {
                            kvp.Value[j].gameObject.SetActive(false);
                        }
                    }
                }

            }
            lastdisplay = ofTypes;

        }

        public void EnableHoverOverInstance(Transform atPos, Item item, bool comparisonEnabled)
        {
            EnableItemHover(atPos, item, comparisonEnabled);
        }
        protected void EnableItemHover(Transform atPos, Item item, bool useComparison)
        {
            if (hoverOverInstance == null)
            {
                hoverOverInstance = Instantiate(hoverOverPrefab, hoverOverParent);
            }
            hoverOverInstance.transform.position = atPos.position;

            IDescribeEquipment describe = hoverOverInstance.GetComponent<IDescribeEquipment>();
            describe.SetHighlightedItem(null);
            describe.SetMyEquipment(null);
            describe.DisableComparisonPanel();

            string description = item.GetUserDescription();
            describe.SetHighlightedItem(item);
            describe.DescribeEquippedEquipment(description);
            string comparisondesc = "";

            if (useComparison == true)
            {
                describe.EnableComparisonPanel();
                switch (item.GetItemType())
                {
                    case ItemType.Equipment:
                        Equipment equipment = (Equipment)item;
                        EquipmentSlot comparisonEq = seller.GetSellerInventory().GetInventoryRuntime().GetEquipmentAtSlot(equipment.GetEquipmentSlot()[0]);
                        if (comparisonEq != null)
                        {
                            Equipment comparedEq = comparisonEq.EquipmentInSlots;
                            if (comparedEq != null)
                            {
                                comparisondesc = comparisonEq.EquipmentInSlots.GetUserDescription();
                                describe.SetMyEquipment(comparedEq);
                            }
                        }
                        break;
                }

            }

            describe.DescribeHighlightedEquipment(comparisondesc);
            hoverOverInstance.SetActive(true);
        }

        public void SetToSell(ItemStack toSell)
        {
            equippedToSell = null;
            itemToSell = toSell;
            if (useConfirmPanel)
            {
                choicesPanel.SetActive(true);
                return;
            }
            //else just do the sell.
        }

        public void DisableHover()
        {
            hoverOverInstance.SetActive(false);
        }

        public void Close()
        {
            mainPanel.SetActive(false);
        }

        public void SellItem()
        {
            bool sold = false;
            if (itemToSell != null)
            {
                sold = seller.TrySell(itemToSell.SlotID);
            }
            else if (equippedToSell != null)
            {
                sold = seller.TrySell(equippedToSell);   
            }
            else
            {
                Debug.LogError("Must set the item first before selling.");
            }
            if (sold)
            {
                itemToSell = null;
                equippedToSell = null;
                choicesPanel.SetActive(false);
                RefreshShop();

            }
        }

        public void SetToSell(Equipment equippedEquipment)
        {
            itemToSell = null;
            equippedToSell = equippedEquipment;
            if (useConfirmPanel)
            {
                choicesPanel.SetActive(true);
                return;
            }
        }

        public void DisplaySellerCurrency(int displayAmount)
        {
            currencyText.SetText(displayAmount.ToString());
        }
    }
}