
using GWLPXL.ARPGCore.com;


namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public interface IActionBarUI
    {
        void EnableUI(bool isEnabled);
        void SetUser(IActorHub newUser);
        void ToggleUI();
        bool GetEnabled();
        void SetDraggableAbility(IAbilityDraggableManager dragman);
    }
}