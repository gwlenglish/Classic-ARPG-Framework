using System.Collections.Generic;
using System.IO;
using GWLPXL.ARPG._Scripts.Editor.EditorModels.GameDatabase.com;
using GWLPXL.ARPG._Scripts.Editor.ObjectEditors.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEditor;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.ObjectEditors.GameDatabase.com
{
    public class GameDatabaseCreateEditor: ArpgBaseEditor
    {
        public override void OnInspectorGUI()
        {
            var model = (CreateGameDatabase)target;
            
            if (model.withsubdatabases)
            {
                model.suffx = "and CREATE new sub databases?";
            }
            else
            {
                model.suffx = "and DO NOT create new sub databases.";
            }

            model.copyOtherDatabaseEntries = GUILayout.Toggle(model.copyOtherDatabaseEntries, "Copy entries from other database?");
            if (model.copyOtherDatabaseEntries)
            {
                model.toCopy = EditorGUILayout.Popup(model.toCopy, model.names.ToArray());
            }

            model.copySettings = GUILayout.Toggle(model.copySettings, "Copy Settings from other database?");
            if (model.copySettings)
            {
                model.settingsCopy = EditorGUILayout.Popup(model.settingsCopy, model.names.ToArray());
            }
            model.withsubdatabases = GUILayout.Toggle(model.withsubdatabases, model.suffx);
            GUILayout.Label("New Database Name:");
            model.defaultname = EditorGUILayout.TextField(model.defaultname);
            bool createnew = GUILayout.Button("Create New Database " + "\n" + model.suffx);


            if (createnew)
            {
                if (string.IsNullOrEmpty(model.defaultname))
                {
                    Debug.LogWarning("Need to name the new database");
                    return;
                }
  
                string path = EditorUtility.SaveFilePanelInProject("Create New Game Database ", model.defaultname, "asset", "This will create a new ARPG Game Database.");
                if (path.Length > 0)
                {
                    //do the thing
                    GWLPXL.ARPGCore.com.GameDatabase newDatabase = DatabaseHandler.CreateNewGameDatabse(path, model.withsubdatabases, model.defaultname);


                    if (model.copyOtherDatabaseEntries)
                    {
                        GWLPXL.ARPGCore.com.GameDatabase other = model.objects[model.toCopy];
                        DatabaseHandler.ReloadDatabase(other);
                        var subfoldersdic = CreateSubFolders(newDatabase, other);
                        CreateCopies(newDatabase, other, subfoldersdic);
                        ////how to cycle through them all and copy?

                    }

                    if (model.copySettings)
                    {
                        ProjectSettings copiedSettings = model.objects[model.toCopy].Settings;
                        ProjectSettings current = newDatabase.Settings;
                        ProjectSettings newCopy = Instantiate(copiedSettings);
                        // string newname = path + newCopy.name + ".asset";
                        string extension = ".asset";
                        string dpath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(newDatabase));
                        string exportName = "\\" + newCopy.name + extension;
                        AssetDatabase.CreateAsset(newCopy, dpath + exportName);
                        AssetDatabase.Refresh();
                        Object obj = AssetDatabase.LoadAssetAtPath(dpath + exportName, typeof(Object));
                        Debug.Log("Loaded " + obj);
                        newDatabase.Settings = obj as ProjectSettings;
                        EditorUtility.SetDirty(newDatabase);
                     
                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(current));
                       

                    }
                }
            }
        }
        
        
        private void CreateCopies(GWLPXL.ARPGCore.com.GameDatabase destination, GWLPXL.ARPGCore.com.GameDatabase sourceToCopy,
            Dictionary<DatabaseID, string> subfoldersdic)
        {
            foreach (var kvp in subfoldersdic)
            {
                string path = kvp.Value;
               // Debug.Log(path);
                IDatabase database = null;
                switch (kvp.Key)
                {
                    case DatabaseID.Abilities:
                        database = sourceToCopy.Abilities;
                        break;
                    case DatabaseID.AbilityControllers:
                        database = sourceToCopy.AbilityControllers;
                        break;
                    case DatabaseID.ActorDamageDealers:
                        database = sourceToCopy.ActorDamageTypes;
                        break;
                    case DatabaseID.Attributes:
                        database = sourceToCopy.Attributes;
                        break;
                    case DatabaseID.AuraControllers:
                        database = sourceToCopy.AuraControllers;
                        break;
                    case DatabaseID.Auras:
                        database = sourceToCopy.Auras;

                        break;
                    case DatabaseID.Classes:
                        database = sourceToCopy.Classes;
                        break;
                    case DatabaseID.EquipmentTraits:
                        database = sourceToCopy.Traits;
                        break;
                    case DatabaseID.Inventories:
                        database = sourceToCopy.Inventories;

                        break;
                    case DatabaseID.Items:
                        database = sourceToCopy.Items;
                        break;
                    case DatabaseID.LootDrops:
                        database = sourceToCopy.Loot;
                        break;
                    case DatabaseID.Projectiles:
                        database = sourceToCopy.Projectiles;
                        break;
                    case DatabaseID.Questchains:
                        database = sourceToCopy.Questchains;
                        break;
                    case DatabaseID.QuestLogs:
                        database = sourceToCopy.QuestLog;
                        break;
                    case DatabaseID.Quests:
                        database = sourceToCopy.Quests;
                        break;


                }

                if (database != null)
                {
                   DatabaseHandler.CreateCopies(database, path);
                }

            }
            subfoldersdic.Clear();
            DatabaseHandler.ReloadAll(destination);
        }

     

        Dictionary<DatabaseID, string> CreateSubFolders(GWLPXL.ARPGCore.com.GameDatabase newDB, GWLPXL.ARPGCore.com.GameDatabase copiedDatabtase)
        {
            string dpath = System.IO.Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(newDB));
            Debug.Log(dpath);
            Dictionary<DatabaseID, string> subfoldersdic = new Dictionary<DatabaseID, string>();
            DatabaseID[] ids = copiedDatabtase.GetDatabaseTypes();
            for (int i = 0; i < ids.Length; i++)
            {
                DatabaseID dbID = ids[i];
                if (dbID == DatabaseID.GameDatabase) continue;

                string guid = AssetDatabase.CreateFolder(dpath, dbID.ToString());
              //  Debug.Log("GUID " + guid);
                string subfolder = AssetDatabase.GUIDToAssetPath(guid);
           //     Debug.Log("Subfolder " + subfolder);

                subfoldersdic.Add(dbID, subfolder);
            }
            return subfoldersdic;
        }
    }
}