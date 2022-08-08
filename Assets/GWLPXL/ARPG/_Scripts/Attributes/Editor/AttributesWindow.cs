using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GWLPXL.ARPGCore.Attributes.com
{


    public class AttributesWindow : ARPGDatabaseWindow
    {
        ActorAttributesDatabase database;
        public override void SetDatabase(Object database)
        {
            this.database = database as ActorAttributesDatabase;
            source = GetDatabase();
        }

        public override void ShowWindow()
        {

        }
       

        protected override void NewLayout()
        {
            if (GUILayout.Button("Create as New"))
            {
                ActorAttributes newAbility = Instantiate(temp) as ActorAttributes;
                string name = newAbility.ActorName;
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
        protected override IDatabase GetDatabase()
        {
            return database as IDatabase;
        }

       

        protected override void ReloadDatabase()
        {
            DatabaseHandler.ReloadDatabase(database);
            EditorUtility.DisplayDialog("Reloaded", "Database reloaded", "Okay");
        }

        protected override void MakeBlankCopy()
        {
            temp = ScriptableObject.CreateInstance<ActorAttributes>();

        }
    }
}