using GWLPXL.ARPGCore.Items.com;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public interface IItemViewUI
    {
        void SetItemStack(ItemStack toview);
        ItemStack GetViewStack();
        void SetItem(Item toView);
 
    }

    public class ItemView : MonoBehaviour, IItemViewUI
    {
        public TextMeshProUGUI ItemDescriptionText = null;
        public Image ItemImage;
        public string EmptyText = "Empty";
        public Sprite EmptySprite = null;
        ItemStack stack;
        Item item;
        public ItemStack GetViewStack()
        {
            return stack;
        }

        public void SetItem(Item toView)
        {
            this.item = toView;
            Setup(toView);

        }
        public void SetItemStack(ItemStack toview)
        {
            this.stack = toview;
            Setup(toview);
        }
        protected virtual void Setup(Item toview)
        {
            if (toview == null)
            {

                ItemDescriptionText.SetText("");
                ItemImage.sprite = EmptySprite;
            }

            else
            {
                ItemDescriptionText.SetText(toview.GetUserDescription());
                ItemImage.sprite = toview.GetSprite();

            }
        }
        protected virtual void Setup(ItemStack toview)
        {
            if (toview == null)
            {

                ItemDescriptionText.SetText("");
                ItemImage.sprite = EmptySprite;
            }
            else
            {
                Setup(toview.Item);
            }
           
        }
    }
}