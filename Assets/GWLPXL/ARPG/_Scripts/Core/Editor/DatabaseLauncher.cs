#if UNITY_EDITOR
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace GWLPXL.ARPGCore.com
{
    public class DatabaseLauncher : EditorWindow
    {
        Vector2 scroll;
        bool withsubdatabases = true;
        bool copyOtherDatabaseEntries = true;
        bool copySettings = true;
        int toCopy = 0;
        int settingsCopy = 0;
        string suffx = string.Empty;
        string defaultname = "ARPG Game Database";

        List<string> names = new List<string>();
        List<GameDatabase> objects = new List<GameDatabase>();
        GameDatabase template = null;
        string key = string.Empty;

        Dictionary<DatabaseID, string> subfoldersdic = new Dictionary<DatabaseID, string>();
        private void OnEnable()
        {
            template = ScriptableObject.CreateInstance<GameDatabase>();
            key = template.GetType().Name;
            string[] percents = UnityEditor.AssetDatabase.FindAssets("t:" + key);//specific if you want by putting t:armor or t:equipment, etc.
            List<GameDatabase> temp = new List<GameDatabase>();
            foreach (var guid in percents)
            {
                string obj = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                GameDatabase newItem = UnityEditor.AssetDatabase.LoadAssetAtPath(obj, typeof(GameDatabase)) as GameDatabase;
                if (newItem != null && objects.Contains(newItem) == false)
                {
                    objects.Add(newItem);
                    names.Add(newItem.name);
                    //  Debug.Log("Added");
                }
            }
        }

        private void OnProjectChange()
        {
            string[] percents = UnityEditor.AssetDatabase.FindAssets("t:" + key);//specific if you want by putting t:armor or t:equipment, etc.
            List<GameDatabase> temp = new List<GameDatabase>();
            foreach (var guid in percents)
            {
                string obj = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                GameDatabase newItem = UnityEditor.AssetDatabase.LoadAssetAtPath(obj, typeof(GameDatabase)) as GameDatabase;
                if (newItem != null && objects.Contains(newItem) == false)
                {
                    objects.Add(newItem);
                    names.Add(newItem.name);
                    //  Debug.Log("Added");
                }
            }

            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] == null) objects.RemoveAt(i);
            }   
        }
        private void OnGUI()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] == null) objects.RemoveAt(i);
            }    

            Rect rect = new Rect(10, 10, 312, 512);
            GUILayout.BeginArea(rect);
            EditorGUILayout.BeginVertical("Box");
            scroll = GUILayout.BeginScrollView(scroll, true, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            Rect buttonrect = new Rect(10, 10, 384, 384);
            GUILayout.BeginArea(rect);
            GUILayout.Label("Click to auto create \nAttackable,\nInteractable,\nGround Layers");
            if (GUILayout.Button("Auto Create Layers for Demo"))
            {
                bool popup = EditorUtility.DisplayDialog("Are you sure?", "This will auto create layers Ground, Interactable, and Attackable starting at layer 23.", "Go ahead.", "No thanks.");
                if (popup)
                {
                    EditorMethods.CreateLayer("Ground");//23
                    EditorMethods.CreateLayer("Interactable");//24
                    EditorMethods.CreateLayer("Attackable");//25
                }
      
            }
            GUILayout.Space(16);
            GUILayout.Label("Click to open a game database");
            for (int i = 0; i < objects.Count; i++)
            {
                bool button = GUILayout.Button(objects[i].name);
                if (button)
                {
                    EditorGUIUtility.PingObject(objects[i]);
                    GameDatabase databse = objects[i] as GameDatabase;
                    if (databse.Settings == null)
                    {
                        Debug.LogWarning("Cant open window, there's no settings in the game database");
                        return;
                    }
                    EditorMethods.OpenDatabaseWindow(objects[i]);
                    

                }
            }

            GUILayout.Space(25);
            if (withsubdatabases)
            {
                suffx = "and CREATE new sub databases?";
            }
            else
            {
                suffx = "and DO NOT create new sub databases.";
            }

            copyOtherDatabaseEntries = GUILayout.Toggle(copyOtherDatabaseEntries, "Copy entries from other database?");
            if (copyOtherDatabaseEntries)
            {
                toCopy = EditorGUILayout.Popup(toCopy, names.ToArray());

            }

            copySettings = GUILayout.Toggle(copySettings, "Copy Settings from other database?");
           if (copySettings)
            {
                settingsCopy = EditorGUILayout.Popup(settingsCopy, names.ToArray());
            }
            withsubdatabases = GUILayout.Toggle(withsubdatabases, suffx);
            GUILayout.Label("New Database Name:");
            defaultname = EditorGUILayout.TextField(defaultname);
            bool createnew = GUILayout.Button("Create New Database " + "\n" + suffx);


            if (createnew)
            {
                if (string.IsNullOrEmpty(defaultname))
                {
                    Debug.LogWarning("Need to name the new database");
                    return;
                }
  
                string path = EditorUtility.SaveFilePanelInProject("Create New Game Database ", defaultname, "asset", "This will create a new ARPG Game Database.");
                if (path.Length > 0)
                {
                    //do the thing
                    GameDatabase newDatabase = DatabaseHandler.CreateNewGameDatabse(path, withsubdatabases, defaultname);


                    if (copyOtherDatabaseEntries)
                    {
                        GameDatabase other = objects[toCopy];
                        DatabaseHandler.ReloadDatabase(other);
                        subfoldersdic = CreateSubFolders(newDatabase, other);
                        CreateCopies(newDatabase, other);
                        ////how to cycle through them all and copy?

                    }

                    if (copySettings)
                    {
                        ProjectSettings copiedSettings = objects[toCopy].Settings;
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

            GUILayout.EndVertical();
            GUILayout.EndArea();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private void CreateCopies(GameDatabase destination, GameDatabase sourceToCopy)
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

     

        Dictionary<DatabaseID, string> CreateSubFolders(GameDatabase newDB, GameDatabase copiedDatabtase)
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
#endif