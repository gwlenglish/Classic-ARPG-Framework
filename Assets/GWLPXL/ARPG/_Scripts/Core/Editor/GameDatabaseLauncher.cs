using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Wearables.com;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace GWLPXL.ARPGCore.com
{


    public static class ARPGLauncher
    {
        [MenuItem("GWLPXL/ARPG/Launch Databases")]
        static void OpenLauncherMenu()
        {
            //open the window that asks which database to launch or create new.
            EditorWindow window = ScriptableObject.CreateInstance<DatabaseLauncher>();
            window.Show();
        }
       

      
        [MenuItem("GWLPXL/ARPG/Insert DungeonMaster")]
        static void InsertSingleton()
        {
            bool confirm = EditorUtility.DisplayDialog("Insert Dungeon Master Singleton", "This will add a Dungeon Master Singleton to the scene.", "Proceed", "Cancel");
            if (confirm == false) return;

            DungeonMaster master = GameObject.FindObjectOfType<DungeonMaster>();
            if (master != null)
            {
                Debug.Log("Dungeon Master already added to scene");
                return;
            }
            else
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath("Assets/GWLPXL/ARPG/Prefabs/Singleton/ARPGDungeonMaster.prefab", typeof(GameObject)) as GameObject;
                PrefabUtility.InstantiatePrefab(prefab);
            }

        }
    }
}
