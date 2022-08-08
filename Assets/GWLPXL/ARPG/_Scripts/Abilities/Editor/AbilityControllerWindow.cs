
using UnityEngine;
using UnityEditor;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Abilities.com
{

    public class AbilityControllerWindow : ARPGDatabaseWindow
    {
        AbilityControllerDatabase abilitiesdb;

        protected override IDatabase GetDatabase()
        {
            return abilitiesdb as IDatabase;
        }
        public override void SetDatabase(Object database)
        {
            abilitiesdb = database as AbilityControllerDatabase;
            source = GetDatabase();
        }
        public override void ShowWindow()
        {
           

            
        }

     

       

        protected override void NewLayout()
        {
            if (GUILayout.Button("Create as New"))
            {
                AbilityController newAbility = Instantiate(temp) as AbilityController;
                string name = newAbility.Data.Name;
                TryCreateNew(name, newAbility);
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
            DatabaseHandler.ReloadDatabase(abilitiesdb);
            ReloadMessage();
        }

        protected override void MakeBlankCopy()
        {
            temp = ScriptableObject.CreateInstance<AbilityController>() as AbilityController;
        }
    }
}