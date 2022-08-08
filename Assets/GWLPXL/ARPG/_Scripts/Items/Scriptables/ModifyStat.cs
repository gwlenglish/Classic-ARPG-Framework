
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.StatusEffects.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;
namespace GWLPXL.ARPGCore.Items.com

{


    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Potions/NEW_ModifyStat")]

    public class ModifyStat : Potion
    {
        [Header("Potion Unique")]
        [SerializeField]
        ModifyStatType ModStatType = null;

        protected ItemType itemType = ItemType.Potions;
        protected PotionType potType = PotionType.ModifyStat;
        protected bool stacking = true;
        public virtual StatType GetStatType()
        {
            return ModStatType.Type;
        }
        public virtual int GetModAmount()
        {
            return ModStatType.ModAmount;
        }
        public override ItemType GetItemType()
        {
            return itemType;
        }

        public override PotionType GetPotionType()
        {
            return potType;
        }

        public override string GetUserDescription()
        {
            return GetGeneratedItemName() + ":" + "\n" + GetModAmount().ToString();
        }


        public override void UsePotion(IAttributeUser onStatUser, IInventoryUser usersInventory, int inventorySlot)
        {
            //creates a mono that will track the duration applied.
            ModifyStatType modType = new ModifyStatType(GetStatType(), GetModAmount(), ModStatType.Duration);
            ModStat modStat = onStatUser.GetInstance().gameObject.AddComponent<ModStat>();
            modStat.ModStatType = modType;

            RemoveItemFromInventory(this, usersInventory.GetInventoryRuntime(), inventorySlot);


        }

        public override bool IsStacking()
        {
            return stacking;
        }
    }
}