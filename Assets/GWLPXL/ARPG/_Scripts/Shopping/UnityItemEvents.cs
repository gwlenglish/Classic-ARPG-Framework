using UnityEngine.Events;
using GWLPXL.ARPGCore.Items.com;

namespace GWLPXL.ARPGCore.Shopping.com
{
    [System.Serializable]
    public class UnityItemEvent : UnityEvent<Item> { }

    [System.Serializable]
    public class UnityShopperEvents
    {
        public UnityItemEvent OnPurchaseComplete;
    }
    [System.Serializable]
    public class UnitySellerEvents
    {
        public UnityItemEvent OnSoldComplete;
    }
    [System.Serializable]
    public class PlayerBuyerEvents
    {
        //  public LevelUpEvent GameEvents;//on the actual attributes
        public UnityShopperEvents SceneEvents = new UnityShopperEvents();
    }

    [System.Serializable]
    public class PlayerSellerEvents
    {
        public UnitySellerEvents SceneEvents = new UnitySellerEvents();
    }


    [System.Serializable]
    public class ShopKeeperEvents
    {
        public UnityEvent OnShopOpen = new UnityEvent();
        public UnityEvent OnShopClosed = new UnityEvent();
        public UnityItemEvent OnPurchaseSuccess = new UnityItemEvent();
        public UnityItemEvent OnPurchaseFail = new UnityItemEvent();

    }

    [System.Serializable]
    public class ActorShopKeeperEvents
    {
        public ShopKeeperEvents SceneEvents = new ShopKeeperEvents();
    }

    [System.Serializable]
    public class UIShopKeeperEvents
    {
        public ShopCanvasUIEvents SceneEvents = new ShopCanvasUIEvents();
    }
    [System.Serializable]
    public class ShopCanvasUIEvents
    {
        public UnityEvent OnUIEnabled = new UnityEvent();
        public UnityEvent OnUIDisabled = new UnityEvent();
        public UnityEvent OnShopRefreshed = new UnityEvent();
        public UnityEvent OnDisplayItems = new UnityEvent();
        public UnityEvent OnHoverOverEnabled = new UnityEvent();
        public UnityEvent OnHoverOverDisabled = new UnityEvent();
        public UnityEvent OnPurchaseItem = new UnityEvent();
        public UnityItemEvent OnSetItemToPurchase = new UnityItemEvent();
    }
}