
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Items.com

{
    //change potion to consumable 
    public abstract class Potion : Item
    {
        [Header("Potion Info")]

        [SerializeField]
        protected string potionName = "NULL";
        [SerializeField]
        protected int stackingAmount = 1;
        public override string GetBaseItemName()
        {
            return potionName;
        }
        public abstract void UsePotion(IAttributeUser onStatUser, IInventoryUser usersInventory, int inventorySlot);
        public override void SetGeneratedItemName(string newName)
        {
            potionName = newName;
        }
        public override int GetStackingAmount()
        {
            return stackingAmount;
        }
        public override string GetGeneratedItemName()
        {
            return potionName;
        }
        
        public abstract PotionType GetPotionType();



    }
}