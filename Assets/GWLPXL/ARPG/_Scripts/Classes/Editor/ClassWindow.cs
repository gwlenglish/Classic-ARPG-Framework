using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Saving.com;
using UnityEditor;
using GWLPXL.ARPGCore.Statics.com;

namespace GWLPXL.ARPGCore.Classes.com
{


    public class ClassWindow : ARPGDatabaseWindow
    {
        ActorClassDatabase database;
        public override void SetDatabase(Object database)
        {
            this.database = database as ActorClassDatabase;
            source = GetDatabase();
        }

        public override void ShowWindow()
        {
           // EditorWindow.GetWindow(typeof(ClassWindow));
     

        }

        protected override IDatabase GetDatabase()
        {
            return database as IDatabase;
        }

        protected override void MakeBlankCopy()
        {
            temp = ScriptableObject.CreateInstance<ActorClass>();
        }

        protected override void NewLayout()
        {
            GUILayout.Space(25);
            if (GUILayout.Button("Create as New"))
            {
                ActorClass newAsset = Instantiate(temp) as ActorClass;
                string name = newAsset.GetClassName();
                TryCreateNew(name, newAsset);
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