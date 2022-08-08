namespace GWLPXL.ARPGCore.Saving.com
{
    public interface ISaveCanvas
    {
        void DeleteActiveFile();
        void SaveGame();
        void SetUser(IUseSaveCanvas _user);
        bool GetCanvasEnabled();
        void TogglePlayerSaveCanvas();
    }
}