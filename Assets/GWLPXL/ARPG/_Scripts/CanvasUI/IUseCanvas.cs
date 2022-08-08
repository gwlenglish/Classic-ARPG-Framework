using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public interface IUseCanvas
    {
        void SetActorHub(IActorHub hub);
        void SetUserToCanvas();
        bool GetCanvasEnabled();
        /// <summary>
        /// Stop the mover when the UI is open.
        /// </summary>
        /// <returns></returns>
        bool GetFreezeMover();
    }
}
