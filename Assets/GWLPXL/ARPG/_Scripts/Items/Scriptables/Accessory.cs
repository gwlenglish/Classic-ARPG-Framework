
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;
namespace GWLPXL.ARPGCore.Items.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Equipment/Accessory/NEW_")]
    //what to add for accessory derivate types? magical? physical? jewelrY? 
    public class Accessory : Equipment
    {
        [SerializeField]
        protected AccessoryType accessoryType;

        protected ItemType itemType = ItemType.Equipment;
        protected EquipmentType type = EquipmentType.Accessory;

        public virtual AccessoryType GetAccessoryType()
        {
            return accessoryType;
        }
        public override EquipmentType GetEquipmentType()
        {
            return type;
        }

        public override ItemType GetItemType()
        {
            return itemType;
        }

        public override string GetMaterialDescription()
        {
            return "";
        }


    }
}