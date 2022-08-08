using System.Collections.Generic;
using System.Linq;
using GWLPXL.ARPG._Scripts.Editor.ReloadProcessors;
using GWLPXL.ARPG._Scripts.Editor.ReloadProcessors.com;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.EditorModels.GameDatabase.com
{
    public class CreateGameDatabase: ScriptableObject, IReloadGameDatabaseList
    {
        public bool withsubdatabases = true;
        public string suffx;
        public bool copyOtherDatabaseEntries;
        public int toCopy = -1;
        public bool copySettings;
        public int settingsCopy = -1;
        public string defaultname;
        
        
        public List<string> names = new List<string>();
        public List<GWLPXL.ARPGCore.com.GameDatabase> objects = new List<GWLPXL.ARPGCore.com.GameDatabase>();
        

        public List<GWLPXL.ARPGCore.com.GameDatabase> GameDatabases
        {
            get => objects;
            set
            {
                objects = value;
                names = objects.Select(s => s.name).ToList();
            }
        }

        private void OnEnable()
        {
            var key = nameof(GWLPXL.ARPGCore.com.GameDatabase);
            string[] percents = UnityEditor.AssetDatabase.FindAssets("t:" + key);//specific if you want by putting t:armor or t:equipment, etc.
            List<GWLPXL.ARPGCore.com.GameDatabase> temp = new List<GWLPXL.ARPGCore.com.GameDatabase>();
            foreach (var guid in percents)
            {
                string obj = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                GWLPXL.ARPGCore.com.GameDatabase newItem = UnityEditor.AssetDatabase.LoadAssetAtPath(obj, typeof(GWLPXL.ARPGCore.com.GameDatabase)) as GWLPXL.ARPGCore.com.GameDatabase;
                if (newItem != null && objects.Contains(newItem) == false)
                {
                    objects.Add(newItem);
                    names.Add(newItem.name);
                }
            }

            // if we have any database - set indexes to copy it
            if (objects.Any())
            {
                toCopy = 0;
                settingsCopy = 0;
                copySettings = true;
                copyOtherDatabaseEntries = true;
            }
        }
    }
}