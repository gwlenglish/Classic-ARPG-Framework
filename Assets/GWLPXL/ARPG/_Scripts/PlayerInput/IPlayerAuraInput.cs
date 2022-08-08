using GWLPXL.ARPGCore.Auras.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.PlayerInput.com
{
    public interface IPlayerAuraInput
    {
        void SetActorHub(IActorHub newhub);
        Aura GetFirstAuraToggle();
    }
}