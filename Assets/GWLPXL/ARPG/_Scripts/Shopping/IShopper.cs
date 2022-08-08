using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Shopping.com
{
    public interface IShopper
    {
        void SetActorHub(IActorHub newHub);
        IInventoryUser GetInventory();
        bool PurchaseItem(Item item, int cost);
        int GetCurrencyAmount();
        void OpenInventory();
        void CloseInventory();
        Transform GetInstance();
    }
}