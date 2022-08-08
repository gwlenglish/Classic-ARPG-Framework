using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.PlayerInput.com
{
    public interface IAbilityPlayerInput
    {
        Ability GetFirstBasicAttack();
        Ability GetFirstAbilityInput();
        bool GetForceAbility();
        void SetActorHub(IActorHub newhub);
        bool GetAbilityInput(Ability forability);

    }
}