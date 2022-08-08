
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Items.com
{



    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Equipment/Weapon/NEW_")]

    public class Weapon : Equipment
    {
        [SerializeField]
        protected WeaponType weaponType;

        protected EquipmentType type = EquipmentType.Weapon;
        protected ItemType itemType = ItemType.Equipment;


        public virtual WeaponType GetWeaponType()
        {
            return weaponType;
        }

        public override ItemType GetItemType()
        {
            return itemType;
        }
        public override EquipmentType GetEquipmentType()
        {
            return type;
        }

        public override string GetMaterialDescription()
        {
            return weaponType.ToString();
        }
    }
}