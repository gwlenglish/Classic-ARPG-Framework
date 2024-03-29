﻿
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using UnityEngine;
using TMPro;
namespace GWLPXL.ARPGCore.Demo.com
{


    public class OnClickPickupItem : BareClass, IMouseClickable
    {
        public Item ItemToPickup;
        public GameObject PickerUpper;
        public TextMeshProUGUI TM;
        public void DoClick()
        {

            IInventoryUser inventory = PickerUpper.GetComponent<IInventoryUser>();

            Item copy = Instantiate(ItemToPickup);
            Debug.Log(inventory.GetInventoryRuntime().name + " picked up " + copy.GetGeneratedItemName());
            if (TM != null)
            {
                TM.SetText(inventory.GetInventoryRuntime().name + " picked up " + copy.GetGeneratedItemName());
            }
            inventory.GetInventoryRuntime().AddItemToInventory(copy);


        }

    }
}