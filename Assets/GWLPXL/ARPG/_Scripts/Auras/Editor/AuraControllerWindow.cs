
using UnityEngine;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEditor;

namespace GWLPXL.ARPGCore.Auras.com
{


    public class AuraControllerWindow : ARPGDatabaseWindow
    {
        AuraControllerDatabase database;
        public override void SetDatabase(Object database)
        {
            this.database = database as AuraControllerDatabase;
            source = GetDatabase();
        }

        public override void ShowWindow()
        {
           // EditorWindow.GetWindow(typeof(AuraControllerWindow));
        }

        protected override IDatabase GetDatabase()
        {
            return database as IDatabase;
        }

        protected override void MakeBlankCopy()
        {
            temp = ScriptableObject.CreateInstance<AuraController>();
        }

        protected override void NewLayout()
        {
            GUILayout.Space(25);
            if (GUILayout.Button("Create as New"))
            {
                AuraController newAbility = Instantiate(temp) as AuraController;
                string name = newAbility.GetID().Name;
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