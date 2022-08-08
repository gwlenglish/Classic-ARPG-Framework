using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections.Generic;
using GWLPXL.ARPGCore.Looting.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Shopping.com
{
    public interface IShopKeeper
    {
        int GetShopLevel();
        void SetShopLevel(int newlevel);
        void SetItemRolls(int newrollamount);
        void SetItemsToSell(ItemType[] types);
        void SetStoreTable(LootDrops newtable);
        IShopper GetShopper();
        ItemType[] GetItemTypesToSell();
        List<Item> GetShopKeeperItems();
        void SetupShop();
        void OpenShop(IShopper forShopper);
        void CloseShop();
        bool TryPurchase(Item item);
        void TogglePlayerSelling();
        Transform GetInstance();
    }
}