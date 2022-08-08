using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.CanvasUI.com
{

    //user interface for the inventory and equipment
    //saving should be a separate canvas. requires the dungeonmaster.
    //TO DO: subscribe to actions
    public class PlayerInventory_UI : MonoBehaviour, IInventoryCanvas
    {
        public IUseInvCanvas MyUserInv { get; set; }
        public bool FreezeDungeon = true;
        [SerializeField]
        Saving_UI savingUI;

        [SerializeField]
        GameObject MainPanel = null;
        [Header("Equipped Items")]
        [Tooltip("These are made ahead of time in the editor")]
        [SerializeField]
        GameObject HoverObjPrefab = null;
        [SerializeField]
        UpdateCharacteInfoUI charInfo = null;
        [SerializeField]
        Transform HoverOverParent = null;

        EquipmentSlot_UI[] equippedSlots = new EquipmentSlot_UI[0];
        GameObject hoverOverInstance = null;
        ItemInventory_UI[] inventorySlots = new ItemInventory_UI[0];

        IDescribePlayerStats describeStats = null;
        IActorHub actorhub = null;
        #region unity calls

        protected virtual void Awake()
        {
            EquipmentSlot_UI[] equipmentSlots = MainPanel.GetComponentsInChildren<EquipmentSlot_UI>();
            equippedSlots = equipmentSlots;
            ItemInventory_UI[] _inventorySlots = MainPanel.GetComponentsInChildren<ItemInventory_UI>();
            inventorySlots = _inventorySlots;
            describeStats = charInfo.GetComponent<IDescribePlayerStats>();
        }

        //protected virtual void Update()
        //{
        //    if (MyUserInv == null) return;
        //    if (MyUserInv.ToggleCanvas())
        //    {
        //        TogglePlayerInventoryUI();
        //    }

        //}
        #endregion
        #region protected
        //works for only one player instance in the scene

        //pop up text box that describes the items


        protected void EnableItemHover(Transform atPos, Item item, bool useComparison)
        {
            if (hoverOverInstance == null)
            {
                hoverOverInstance = Instantiate(HoverObjPrefab, HoverOverParent);
            }
            hoverOverInstance.transform.position = atPos.position;

            IDescribeEquipment describe = hoverOverInstance.GetComponent<IDescribeEquipment>();

            string description = item.GetUserDescription();
            describe.SetMyEquipment(null);
            describe.SetHighlightedItem(null);
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

                        EquipmentSlot comparisonEq = MyUserInv.GetUser().GetInventoryRuntime().GetEquipmentAtSlot(equipment.GetEquipmentSlot()[0]);
                        if (comparisonEq != null)
                        {
                            Equipment comparedEq = comparisonEq.EquipmentInSlots;
                            if (comparedEq != null)
                            {
                               describe.SetMyEquipment(comparedEq);
                               comparisondesc = comparisonEq.EquipmentInSlots.GetUserDescription();
                            }
                        }
                        break;
                }
            }
            else
            {
                comparisondesc = description;
                describe.DisableComparisonPanel();
            }
          

            describe.DescribeHighlightedEquipment(comparisondesc);
            hoverOverInstance.SetActive(true);
        }
        //pop up text box that describes the equipped, same as the hover


        #endregion
        #region public functions
        public bool GetCanvasEnabled()
        {
            return MainPanel.activeInHierarchy;
        }
        public void DisableHoverOver()
        {
            if (hoverOverInstance == null) return;
            hoverOverInstance.SetActive(false);
        }
        public void EnableHoverOverInstance(Transform atPos, Item item, bool comparisonEnabled)
        {
            EnableItemHover(atPos, item, comparisonEnabled);

        }
        public void EnablePlayerInventoryUI(bool isEnabled)
        {
            if (savingUI != null)
            {
                savingUI.EnableMainPanel(false);
            }
            MainPanel.SetActive(isEnabled);
            if (isEnabled)
            {
                RefreshInventoryUI();
            }
            
        }

        //change so it just knows the inventory. Allow anyone to plug into it
        public void RefreshInventoryUI()
        {

            DisableHoverOver();
            if (MyUserInv == null)
            {
                ARPGDebugger.DebugMessage("inventory has no user", this);
                return;
            }

            //only relevant for player
            for (int i = 0; i < equippedSlots.Length; i++)//do the same but for inventory
            {
                EquipmentSlot_UI ui = equippedSlots[i];
                Equipment equipment = MyUserInv.GetUser().GetInventoryRuntime().GetEquipmentInSlot(ui.Slot);
                ui.SetEquipmentInSlot(equipment, this);
            }

            //could be used for anything with inventory
            Dictionary<int, ItemStack> itemStacks = MyUserInv.GetUser().GetInventoryRuntime().GetItemStacks();
            foreach (var kvp in itemStacks)
            {
                ItemInventory_UI ui = inventorySlots[kvp.Key];
                Item item = null;
                int id = -1;
                if (kvp.Value != null)
                {
                    item = kvp.Value.Item;
                    id = kvp.Value.SlotID;
                }
                ui.SetUpItemUI(item, MyUserInv.GetUser(), this, id);

            }


            //only relevant to player character
            DisplayStats();



        }


        public virtual void SetUser(IUseInvCanvas _user)
        {
            if (_user == null)
            {
                ARPGDebugger.DebugMessage(" can't use the inventory UI without an Iuseinvcanvas", this);
                return;
            }
            MyUserInv = _user;
            IAttributeUser stats = _user.GetActorHub().MyStats;
            DisplayStats();
            EnablePlayerInventoryUI(false);
        }



        public void DisplayStats()
        {

            describeStats.DisplayStats(MyUserInv.GetActorHub());
        }

        public void TogglePlayerInventoryUI()
        {
            bool toggle = !MainPanel.activeSelf;
            EnablePlayerInventoryUI(toggle);
            if (FreezeDungeon && MainPanel.activeInHierarchy)
            {
                DungeonMaster.Instance.GetDungeonUISceneRef().SetDungeonState(0);
            }
            else if (FreezeDungeon && !MainPanel.activeInHierarchy)
            {
                DungeonMaster.Instance.GetDungeonUISceneRef().SetDungeonState(1);

            }
        }

        #endregion


    }
}