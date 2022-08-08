
using UnityEngine;

namespace GWLPXL.ARPGCore.Items.com
{
    public interface IUseEnchanterCanvas
    {
        bool GetFreezeMover();
        IEnchanterCanvas GetEnchanterCanvas();
        void SetShopCanvass(IEnchanterCanvas enchantercanvas);
    }

    public class PlayerEnchantingCanvasUser : MonoBehaviour, IUseEnchanterCanvas
    {

        IEnchanterCanvas enchanterui;
        public bool GetFreezeMover() => GetEnchanterCanvas() != null && GetEnchanterCanvas().GetCanvasEnabled() && GetEnchanterCanvas().GetFreezeMover();

        public IEnchanterCanvas GetEnchanterCanvas()
        {
            return enchanterui;
        }

        public void SetShopCanvass(IEnchanterCanvas enchantercanvas)
        {
            enchanterui = enchantercanvas;
        }
    }
}