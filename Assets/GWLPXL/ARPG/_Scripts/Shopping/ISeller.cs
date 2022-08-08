using GWLPXL.ARPGCore.Items.com;

using GWLPXL.ARPGCore.Types.com;

using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Shopping.com
{
    public interface ISeller
    {
        void SetCanvasPrefab(GameObject newprefab);
        IInventoryUser GetSellerInventory();
        ItemType[] GetTypesToSell();
        List<ItemStack> GetSellerItems();
        List<Equipment> GetSellerEquipped();
        void ToggleCanvas();
        /// <summary>
        /// sells 1 from the stack
        /// </summary>
        /// <param name="itemStackID"></param>
        /// <returns></returns>
        bool TrySell(int itemStackID);
        bool TrySell(Equipment equipment);
        bool GetCanvasActive();

    }
}