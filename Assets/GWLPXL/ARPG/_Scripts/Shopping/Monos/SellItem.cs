using GWLPXL.ARPGCore.Items.com;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace GWLPXL.ARPGCore.Shopping.com
{

    /// <summary>
    /// Used for buttons.
    /// </summary>
    public class SellItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        Transform hoverOverPos = null;
        [SerializeField]
        TextMeshProUGUI nameText = null;
        [SerializeField]
        TextMeshProUGUI costText = null;
        [SerializeField]
        UnityEngine.UI.Image itemImage = null;
        ItemStack myItem = null;
        ISellerCanvasUI canvas = null;
       
        public void SetShopItem(ItemStack myItem, ISellerCanvasUI shopCanvas)
        {
            this.myItem = myItem;
            canvas = shopCanvas;

            if (myItem == null) return;

            nameText.SetText(myItem.Item.GetGeneratedItemName());
            costText.SetText(myItem.Item.GetSellCost().ToString());
            itemImage.sprite = myItem.Item.GetSprite();
        }

        public void Purchase()
        {
            canvas.SetToSell(myItem);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {

            if (myItem == null) return;
            canvas.EnableHoverOverInstance(hoverOverPos, myItem.Item, false);

           
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (myItem == null) return;
            canvas.DisableHover();
        }
    }
}