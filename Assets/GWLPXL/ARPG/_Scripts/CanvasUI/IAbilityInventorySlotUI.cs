using GWLPXL.ARPGCore.Abilities.com;
using UnityEngine;
namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public interface IAbilityInventorySlotUI
    {
        GameObject GetInstance();
        void SetAbilitySlot(IAbilityUser forUser, int slot, IAbilityDraggableManager dragman);
        void UpdateSlot();
    }
}