
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Looting.com;
using GWLPXL.ARPGCore.Types.com;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GWLPXL.ARPGCore.CanvasUI.com
{


    public class EquipmentSlot_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public EquipmentSlotsType Slot = default;
        [SerializeField]
        TextMeshProUGUI text = null;
        [SerializeField]
        Image image = null;
        [SerializeField]
        Transform hoverOverPosition = null;
        [SerializeField]
        Loot lootPrefab = null;
        Button myButton = null;
        Equipment myEquipment = null;
        PlayerInventory_UI ui = null;
        ColorBlock defaultColors = default;

        private void Awake()
        {
            myButton = GetComponent<Button>();
            ColorBlock colors = myButton.colors;
            defaultColors = colors;

            //myButton.onClick.AddListener(() => this.PlayerClicked());
        }
        public void SetEquipmentInSlot(Equipment equipment, PlayerInventory_UI _ui)
        {
            myEquipment = equipment;
            ui = _ui;
            myButton = GetComponent<Button>();
            if (myEquipment != null)
            {
                text.text = equipment.GetGeneratedItemName();
                image.sprite = equipment.GetSprite();
                image.enabled = true;
                ColorBlock colors = myButton.colors;
                defaultColors = colors;
                colors.highlightedColor = equipment.GetRarityColor();
                colors.pressedColor = equipment.GetRarityColor();
                myButton.colors = colors;

            }
            else
            {
                text.text = "EMPTY";
                image.sprite = null;
                image.enabled = false;

                myButton.colors = defaultColors;

            }

        }

        //unity UI event hooked up to the button
        public void PlayerClicked()
        {
            if (myEquipment == null) return;
            if (ui.MyUserInv.GetUser().GetInventoryRuntime().CanWeAddItem(myEquipment))//auto unequip sequence
            {
                ui.MyUserInv.GetUser().GetInventoryRuntime().UnEquip(myEquipment);//if we got space in the inventory, just unequip
                                                                           //do the unequip here for wearables


            }
            else
            {
                //we don't have space, so throw it on the floor.
                PlayerDiscard();
            }

            ui.RefreshInventoryUI();


        }

        public void PlayerDiscard()
        {
            if (myEquipment == null) return;
            ui.MyUserInv.GetUser().GetInventoryRuntime().UnEquip(myEquipment);
            ui.MyUserInv.GetUser().GetInventoryRuntime().RemoveFirstItemFromInventory(myEquipment);
            ui.RefreshInventoryUI();
            //not great, but good enough for now
            float offset = Random.Range(1, 2);
            Vector3 randomPoint = ui.MyUserInv.GetUser().GetMyInstance().transform.position + Random.insideUnitSphere * offset;
            DropLoot(myEquipment, randomPoint);
        }

        GameObject DropLoot(Item forItem, Vector3 atLocation)
        {
            if (lootPrefab == null) return null;
            GameObject newDrop = Instantiate(lootPrefab.gameObject, atLocation, Quaternion.identity) as GameObject;
            newDrop.GetComponent<Loot>().LootOptions.DroppedItem = forItem;
            return newDrop;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (ui == null) return;
            if (myEquipment == null) return;
            if (myEquipment is Equipment)
            {
                ui.EnableHoverOverInstance(hoverOverPosition, myEquipment, false);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (ui == null) return;
            ui.DisableHoverOver();
        }
    }
}