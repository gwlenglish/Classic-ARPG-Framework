#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GWLPXL.ARPGCore.ProgressTree.com
{

    /// <summary>
    /// delete this, deprecated, old
    /// </summary>
    public static class JSonAttributes
    {
        static string extension = ".json";

        public static void OverwriteJson(ProgressTreeHolder holder)
        {
            if (File.Exists(AssetDatabase.GetAssetPath(holder.JsonConfig)))
            {
                TextAsset textasset = holder.JsonConfig;

                string path = AssetDatabase.GetAssetPath(textasset);
                string savedJson = JsonUtility.ToJson(holder);
                File.WriteAllText(path, savedJson);

                holder.JsonConfig = textasset;
                AssetDatabase.Refresh();
            }
        }
        public static void OverwriteJson(TreeNodeHolder holder)
        {
            if (File.Exists(AssetDatabase.GetAssetPath(holder.JsonConfig)))
            {
                TextAsset textasset = holder.JsonConfig;

                string path = AssetDatabase.GetAssetPath(textasset);
                string savedJson = JsonUtility.ToJson(holder);
                File.WriteAllText(path, savedJson);

                holder.JsonConfig = textasset;
                AssetDatabase.Refresh();
            }
        }

        public static void SaveJson(TreeNodeHolder holder)
        {
            string dpath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(holder));
            string exportName = "\\" + holder.name + extension;

            //try to move the old file. 
            if (File.Exists(dpath + exportName))
            {
                if (File.Exists(dpath + "\\" + "_old" + extension))
                {
                    File.Delete(dpath + "\\" + "_old" + extension);
                }
                File.Move(dpath + exportName, dpath + "\\" + "_old" + extension);
            }

            string savedJson = JsonUtility.ToJson(holder);
            File.WriteAllText(dpath + exportName, savedJson);


            AssetDatabase.Refresh();
        }

        public static void SaveJson(ProgressTreeHolder holder)
        {
            string dpath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(holder));
            string exportName = "\\" + holder.name + extension;

            if (File.Exists(dpath + exportName))
            {
                if (File.Exists(dpath + "\\" + "_old" + extension))
                {
                    File.Delete(dpath + "\\" + "_old" + extension);
                }
                File.Move(dpath + exportName, dpath + "\\" + "_old" + extension);
            }

            var savedJson = JsonUtility.ToJson(holder);
            File.WriteAllText(dpath + exportName, savedJson);


            AssetDatabase.Refresh();
        }

        public static void ReloadAallTreeHolders()
        {
            string[] guids = FindAttributeHolders();

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                ProgressTreeHolder asset = AssetDatabase.LoadAssetAtPath<ProgressTreeHolder>(assetPath);
                LoadJson(asset);

            }
        }

        public static void ReloadAallNodeHolders()
        {
            string[] guids = FindAttributeHolders();

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                TreeNodeHolder asset = AssetDatabase.LoadAssetAtPath<TreeNodeHolder>(assetPath);
                LoadJson(asset);

            }
        }
        public static string[] FindAttributeHolders()
        {
            ProgressTreeHolder keyholder = ScriptableObject.CreateInstance<ProgressTreeHolder>();
            string typeKey = keyholder.GetType().Name;
            string[] guids = AssetDatabase.FindAssets("t:" + typeKey, null);
            return guids;
        }

        public static void LoadJson(ProgressTreeHolder holder)
        {
            if (File.Exists(AssetDatabase.GetAssetPath(holder.JsonConfig)))
            {
                TextAsset textasset = holder.JsonConfig;
                string textFile = File.ReadAllText(AssetDatabase.GetAssetPath(holder.JsonConfig));
                JsonUtility.FromJsonOverwrite(textFile, holder);
                holder.JsonConfig = textasset;
                AssetDatabase.Refresh();
            }
        }

        public static void LoadJson(TreeNodeHolder holder)
        {
            if (File.Exists(AssetDatabase.GetAssetPath(holder.JsonConfig)))
            {
                TextAsset textasset = holder.JsonConfig;
                string textFile = File.ReadAllText(AssetDatabase.GetAssetPath(holder.JsonConfig));
                JsonUtility.FromJsonOverwrite(textFile, holder);
                holder.JsonConfig = textasset;
                AssetDatabase.Refresh();
            }
        }

    }
}

#endif