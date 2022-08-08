using System.Collections.Generic;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Looting.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEditor;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.ObjectEditors.com
{
    public class LootModifyEditor: ArpgBaseModifyEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            LootDrops drops = (LootDrops)target;

            if (GUILayout.Button("Add All Drops to List"))
            {
                AddAllToList(drops);
            }

            if (GUILayout.Button("Sort List By Equipment Type"))
            {
                SortListByEquipmentType(drops, false);
            }
            if (GUILayout.Button("Sort List By Equipment SLOT"))
            {
                SortListByEquipmentType(drops, true);
            }

            if (GUILayout.Button("Refresh List"))
            {
                RefreshList(drops);
            }

            GUILayout.Space(25);
            if (GUILayout.Button("Validate Loot Table"))
            {
                CreateLootTable(drops);
            }
            if (GUILayout.Button("Validate ALL Loot Tables"))
            {
                ValidAllLootTables(drops);
            }

            GUILayout.Space(25);
            if (GUILayout.Button("Save Config"))
            {
               JsconConfig.SaveJson(drops);

            }
            if (GUILayout.Button("Load Config"))
            {
                JsconConfig.LoadJson(drops);

            }
            if (GUILayout.Button("Overwrite Config"))
            {
                JsconConfig.OverwriteJson(drops);

            }

        }

        void ValidAllLootTables(LootDrops drops)
        {
            List<LootDrops> all = new List<LootDrops>();
            string key = drops.GetType().Name;
            string[] percents = UnityEditor.AssetDatabase.FindAssets("t:" + TypeKeys.Items);//specific if you want by putting t:armor or t:equipment, etc.
            foreach (var guid in percents)
            {
                string obj = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                LootDrops newItem = UnityEditor.AssetDatabase.LoadAssetAtPath(obj, typeof(LootDrops)) as LootDrops;
                if (newItem != null)
                {
                    newItem.CreateLootTable();
                }
            }
        }
        public void CreateLootTable(LootDrops drops)
        {
            drops.CreateLootTable();

        }
        public void RefreshList(LootDrops dropList)
        {
            List<Item> _new = new List<Item>();
            for (int i = 0; i < dropList.AllPossibleItems.Count; i++)
            {
                if (dropList.AllPossibleItems[i] != null)
                {
                    _new.Add(dropList.AllPossibleItems[i]);
                }
            }
            dropList.AllPossibleItems = _new;
        }

        public void AddAllToList(LootDrops dropList)
        {
            List<Item> all = new List<Item>();
            string[] percents = UnityEditor.AssetDatabase.FindAssets("t:" + TypeKeys.Items);//specific if you want by putting t:armor or t:equipment, etc.
            foreach (var guid in percents)
            {
                string obj = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                Item newItem = UnityEditor.AssetDatabase.LoadAssetAtPath(obj, typeof(Item)) as Item;
                if (newItem != null)
                {
                    all.Add(newItem);
                    EditorUtility.SetDirty(newItem);

                }
            }
            dropList.AllPossibleItems = all;

            EditorUtility.SetDirty(dropList);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void SortListByEquipmentType(LootDrops dropList, bool bySlot)
        {
            List<Item> items = dropList.AllPossibleItems;
            List<Equipment> equipment = new List<Equipment>();
            List<Item> therest = new List<Item>();
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] is Equipment)
                {
                    equipment.Add(items[i] as Equipment);
                }
                else
                {
                    therest.Add(items[i]);
                }
            }

            if (bySlot == false)
            {
                equipment.Sort((p1, p2) => p1.GetEquipmentType().CompareTo(p2.GetEquipmentType()));
            }
            else
            {
                equipment.Sort((p1, p2) => p1.GetEquipmentSlot()[0].CompareTo(p2.GetEquipmentSlot()[0]));

            }
            List<Item> sorted = new List<Item>();
            for (int i = 0; i < equipment.Count; i++)
            {
                sorted.Add(equipment[i]);
            }
            for (int i = 0; i < therest.Count; i++)
            {
                sorted.Add(therest[i]);
            }
            dropList.AllPossibleItems = sorted;
        }
    }
}