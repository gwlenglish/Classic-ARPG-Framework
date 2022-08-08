using GWLPXL.ARPGCore.Items.com;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GWLPXL.ARPGCore.Shopping.com
{


    public class SellEquipped : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        Transform hoverOverPos = null;
        [SerializeField]
        TextMeshProUGUI nameText = null;
        [SerializeField]
        TextMeshProUGUI costText = null;
        [SerializeField]
        UnityEngine.UI.Image itemImage = null;
        Equipment myItem = null;
        ISellerCanvasUI canvas = null;

        public void SetShopItem(Equipment myItem, ISellerCanvasUI shopCanvas)
        {
            this.myItem = myItem;
            canvas = shopCanvas;

            if (this.myItem == null) return;

            nameText.SetText(this.myItem.GetGeneratedItemName());
            costText.SetText(this.myItem.GetSellCost().ToString());
            itemImage.sprite = this.myItem.GetSprite();
        }

        public void Purchase()
        {
            canvas.SetToSell(myItem);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {

            if (myItem == null) return;
            canvas.EnableHoverOverInstance(hoverOverPos, myItem, false);


        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (myItem == null) return;
            canvas.DisableHover();
        }
    }
}