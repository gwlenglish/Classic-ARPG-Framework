using GWLPXL.ARPGCore.Items.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public interface ISocketItemUIElement
    {
        void SetSocketItem(int slot, ActorInventory inventory);
        ItemStack GetSocketItem();
        void UpdateItem();
        void UpdateItem(int slot);


    }

    public class SocketItemUIElement : MonoBehaviour, ISocketItemUIElement
    {
        public bool DraggableInstance = false;
        public Image ThingImage = default;
        public TextMeshProUGUI ThingNameText = default;
        public TextMeshProUGUI ThingDescriptionText = default;
        int slot;


        ActorInventory inventory = null;

        public ItemStack GetSocketItem()
        {
            return inventory.GetItemStackBySlot(slot);
        }

      
        
        public void SetSocketItem(int stackSlot, ActorInventory inventory)
        {
            this.slot = stackSlot;
            this.inventory = inventory;
            Setup();
        }

        public void UpdateItem()
        {
            Setup();
        }

        public void UpdateItem(int slot)
        {
            if (this.slot !=slot) return;
            Setup();

        }

        protected virtual void Setup()
        {
            ItemStack stack = inventory.GetItemStackBySlot(slot);
            if (stack.Item == false || stack.CurrentStackSize <= 0)
            {
                if (DraggableInstance == false)
                {
                    gameObject.SetActive(false);
                }

                ThingImage.sprite = null;
                ThingNameText.SetText("Empty");
                ThingDescriptionText.SetText("Empty");
            }
            else
            {
                //here's we could filter if we want.
                if (stack.Item is SocketItem)
                {
                    ThingImage.sprite = stack.Item.GetSprite();
                    ThingNameText.SetText(stack.Item.GetGeneratedItemName());
                    ThingDescriptionText.SetText(stack.Item.GetUserDescription());
                }
                else
                {
                    gameObject.SetActive(false);
                   

                }

            }
           
  
        }

      

    }
}