
using UnityEngine;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEditor;

namespace GWLPXL.ARPGCore.Looting.com
{


    public class LootWindow : ARPGDatabaseWindow
    {
        LootDropsDatabase database;
        public override void SetDatabase(Object database)
        {
            this.database = database as LootDropsDatabase;
            source = GetDatabase();
        }

        public override void ShowWindow()
        {
           // EditorWindow.GetWindow(typeof(LootWindow));
        }

        protected override IDatabase GetDatabase()
        {
            return database as IDatabase;
        }

        protected override void MakeBlankCopy()
        {
            temp = ScriptableObject.CreateInstance<LootDrops>();
        }

        protected override void NewLayout()
        {
            GUILayout.Space(25);
            if (GUILayout.Button("Create as New"))
            {
                LootDrops newAbility = Instantiate(temp) as LootDrops;
                string name = newAbility.ID.Name;
                TryCreateNew(name, newAbility);
                ReloadDatabase();
            }


            if (GUILayout.Button("Close"))
            {
                //close.
                CloseWindow();
            }
        }

      

        protected override void ReloadDatabase()
        {
            DatabaseHandler.ReloadDatabase(database);
            ReloadMessage();
        }
    }
}