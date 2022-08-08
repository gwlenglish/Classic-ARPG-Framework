
using UnityEngine;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Saving.com;

namespace GWLPXL.ARPGCore.Abilities.com
{
    /// <summary>
    /// database for the ability controllers
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Database/NEW_Ability Controllers")]

    public class AbilityControllerDatabase : ScriptableObject, IDatabase
    {
        [SerializeField]
        string[] searchFolders = new string[1] { "Assets/GWLPXL/ARPG" };
        [SerializeField]
        AbilityControllerDatabaseSlot[] slots = new AbilityControllerDatabaseSlot[0];
        int windowRowSize = 6;
        #region window
        public DatabaseID GetDatabaseEntry() => DatabaseID.AbilityControllers;
        public void SetSearchFolders(string[] newfolders) => searchFolders = newfolders;

        public int GetWindowRowSize() => windowRowSize;
        public string[] GetSearchFolders() => searchFolders;
        public void SetSlots(AbilityControllerDatabaseSlot[] newSlots) => slots = newSlots;
        public AbilityControllerDatabaseSlot[] GetSlots() => slots;
        public UnityEngine.Object GetDatabaseObjectBySlotIndex(int slotIndex)
        {
            if (slotIndex > slots.Length - 1 || slotIndex < 0) return null;
            return (Object)slots[slotIndex].AbilityController as UnityEngine.Object;
        }
        public ISaveJsonConfig[] GetJsons()
        {
            ISaveJsonConfig[] saves = new ISaveJsonConfig[slots.Length];
            for (int i = 0; i < slots.Length; i++)
            {
                saves[i] = slots[i] as ISaveJsonConfig;
            }
            return saves;
        }
        public string[] GetAllNames()
        {
            string[] newone = new string[slots.Length];
            for (int i = 0; i < slots.Length; i++)
            {
                newone[i] = slots[i].DescriptiveName;
            }
            return newone;
        }
        #endregion
        #region saving/loading
        public int GetAbilityControllerID(AbilityController forAbility)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (forAbility == slots[i].ID.AbilityController)
                {
                    return slots[i].ID.ID;
                }
            }
            return 0;
        }
        public AbilityController FindAbilityControllerByID(int skillID)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (skillID == slots[i].ID.ID)
                {
                    return slots[i].AbilityController as AbilityController;
                }
            }
            return null;
        }

        public Object GetMyObject() => this;

       



        #endregion

    }
    [System.Serializable]
    public class AbilityControllerID
    {
        public string Name;
        public int ID;
        public AbilityController AbilityController;

        public AbilityControllerID(string name, int id, AbilityController abilitycontroller)
        {
            Name = name;
            ID = id;
            AbilityController = abilitycontroller;
        }
    }

    [System.Serializable]
    public class AbilityControllerDatabaseSlot
    {
        public string DescriptiveName;
        public AbilityControllerID ID;
        public AbilityController AbilityController;
        public AbilityControllerDatabaseSlot(AbilityControllerID id, AbilityController abilitycontroller)
        {
            ID = id;
            AbilityController = abilitycontroller;
            ID.AbilityController = abilitycontroller;
            DescriptiveName = AbilityController.Data.Name;
            AbilityController.Data.UniqueID = id.ID;

        }
    }

}
