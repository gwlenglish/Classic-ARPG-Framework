using GWLPXL.ARPGCore.Types.com;


namespace GWLPXL.ARPGCore.Items.com
{

    [System.Serializable]
    public class EquipmentSlot
    {
        public EquipmentSlotsType slot;
        public Equipment EquipmentInSlots { get; set; }
    }

}