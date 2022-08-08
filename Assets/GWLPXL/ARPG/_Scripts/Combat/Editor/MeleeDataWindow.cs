using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Combat.com
{


    public class MeleeDataWindow : ARPGDatabaseWindow
    {
        MeleeDataDatabase db;

        public override void SetDatabase(Object database)
        {
            db = database as MeleeDataDatabase;
            source = GetDatabase();
        }

        public override void ShowWindow()
        {
           
        }

        protected override IDatabase GetDatabase()
        {
            return db as IDatabase;
        }

        protected override void MakeBlankCopy()
        {
            temp = ScriptableObject.CreateInstance<MeleeData>() as MeleeData;
        }

        protected override void NewLayout()
        {
            if (GUILayout.Button("Create as New"))
            {
                MeleeData newInstance = Instantiate(temp) as MeleeData;
                string name = newInstance.MeleeVars.Name;
                Object asset = TryCreateNew(name, newInstance);

                ReloadDatabase();
                MakeBlankCopy();
            }


            if (GUILayout.Button("Close"))
            {
                //close.
                Close();
            }
        }

        protected override void ReloadDatabase()
        {
            DatabaseHandler.ReloadDatabase(db);
            ReloadMessage();
        }

        
    }
}