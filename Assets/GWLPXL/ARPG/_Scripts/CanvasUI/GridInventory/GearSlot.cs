using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;
using UnityEngine.UI;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public interface IGearSlot
    {
        GameObject SlotInstance { get; set; }
        UnityEngine.UI.Image SlotImage { get; set; }
        UnityEngine.UI.Image ItemInSlotImage { get; set; }
        int Identifier { get; set; }
        Equipment Equipment { get; set; }
    }


    [System.Serializable]
    public class ARPGGearSlot : IGearSlot
    {
        public Equipment Equipment { get => equipment; set => equipment = value; }
        public GameObject SlotInstance { get => slotInstance; set => slotInstance = value; }
        public int Identifier { get => (int)identifier; set => identifier = (EquipmentSlotsType)value; }
        public Image SlotImage { get => slotImage; set => slotImage = value; }
        public Image ItemInSlotImage { get => itemInSlotImage; set => itemInSlotImage = value; }

        [SerializeField]
        protected GameObject slotInstance = default;

        protected Image slotImage = default;

        protected Image itemInSlotImage = default;
        [SerializeField]
        protected EquipmentSlotsType identifier = EquipmentSlotsType.None;
        protected IInventoryPiece piece = null;
        protected Equipment equipment;
       
        public ARPGGearSlot(GameObject iteminstance, Equipment equipment, int identifier)
        {
            this.equipment = equipment;
            slotInstance = iteminstance;
            this.identifier = (EquipmentSlotsType)identifier;
            Ini(iteminstance);
        }

        protected virtual void Ini(GameObject iteminstance)
        {
            slotImage = iteminstance.GetComponent<Image>();
            ItemInSlotImage = iteminstance.transform.GetChild(0).GetComponent<Image>();

        }
    }
}