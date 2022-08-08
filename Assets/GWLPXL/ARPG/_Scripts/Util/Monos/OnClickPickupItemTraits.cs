
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using UnityEngine;
using TMPro;
namespace GWLPXL.ARPGCore.Demo.com
{


    public class OnClickPickupItemTraits : BareClass, IMouseClickable
    {
        public Equipment Equipment;
        public int OfILevel;
        public GameObject PickerUpper;
        public TextMeshProUGUI TM;

        public void DoClick()
        {
            IInventoryUser inventory = PickerUpper.GetComponent<IInventoryUser>();
            if (inventory != null)
            {

                Equipment copy = Instantiate(Equipment);
                copy.AssignEquipmentTraits(OfILevel);
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