

#if UNITY_EDITOR


using UnityEditor;

namespace GWLPXL.ARPGCore.Statics.com
{
    /// <summary>
    /// test to see if works
    /// </summary>
    public static class JsconConfig 
    {
        static string extension = ".json";
        static bool prettyprint = true;
        public static string[] FindAllHoldersOfType(ISaveJsonConfig holder)
        {
            string typeKey = holder.GetObject().GetType().Name;
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:" + typeKey, null);
            return guids;
        }

        public static void OverwriteJson(ISaveJsonConfig holder)
        {
            if (holder == null) return;
            if (holder.GetTextAsset() == null)
            {
                SaveJson(holder);
            }
            if (System.IO.File.Exists(UnityEditor.AssetDatabase.GetAssetPath(holder.GetTextAsset())))
            {
                UnityEngine.TextAsset textasset = holder.GetTextAsset();

                string path = UnityEditor.AssetDatabase.GetAssetPath(textasset);
                string savedJson = UnityEngine.JsonUtility.ToJson(holder.GetObject(), prettyprint);
                System.IO.File.WriteAllText(path, savedJson);
               
                holder.SetTextAsset(textasset);
                EditorUtility.SetDirty(textasset);
                EditorUtility.SetDirty(holder.GetObject());
                AssetDatabase.RenameAsset(path, holder.GetObject().name + extension);
                UnityEditor.AssetDatabase.Refresh();
            }
        }

        
        public static void SaveJson(ISaveJsonConfig holder)
        {
            if (holder == null) return;
            string dpath = System.IO.Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(holder.GetObject()));
            string exportName = "\\" + holder.GetObject().name + extension;

            RenameOrDeleteOld(dpath, exportName);

            string savedJson = UnityEngine.JsonUtility.ToJson(holder, prettyprint);
            System.IO.File.WriteAllText(dpath + exportName, savedJson);
            
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

            UnityEngine.TextAsset created = UnityEditor.AssetDatabase.LoadAssetAtPath(dpath + exportName, typeof(UnityEngine.TextAsset)) as UnityEngine.TextAsset;
            holder.SetTextAsset(created);
            EditorUtility.SetDirty(holder.GetObject());
        }

        public static void LoadJson(ISaveJsonConfig holder)
        {
            if (holder == null) return;
            if (System.IO.File.Exists(UnityEditor.AssetDatabase.GetAssetPath(holder.GetTextAsset())))
            {
                UnityEngine.TextAsset textasset = holder.GetTextAsset();
                string textFile = System.IO.File.ReadAllText(UnityEditor.AssetDatabase.GetAssetPath(holder.GetTextAsset()));
                UnityEngine.JsonUtility.FromJsonOverwrite(textFile, holder.GetObject());
                holder.SetTextAsset(textasset);
                UnityEditor.AssetDatabase.Refresh();
            }
        }
        static void RenameOrDeleteOld(string dpath, string exportName)
        {
            //try to move the old file. 
            if (System.IO.File.Exists(dpath + exportName))
            {
                //only allows up to save the two versions, the old and then the new
                if (System.IO.File.Exists(dpath + "\\" + "_old" + extension))
                {
                    System.IO.File.Delete(dpath + "\\" + "_old" + extension);
                }
                System.IO.File.Move(dpath + exportName, dpath + "\\" + "_old" + extension);
            }
        }
    }
}
#endif