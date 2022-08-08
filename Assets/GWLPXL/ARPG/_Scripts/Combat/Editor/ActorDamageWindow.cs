
using UnityEngine;
using UnityEditor;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Combat.com
{

    public class ActorDamageWindow : ARPGDatabaseWindow
    {
        ActorDamageDatabase db;

        protected override IDatabase GetDatabase()
        {
            return db as IDatabase;
        }
        public override void SetDatabase(Object database)
        {
            db = database as ActorDamageDatabase;
            source = GetDatabase();

         
            
        }
        public override void ShowWindow()
        {
 
        }



        protected override void NewLayout()
        {

            if (GUILayout.Button("Create as New"))
            {
                ActorDamageData newInstance = Instantiate(temp) as ActorDamageData;
                string name = newInstance.DamageVar.Name;
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

        protected override void MakeBlankCopy()
        {
            temp = ScriptableObject.CreateInstance<ActorDamageData>() as ActorDamageData;
        }
    }
}