using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Shopping.com
{


    public class ShopKeeperCanvas_UI : MonoBehaviour, IShopKeeperCanvas
    {
        [Header("Events")]
        [SerializeField]
        UIShopKeeperEvents eventsUI = new UIShopKeeperEvents();
        [Header("Options")]
        [SerializeField]
        bool freezeDungeon = true;
        [SerializeField]
        bool useConfirmPanel = true;
        [SerializeField]
        bool allowRefreshButton = true;
        [SerializeField]
        GameObject refreshButtonObj = null;
        [Header("Main")]
        [Space(5)]
        [Header("Required")]
        [SerializeField]
        GameObject mainPanel = null;
        [SerializeField]
        Transform content = null;
        [SerializeField]
        ShopItem shopItemPrefab = null;
        [SerializeField]
        GameObject choicesPanel = null;
        [Header("Panels")]
        [SerializeField]
        Transform panelTab = null;
        [SerializeField]
        ShopTab shopTabPrefab = null;
        [Header("Hover")]
        [SerializeField]
        GameObject hoverOverPrefab = null;
        [SerializeField]
        Transform hoverOverParent = null;
        [Header("Misc")]
        [SerializeField]
        TMPro.TextMeshProUGUI currencyText = null;


        GameObject hoverOverInstance = null;
        IShopKeeper shopKeeper = null;
        Dictionary<ItemType, List<ShopItem>> itemInstancesDic = new Dictionary<ItemType, List<ShopItem>>();
        List<ShopTab> tabs = new List<ShopTab>();
        Item toPurchase = null;
        ItemType[] previousDisplays = new ItemType[0];
        private void Awake()
        {
            mainPanel.SetActive(false);
            choicesPanel.SetActive(false);
            if (refreshButtonObj != null)
            {
                refreshButtonObj.SetActive(allowRefreshButton);
               
            }
           
        }
        public bool GetCanvasEnabled()
        {if (mainPanel == null) return false;
            return mainPanel.activeInHierarchy;
        }
        public void SetUser(IShopKeeper newUser)
        {
            shopKeeper = newUser;
            RefreshShop();
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


            ItemType[] shopTypes = shopKeeper.GetItemTypesToSell();
            for (int i = 0; i < shopTypes.Length; i++)
            {
                ShopTab instance = Instantiate(shopTabPrefab, panelTab);
                instance.SetTypes(shopTypes[i], this);
                tabs.Add(instance);
            }
            //make new ones
            List<Item> allItems = shopKeeper.GetShopKeeperItems();
            for (int i = 0; i < allItems.Count; i++)
            {

                ShopItem instance = Instantiate(shopItemPrefab, content);
                instance.SetShopItem(allItems[i], this);
                itemInstancesDic.TryGetValue(allItems[i].GetItemType(), out List<ShopItem> value);
                if (value == null)
                {
                    value = new List<ShopItem>();
                }
                value.Add(instance);
                itemInstancesDic[allItems[i].GetItemType()] = value;
                //make a button to register the event
            }
            if (previousDisplays.Length ==0)
            {
                ItemType[] defaulttype = new ItemType[1] { shopKeeper.GetItemTypesToSell()[0] };
                DisplayItems(defaulttype);//default to first
            }
            else
            {
                DisplayItems(previousDisplays);
            }

            eventsUI.SceneEvents.OnShopRefreshed.Invoke();


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

            previousDisplays = ofTypes;

            eventsUI.SceneEvents.OnDisplayItems.Invoke();
        }
      

        public void ToggleUI()
        {
            mainPanel.SetActive(!mainPanel.activeInHierarchy);
            if (freezeDungeon)
            {
                if (mainPanel.activeInHierarchy)
                {
                    DungeonMaster.Instance.GetDungeonUISceneRef().SetDungeonState(0);
                    shopKeeper.GetShopper().GetInventory().GetInventoryRuntime().OnGoldChanged += DisplayShopperCurrency;
                    eventsUI.SceneEvents.OnUIEnabled.Invoke();

                }
                else
                {
                    DungeonMaster.Instance.GetDungeonUISceneRef().SetDungeonState(1);
                    shopKeeper.GetShopper().GetInventory().GetInventoryRuntime().OnGoldChanged -= DisplayShopperCurrency;

                    shopKeeper.CloseShop();
                    eventsUI.SceneEvents.OnUIDisabled.Invoke();
                }
            }
           
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
                        EquipmentSlot comparisonEq =  shopKeeper.GetShopper().GetInventory().GetInventoryRuntime().GetEquipmentAtSlot(equipment.GetEquipmentSlot()[0]);
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

            eventsUI.SceneEvents.OnHoverOverEnabled.Invoke();
        }

        public IShopKeeper GetShopKeeper()
        {
            return shopKeeper;
        }

        public void DisableHover()
        {
            if (hoverOverInstance == null) return;
            hoverOverInstance.SetActive(false);
            eventsUI.SceneEvents.OnHoverOverDisabled.Invoke();
        }

        public void DisplayShopperCurrency(int displayAmount)
        {
            currencyText.SetText(displayAmount.ToString());
        }

        public void SetItemToPurchase(Item item)
        {
            toPurchase = item;
            eventsUI.SceneEvents.OnSetItemToPurchase.Invoke(item);
            if (useConfirmPanel)
            {
                choicesPanel.SetActive(true);
                return;
            }
            //else just purchase
            PurchaseItem();
        }

        public void PurchaseItem()
        {
            bool success = shopKeeper.TryPurchase(toPurchase);
            if (useConfirmPanel)
            {
               
                choicesPanel.SetActive(false);
              
            }
            if (success)
            {
                eventsUI.SceneEvents.OnPurchaseItem.Invoke();
            }

        }

        public void ToggleSell()
        {
            shopKeeper.TogglePlayerSelling();
        }
    }
}