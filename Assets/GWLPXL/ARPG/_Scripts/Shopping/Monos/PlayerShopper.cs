using GWLPXL.ARPGCore.CanvasUI.com;
using GWLPXL.ARPGCore.Items.com;
using UnityEngine.Events;
using UnityEngine;
using GWLPXL.ARPGCore.com;
namespace GWLPXL.ARPGCore.Shopping.com
{
    public interface IUseShopKeeperCanvas
    {
        bool GetFreezeMover();
        IShopKeeperCanvas GetShopKeeperCanvas();
        void SetShopCanvass(IShopKeeperCanvas shoppercanvas);
    }


    public class PlayerShopper : MonoBehaviour, IShopper, IUseShopKeeperCanvas
    {
        [SerializeField]
        PlayerBuyerEvents shopperEvents = new PlayerBuyerEvents();
        [SerializeField]
        bool freezeMoverWhileShopping = true;
        IUseInvCanvas canvas = null;
        IShopKeeperCanvas shopcanvas = null;
        IActorHub hub;
        public int GetCurrencyAmount()
        {
            return GetInventory().GetInventoryRuntime().GetCurrency();
        }

        public bool PurchaseItem(Item item, int cost)
        {
            if (hub.MyInventory.GetInventoryRuntime().CanWeAddItem(item))
            {
                cost = Mathf.Abs(cost);//a precaution in case we pass negative numbers
                hub.MyInventory.GetInventoryRuntime().ModifyCurrency(-cost);
                hub.MyInventory.GetInventoryRuntime().AddItemToInventory(item);

                RaisePurchaseEvents(item);
                return true;
            }
            return false;//cant add, for whatever reason. Space issues, e.g.
        }

        private void RaisePurchaseEvents(Item item)
        {
            if (shopperEvents == null) return;
            shopperEvents.SceneEvents.OnPurchaseComplete.Invoke(item);
        }

        public IInventoryUser GetInventory()
        {
            return hub.MyInventory;
        }

        public void OpenInventory()
        {
            

                hub.PlayerControlled.CanvasHub.InvCanvas.EnableCanvas();
            
        }

        public void CloseInventory()
        {
            hub.PlayerControlled.CanvasHub.InvCanvas.DisableCanvas();

        }

        public bool GetFreezeMover() => GetShopKeeperCanvas() != null && GetShopKeeperCanvas().GetCanvasEnabled() && freezeMoverWhileShopping;



        public IShopKeeperCanvas GetShopKeeperCanvas() => shopcanvas;

        public void SetShopCanvass(IShopKeeperCanvas shoppercanvas) => shopcanvas = shoppercanvas;

        public Transform GetInstance() => this.transform;

        public void SetActorHub(IActorHub newHub)
        {
            hub = newHub;
            
        }
    }
}