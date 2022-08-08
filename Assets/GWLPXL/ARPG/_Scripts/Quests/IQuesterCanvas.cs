namespace GWLPXL.ARPGCore.Quests.com
{
    public interface IQuesterCanvas
    {
        void SetUser(IUseQuesterCanvas newUser);
        void ToggleQuesterUI();
        void RefreshQuesterUI();
        bool GetCanvasEnabled();
    }
}