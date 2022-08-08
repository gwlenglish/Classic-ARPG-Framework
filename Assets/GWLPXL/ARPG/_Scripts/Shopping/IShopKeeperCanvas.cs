using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Shopping.com
{
    public interface IShopKeeperCanvas
    {
        IShopKeeper GetShopKeeper();
        void SetUser(IShopKeeper newUser);
        void ToggleUI();
        void RefreshShop();
        bool GetCanvasEnabled();
        void DisplayItems(ItemType[] ofTypes);
        void EnableHoverOverInstance(Transform atPos, Item item, bool comparisonEnabled);
        void DisableHover();
        void DisplayShopperCurrency(int displayAmount);
        void SetItemToPurchase(Item item);
        void PurchaseItem();
        void ToggleSell();
    }
}