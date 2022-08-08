using GWLPXL.ARPGCore.Abilities.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public interface IUseAbilityInventory
    {
        void SetCanvasPrefab(GameObject newprefab);
        void ToggleCanvas();
        IAbilityUser GetUser();
        IAbilityInventoryUI GetAbilityInventoryUI();
        void EnableCanvas();
        void DisableCanvas();
        bool GetFreezeMover();
    }
}