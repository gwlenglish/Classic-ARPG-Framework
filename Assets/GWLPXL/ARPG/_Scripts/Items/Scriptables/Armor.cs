
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Items.com
{


    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Equipment/Armor/NEW_")]

    public class Armor : Equipment
    {
        [SerializeField]
        protected ArmorMaterial armorMat;

        protected EquipmentType type = EquipmentType.Armor;
        protected ItemType itemType = ItemType.Equipment;

        public virtual ArmorMaterial GetArmorMat()
        {
            return armorMat;
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
            return armorMat.ToString();
        }


    }
}