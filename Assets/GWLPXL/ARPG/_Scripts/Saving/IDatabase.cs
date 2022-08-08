using GWLPXL.ARPGCore.Statics.com;

namespace GWLPXL.ARPGCore.Saving.com
{
    /// <summary>
    /// reserve 0 = 100 for me. Use anything past 100
    /// </summary>
    public enum DatabaseID
    {
        Abilities = 0,
        AbilityControllers = 1,
        Attributes = 5,
        Auras = 10,
        AuraControllers = 11,
        Classes = 15,
        Inventories = 20,
        Items = 25,
        LootDrops = 26,
        Questchains = 30,
        Quests = 35,
        QuestLogs = 36,
        EquipmentTraits = 40,
        ActorDamageDealers = 41,
        Projectiles = 42,
        Melees = 43,
        GameDatabase = 100
    }
    public interface IDatabase
    {
        void SetSearchFolders(string[] newfolders);
        string[] GetSearchFolders();
        string[] GetAllNames();
        ISaveJsonConfig[] GetJsons();
        UnityEngine.Object GetDatabaseObjectBySlotIndex(int slotIndex);
        int GetWindowRowSize();
        DatabaseID GetDatabaseEntry();

        UnityEngine.Object GetMyObject();

    }

}
