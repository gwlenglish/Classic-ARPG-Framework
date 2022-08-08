using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.Items.com;
using UnityEngine.UI;
using TMPro;
namespace GWLPXL.ARPGCore.CanvasUI.com
{
    /// <summary>
    /// for invnetory -> ui
    /// </summary>
    public interface IEnchantableUIElement
    {
        void SetEnchantableItem(int slot, ActorInventory inventory);
        ItemStack GetEnchantable();
        void UpdateItem();
        void UpdateItem(int slot);
    }

    public class EnchantableUIElement : MonoBehaviour, IEnchantableUIElement
    {
        public TextMeshProUGUI ItemDescriptionText = null;
        public Image ItemImage;
        public string EmptyText = "Empty";
        public Sprite EmptySprite = null;
        protected int slot;
        protected ActorInventory inv;

        private void Awake()
        {
            ItemDescriptionText.SetText(string.Empty);
            ItemImage.sprite = null;
        }
        public void SetEnchantableItem(int slot, ActorInventory inventory)
        {
            this.slot = slot;
            this.inv = inventory;
            Setup();
   
        }

        public ItemStack GetEnchantable()
        {
            return inv.GetItemStackBySlot(slot);
        }

        public void UpdateItem()
        {
            Setup();
        }

        public void UpdateItem(int slot)
        {
            if (this.slot != slot) return;
            Setup();
        }

        protected virtual void Setup()
        {
            if (slot < 0)
            {
                gameObject.SetActive(false);
                return;
            }
            ItemStack stack = inv.GetItemStackBySlot(slot);
            if (stack.Item == false || stack.CurrentStackSize <= 0)
            {
                gameObject.SetActive(false);

                ItemImage.sprite = EmptySprite;
                ItemDescriptionText.SetText(EmptyText);
            }
            else
            {
                //here's we could filter if we want.
                if (stack.Item is Equipment)
                {
                    ItemImage.sprite = stack.Item.GetSprite();
                    ItemDescriptionText.SetText(stack.Item.GetUserDescription());
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);


                }

            }


        }
    }
}