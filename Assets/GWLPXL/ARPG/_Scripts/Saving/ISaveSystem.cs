namespace GWLPXL.ARPGCore.Saving.com
{


    public interface ISaveSystem
    {
        void SaveGame(PlayerPersistant[] players);
        void LoadGame(string saveFileName);
        string GetSaveDirectory();
        void SetSaveFileName(string newName);
        string GetSaveFileName();
        bool IsSaving();

    }
}