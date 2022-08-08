using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Types.com;

namespace GWLPXL.ARPGCore.PlayerInput.com
{
    public interface IPlayerCanvasInputToggle
    {
        void CheckForCanvasInputs();
        void ToggleCanvas(CanvasType type);
        bool HasFreezeMoverCanvasEnabled();

        void SetInputHub(IActorHub actorhub);

    }
}