using GWLPXL.ARPGCore.Abilities.com;
namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public interface IDraggableAbility
    {
        DraggableAbilityVars GetDraggableAbility();
        void SetDraggable(Ability ability, IAbilityUser forUser);
        void Drag();
    }
}