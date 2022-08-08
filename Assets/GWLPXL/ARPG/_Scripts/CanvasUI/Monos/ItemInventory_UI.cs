
using GWLPXL.ARPGCore.Classes.com;

using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Looting.com;
using GWLPXL.ARPGCore.Statics.com;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GWLPXL.ARPGCore.CanvasUI.com
{


    public class ItemInventory_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {

        [SerializeField]
        bool allowEquipmentComparison = true;
        [SerializeField]
        TextMeshProUGUI text = null;
        [SerializeField]
        Image itemImage = null;
        [SerializeField]
        Transform hoverOverPos = default;
        [SerializeField]
        GameObject lootPrefab = default;
        [SerializeField]
        ColorBlock defaultblock = default;

        IInventoryUser invUser = default;
        PlayerInventory_UI ui = default;
        Item myItem = default;
        Color startingBackgroundColor = default;
        bool clickable = default;
        int inventorySlotID = default;
  
        private void Awake()
        {
            startingBackgroundColor = GetComponent<Image>().color;
            ui = transform.root.GetComponent<PlayerInventory_UI>();
            itemImage.sprite = null;

        }


        void ResetDefaults()
        {
            clickable = false;
            text.text = "";
            itemImage.sprite = null;

            var tempColor = itemImage.color;
            tempColor.a = 0f;
            itemImage.color = tempColor;
            Button button = GetComponent<Button>();
            if (button != null)
            {
                ColorBlock temp = new ColorBlock();
                temp = defaultblock;
                button.colors = temp;
                //button.colors.highlightedColor = defaultblock.highlightedColor;

            }


        }

        public void SetUpItemUI(Item item, IInventoryUser _invUser, PlayerInventory_UI _ui, int _inventorySlotID)
        {
            ResetDefaults();
            myItem = item;
            invUser = _invUser;
            ui = _ui;
            inventorySlotID = _inventorySlotID;

            if (item != null)
            {
                text.text = item.GetGeneratedItemName();
                itemImage.color = startingBackgroundColor;
                itemImage.sprite = item.GetSprite();

                if (item is Equipment)
                {
                    ColorBlock buttonBlock = GetComponent<Button>().colors;
                    buttonBlock.highlightedColor = item.GetRarityColor();
                    Equipment equipment = item as Equipment;
                    IClassUser actorClass = invUser.GetMyInstance().GetComponent<IClassUser>();
                    if (actorClass != null && actorClass.GetMyClass() != null)
                    {
                        if (actorClass.GetMyClass().CanEquipment(equipment))
                        {

                            clickable = true;
                            GetComponent<Button>().colors = buttonBlock;
                        }
                        else
                        {
                            buttonBlock.highlightedColor = Color.red;//used to be selectedcolor, but older unity versions don't have that...
                            clickable = false;
                        }
                    }
                    else
                    {
                        clickable = true;

                        GetComponent<Button>().colors = buttonBlock;

                    }
                }
                else if (item.IsStacking())
                {
                    ItemStack stack = invUser.GetInventoryRuntime().GetItemStackBySlot(inventorySlotID);
                    string append = stack.CurrentStackSize.ToString();
                    string currentName = text.text;
                    currentName += ": " + append;
                    text.text = currentName;
                    clickable = true;
                }
            }

        }
        //ideally, this just raises an event which is handled in a seperate class that's a static that handles this anywhere.
        public void PlayerClicked()
        {
            if (myItem == null) return;
            if (clickable == false) return;
            clickable = false;
            ItemHandler.DetermineAndUseItem(myItem, invUser, inventorySlotID);
            ui.RefreshInventoryUI();
            clickable = true;

        }



        public void PlayerDiscard()
        {
            if (myItem == null) return;
            clickable = false;
            ItemHandler.DropItem(myItem, invUser, inventorySlotID);

            ui.RefreshInventoryUI();
            clickable = true;

        }


        public void OnPointerEnter(PointerEventData eventData)
        {

            if (myItem == null) return;
            if (allowEquipmentComparison && myItem is Equipment)
            {
                ui.EnableHoverOverInstance(hoverOverPos, myItem, allowEquipmentComparison);

            }
            else
            {
                ui.EnableHoverOverInstance(hoverOverPos, myItem, false);

            }
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (myItem == null) return;
            ui.DisableHoverOver();
        }



        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                PlayerClicked();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                PlayerDiscard();
            }
        }
    }
}
