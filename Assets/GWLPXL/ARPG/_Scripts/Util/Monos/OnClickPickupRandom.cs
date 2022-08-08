
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Looting.com;
using UnityEngine;
using TMPro;
namespace GWLPXL.ARPGCore.Demo.com
{


    public class OnClickPickupRandom : BareClass, IMouseClickable
    {
        public LootDrops ItemLoot;
        public int OfILevel;
        public GameObject PickerUpper;
        public TextMeshProUGUI TM;
        public void DoClick()
        {
            IInventoryUser inventory = PickerUpper.GetComponent<IInventoryUser>();
            if (inventory != null)
            {
                Item copy = ItemLoot.GetRandomDrop(OfILevel);
                Debug.Log(inventory.GetInventoryRuntime().name + " picked up " + copy.GetGeneratedItemName());
                if (TM != null)
                {
                    TM.SetText(inventory.GetInventoryRuntime().name + " picked up " + copy.GetGeneratedItemName());
                }
                inventory.GetInventoryRuntime().AddItemToInventory(copy);
            }
        }


    }
}