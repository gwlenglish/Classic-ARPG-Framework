
using GWLPXL.ARPGCore.Items.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.CanvasUI.com
{


    public interface IInventoryCanvas
    {
        void SetUser(IUseInvCanvas newUser);
        void DisableHoverOver();
        void EnableHoverOverInstance(Transform atPos, Item item, bool enableComparison);
        void EnablePlayerInventoryUI(bool isEnabled);
        void TogglePlayerInventoryUI();
        void RefreshInventoryUI();
        bool GetCanvasEnabled();

    }
}