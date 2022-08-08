using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEditor;

namespace GWLPXL.ARPGCore.Quests.com
{


    public class QuestWindow : ARPGDatabaseWindow
    {
        QuestDatabase database;
        public override void SetDatabase(Object database)
        {
            this.database = database as QuestDatabase;
            source = GetDatabase();
        }

        public override void ShowWindow()
        {
           // EditorWindow.GetWindow(typeof(QuestWindow));
   
        }

        protected override IDatabase GetDatabase()
        {
            return database as IDatabase;
        }

        protected override void MakeBlankCopy()
        {
            temp = ScriptableObject.CreateInstance<Quest>();
        }

        protected override void NewLayout()
        {
            GUILayout.Space(25);
            if (GUILayout.Button("Create as New"))
            {
                Quest newAsset = Instantiate(temp) as Quest;
                string name = newAsset.GetQuestName();
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
