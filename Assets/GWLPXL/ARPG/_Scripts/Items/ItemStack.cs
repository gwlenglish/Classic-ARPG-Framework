

namespace GWLPXL.ARPGCore.Items.com
{

    [System.Serializable]
    public class ItemStack
    {
        public Item Item;
        public int CurrentStackSize;
        public int SlotID;
        public bool IsFull;
        public ItemStack(Item forItem, int currentstacksize, int slotID)
        {
            SlotID = slotID;
            Item = forItem;
            CurrentStackSize = currentstacksize;
            if (Item == null)
            {
                IsFull = false;
            }
            else
            {
                if (CurrentStackSize >= Item.GetStackingAmount())
                {
                    IsFull = true;
                }
                else
                {
                    IsFull = false;
                }
            }
            
        }
    }

}