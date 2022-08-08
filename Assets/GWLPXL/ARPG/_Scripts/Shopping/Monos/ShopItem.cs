using GWLPXL.ARPGCore.Items.com;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace GWLPXL.ARPGCore.Shopping.com
{

    /// <summary>
    /// Used for buttons.
    /// </summary>
    public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        Transform hoverOverPos = null;
        [SerializeField]
        bool allowEquipmentComparison = true;
        [SerializeField]
        TextMeshProUGUI nameText = null;
        [SerializeField]
        TextMeshProUGUI costText = null;
        [SerializeField]
        UnityEngine.UI.Image itemImage = null;
        Item myItem = null;
        IShopKeeperCanvas canvas = null;
        public void SetShopItem(Item myItem, IShopKeeperCanvas shopCanvas)
        {
            this.myItem = myItem;
            canvas = shopCanvas;
            nameText.SetText(myItem.GetGeneratedItemName());
            costText.SetText(myItem.GetPurchaseCost().ToString());
            itemImage.sprite = myItem.GetSprite();
        }

        public void Purchase()
        {
            canvas.SetItemToPurchase(myItem);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {

            if (myItem == null) return;
            if (myItem is Equipment && allowEquipmentComparison)
            {
                canvas.EnableHoverOverInstance(hoverOverPos, myItem, true);
            }
            else
            {
                canvas.EnableHoverOverInstance(hoverOverPos, myItem, false);

            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (myItem == null) return;
            canvas.DisableHover();
        }
    }
}