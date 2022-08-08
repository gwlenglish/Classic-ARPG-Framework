
using UnityEngine;
using UnityEditor;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Abilities.com
{

    public class AbilityWindow : ARPGDatabaseWindow
    {
        AbilitiesDatabase abilitiesdb;
        Vector2 scroll2;
        Vector2 scroll3;

        string[] logictypes = new string[0];
        protected override IDatabase GetDatabase()
        {
            return abilitiesdb as IDatabase;
        }
        public override void SetDatabase(Object database)
        {
            abilitiesdb = database as AbilitiesDatabase;
            source = GetDatabase();

         
            
        }
        public override void ShowWindow()
        {
 
        }

        //for future use
        //protected override void DefaultBehavior()
        //{
        //    rect = EditorGUILayout.BeginVertical("Box");
        //    selected = GUILayout.SelectionGrid(selected, GetToolbarOptions(), source.GetWindowRowSize());
        //    scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));


        //    Rect rect2 = new Rect(0, 10, 420, 720);
        //    GUILayout.BeginArea(rect2);
        //    scroll2 = GUILayout.BeginScrollView(scroll2, true, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
        //    DrawCustomInspector();
        //    EditorGUILayout.LabelField("Test");
        //    EditorGUILayout.EndScrollView();
        //    GUILayout.EndArea();


        //    Rect rect3 = new Rect(rect2.width + 10, 10, rect2.width, rect2.height / 2);
        //    GUILayout.BeginArea(rect3);
        //    scroll3 = GUILayout.BeginScrollView(scroll3, true, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
        //    DrawCustomInspector();
        //    EditorGUILayout.EndScrollView();
        //    GUILayout.EndArea();

        //    GUILayout.EndScrollView();
        //    EditorGUILayout.EndVertical();


        //    if (selected == 0)
        //    {
        //        NewLayout();
        //    }
        //    else
        //    {
        //        ModifyExistingLayout();
        //    }

        //    Postbehavior();
        //}


        protected override void NewLayout()
        {

            if (GUILayout.Button("Create as New"))
            {
                Ability newAbility = Instantiate(temp) as Ability;
                string name = newAbility.Data.Name;
                Object asset = TryCreateNew(name, newAbility);
               
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
            temp = ScriptableObject.CreateInstance<Ability>() as Ability;
        }
    }
}