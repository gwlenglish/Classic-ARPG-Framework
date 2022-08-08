using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.com;
using UnityEditor;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;

namespace GWLPXL.ARPGCore.Items.com
{


    public class InventoryWindow : ARPGDatabaseWindow
    {
        InventoryDatabase database;
        public override void SetDatabase(Object database)
        {
            this.database = database as InventoryDatabase;
            source = GetDatabase();
        }

        public override void ShowWindow()
        {
          //  EditorWindow.GetWindow(typeof(InventoryWindow));
    
        }

        protected override IDatabase GetDatabase()
        {
            return database as IDatabase;
        }

        protected override void MakeBlankCopy()
        {
            temp = ScriptableObject.CreateInstance<ActorInventory>();
        }

        protected override void NewLayout()
        {
            GUILayout.Space(25);
            if (GUILayout.Button("Create as New"))
            {
                ActorInventory newAbility = Instantiate(temp) as ActorInventory;
                string name = newAbility.GetName();
                TryCreateNew(name, newAbility);
                ReloadDatabase();
                MakeBlankCopy();
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