#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;

[CustomEditor(typeof(GameDatabase))]
public class GameDatabaseEditor : UnityEditor.Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameDatabase gamedatabase = (GameDatabase)target;
        if (GUILayout.Button("Reload Game Database"))
        {
            //ProjectSettings settings = ScriptableObject.CreateInstance<ProjectSettings>();
            //string[] guids = AssetDatabase.FindAssets("t:" + settings.GetType().Name, gamedatabase.GetSearchFolders());//doesnt work at the moment
            //if (guids.Length > 0)
            //{
            //    gamedatabase.Settings = AssetDatabase.LoadAssetAtPath(guids[0], typeof(ProjectSettings)) as ProjectSettings;
            //}
            DatabaseHandler.ReloadDatabase(gamedatabase);

        }
        if (GUILayout.Button("Reload all databases"))
        {
            DatabaseHandler.ReloadAll(gamedatabase);
        }
        GUILayout.Space(25);
        if (GUILayout.Button("Open Editor Window"))
        {
            DatabaseHandler.ReloadDatabase(gamedatabase);
           
            EditorMethods.OpenDatabaseWindow(gamedatabase);



        }
    }


  


}
#endif