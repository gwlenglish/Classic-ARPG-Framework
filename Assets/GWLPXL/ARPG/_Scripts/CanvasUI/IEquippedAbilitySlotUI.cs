
using UnityEngine;
using GWLPXL.ARPGCore.Abilities.com;
namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public interface IReceiveAbilityDraggable
    {
        void SetDraggableManager(IAbilityDraggableManager draggablemanager);
    }
    public interface IEquippedAbilitySlotUI
    {
        GameObject GetInstance();
        void SetAbilitySlot(IAbilityUser forUser, int slot);
        void UpdateSlot();
    }
}