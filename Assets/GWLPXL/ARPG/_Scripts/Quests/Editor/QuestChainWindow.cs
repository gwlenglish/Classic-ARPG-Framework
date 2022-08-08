using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Saving.com;
using UnityEditor;
using GWLPXL.ARPGCore.Statics.com;

namespace GWLPXL.ARPGCore.Quests.com
{


    public class QuestChainWindow : ARPGDatabaseWindow
    {
        QuestchainDatabase database;
        public override void SetDatabase(Object database)
        {
            this.database = database as QuestchainDatabase;
            source = GetDatabase();
        }

        public override void ShowWindow()
        {
         //   EditorWindow.GetWindow(typeof(QuestChainWindow));

        }

        protected override IDatabase GetDatabase()
        {
            return database as IDatabase;
        }

        protected override void MakeBlankCopy()
        {
            temp = ScriptableObject.CreateInstance<Questchain>();
        }

        protected override void NewLayout()
        {
            GUILayout.Space(25);
            if (GUILayout.Button("Create as New"))
            {
                Questchain newAsset = Instantiate(temp) as Questchain;
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