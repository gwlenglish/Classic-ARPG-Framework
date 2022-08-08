namespace GWLPXL.ARPGCore.Items.com
{
    public interface ISocketCanvas
    {
        void Open(IUseEnchanterCanvas user);
        void Close();
        void Toggle();
        void SetStation(SocketStation station);
        bool GetCanvasEnabled();
        bool GetFreezeMover();
    }

    public interface IUseSocketCanvas
    {
        bool GetFreezeMover();
        ISocketCanvas GetEnchanterCanvas();
        void SetShopCanvass(ISocketCanvas enchantercanvas);
    }


}