#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GWLPXL.ARPGCore.Traits.com
{

    [CustomEditor(typeof(TraitDrops))]
    public class TraitDropsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            TraitDrops drops = (TraitDrops)target;
            if (GUILayout.Button("Validate Trait Table"))
            {
                CreateLootTable(drops);
            }
            if (GUILayout.Button("Validate ALL Trait Tables"))
            {
                ValidAllLootTables(drops);
            }
        }


        void CreateLootTable(TraitDrops drops)
        {
            drops.CreateLootTable();

        }

        void ValidAllLootTables(TraitDrops drops)
        {
            List<TraitDrops> all = new List<TraitDrops>();
            string key = drops.GetType().Name;
            string[] percents = UnityEditor.AssetDatabase.FindAssets("t:" + key);//specific if you want by putting t:armor or t:equipment, etc.
            foreach (var guid in percents)
            {
                string obj = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                TraitDrops newItem = UnityEditor.AssetDatabase.LoadAssetAtPath(obj, typeof(TraitDrops)) as TraitDrops;
                if (newItem != null)
                {
                    newItem.CreateLootTable();
                }
            }
        }
    }
}

#endif