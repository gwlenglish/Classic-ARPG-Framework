
using UnityEngine;

namespace GWLPXL.ARPGCore.CanvasUI.com
{

    public interface IUseSocketSmithCanvas
    {
        bool GetFreezeMover();
        ISocketSmithCanvas GetSocketSmithCanvas();
        void SetCanvasSmithCanvas(ISocketSmithCanvas socketsmithcanvas);
    }
    public class PlayerSocketCanvasUser : MonoBehaviour, IUseSocketSmithCanvas
    {

        ISocketSmithCanvas canvas = null;
        public ISocketSmithCanvas GetSocketSmithCanvas()
        {
            return canvas;
        }

        public bool GetFreezeMover()
        {
            return GetSocketSmithCanvas() != null && GetSocketSmithCanvas().GetCanvasEnabled() && GetSocketSmithCanvas().GetFreezeMover();
        }

        public void SetCanvasSmithCanvas(ISocketSmithCanvas socketsmithcanvas)
        {
            this.canvas = socketsmithcanvas;
        }

       
    }
}
