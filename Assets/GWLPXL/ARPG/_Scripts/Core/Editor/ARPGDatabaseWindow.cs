
using UnityEngine;
using UnityEditor;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Saving.com;

namespace GWLPXL.ARPGCore.com
{
    public abstract class ARPGDatabaseWindow : EditorWindow
    {
        
        protected IDatabase source;
        protected UnityEngine.Object temp = null;
        protected UnityEngine.Object focused = null;
        protected int selected = 0;
        protected Vector2 scrollPosition;
        protected Rect rect;
        protected abstract void MakeBlankCopy();
        protected virtual void OnEnable()
        {

            MakeBlankCopy();
        }
        public abstract void SetDatabase(UnityEngine.Object database);
        public abstract void ShowWindow();
        protected abstract IDatabase GetDatabase();
        protected virtual void OnDisable()
        {
            if (temp != null)
            {
                DestroyImmediate(temp);
            }
            temp = null;
        }

        protected virtual string[] GetToolbarOptions(string[] forStrings)
        {
            string[] options = forStrings;
            string[] includeEmpty = new string[options.Length + 1];
            for (int i = 0; i < includeEmpty.Length; i++)
            {
                if (i == 0) continue;
                includeEmpty[i] = options[i - 1];

            }
            includeEmpty[0] = "Blank";
            return includeEmpty;
        }
        protected abstract void ReloadDatabase();
        protected abstract void NewLayout();
        protected virtual void ReloadMessage()
        {
            EditorUtility.DisplayDialog("Reloaded", "Database reloaded", "Okay");
        }
        protected virtual void SaveAllChanges(ISaveJsonConfig[] jsons)
        {
            for (int i = 0; i < jsons.Length; i++)//brute force saving, meh
            {
                JsconConfig.OverwriteJson(jsons[i]);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            for (int i = 0; i < jsons.Length; i++)
            {
                JsconConfig.LoadJson(jsons[i]);
            } 
            EditorUtility.DisplayDialog("Saved", "All Changes saved", "Okay");

        }
        protected virtual void ModifyExistingLayout()
        {
            GUILayout.Space(25);
            if (GUILayout.Button("Save Changes"))
            {
                SaveAllChanges(source.GetJsons());
                //ReloadDatabase();
            }

            if (GUILayout.Button("Close"))
            {
                CloseWindow();
            }

            if (GUILayout.Button("Reload Database"))
            {
                //close.
                ReloadDatabase();

            }

            
        }
        protected virtual string[] GetToolbarOptions()
        {
            string[] options = GetDatabase().GetAllNames();
            string[] includeEmpty = new string[options.Length + 1];
            for (int i = 0; i < includeEmpty.Length; i++)
            {
                if (i == 0) continue;
                includeEmpty[i] = options[i - 1];

            }
            includeEmpty[0] = "Blank";
            return includeEmpty;
        }
        protected virtual void CloseWindow()
        {
            //close.
            if (temp != null)
            {
                DestroyImmediate(temp);
                temp = null;
            }
      
            Close();
            
       
        }

        protected virtual void OnDestroy()
        {
            if (GetDatabase() != null && Application.isEditor)
            {
                bool confirmSave = EditorUtility.DisplayDialog("Save Changes?", "Do you want to save before closing?", "Yes, save.", "No.");
                if (confirmSave)
                {
                    SaveAllChanges(source.GetJsons());

                }
                else
                {
                    //nothing
                }
            }

            
        }
        protected Object TryCreateNew(string assetname, UnityEngine.Object asset)
        {
            if (string.IsNullOrEmpty(assetname))
            {
                Debug.LogError("Need a name to save");
                return null;

            }

            string folder = EditorUtility.SaveFilePanelInProject("Save Asset", assetname, "asset", "asset", GetDatabase().GetSearchFolders()[0]) ;
            if (folder.Length == 0)
            {
                Debug.Log("Did not find a folder for the assets");
                return null;
            }
            
            AssetDatabase.CreateAsset(asset, folder);
            Object newasset = AssetDatabase.LoadAssetAtPath(folder, typeof(Object));
            return newasset;

        }

        protected virtual void TryDelete(Object focused)
        {
            bool confirm = EditorUtility.DisplayDialog("Delete Entry", "This will delete the current entry from the project. Are you sure?", "Yes, Delete Asset", "No, cancel.");
            if (confirm && focused != null)
            {
                ISaveJsonConfig json = (ISaveJsonConfig)focused;
                if (json != null && json.GetTextAsset() != null)
                {
                    bool confirm2 = EditorUtility.DisplayDialog("Delete Config file as well?", "This will delete the current entry's config file from the project. Also delete the config file?", "Yes, delete config.", "No, keep config.");
                    if (confirm2)
                    {
                        string configfile = AssetDatabase.GetAssetPath(json.GetTextAsset());
                        AssetDatabase.DeleteAsset(configfile);
                    }
                }

                string path = AssetDatabase.GetAssetPath(focused);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
               
            }
        }
        protected virtual Object GetObject()
        {
            Object focused;
            if (selected == 0)
            {
                focused = temp;

            }
            else
            {
                focused = source.GetDatabaseObjectBySlotIndex(selected - 1);
            }

            return focused;
        }

        protected virtual void DefaultBehavior()
        {
            rect = EditorGUILayout.BeginVertical("Box");
            selected = GUILayout.SelectionGrid(selected, GetToolbarOptions(), source.GetWindowRowSize());
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            DrawCustomInspector();
            GUILayout.EndScrollView();
            EditorGUILayout.EndVertical();


            if (selected == 0)
            {
                NewLayout();
            }
            else
            {
                ModifyExistingLayout();
            }

            Postbehavior();
        }

        protected virtual void Postbehavior()
        {
            //
        }
        protected virtual void DrawCustomInspector()
        {
            focused = GetObject();
            if (focused != null)
            {
                UnityEditor.Editor editor = UnityEditor.Editor.CreateEditor(focused);
                editor.DrawDefaultInspector();
            }
  
        }
        protected virtual void OnGUI()
        {
            DefaultBehavior();
        }
      
    }
}