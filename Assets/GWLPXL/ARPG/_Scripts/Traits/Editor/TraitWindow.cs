
using UnityEngine;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Saving.com;
using UnityEditor;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Types.com;


namespace GWLPXL.ARPGCore.Traits.com
{


    public class TraitWindow : ARPGDatabaseWindow
    {
        EquipmentTraitDatabase database;
        TraitType type = TraitType.Stat;
        string traitName = string.Empty;
        public override void SetDatabase(Object database)
        {
            this.database = database as EquipmentTraitDatabase;
            source = GetDatabase();
        }

        public override void ShowWindow()
        {
           // EditorWindow.GetWindow(typeof(TraitWindow));

        }

        protected override IDatabase GetDatabase()
        {
            return database as IDatabase;
        }

        protected override void DrawCustomInspector()
        {
            focused = GetObject();
            if (focused != null)
            {
                UnityEditor.Editor editor = UnityEditor.Editor.CreateEditor(focused);
                editor.DrawDefaultInspector();
            }
            else
            {
                EditorGUILayout.BeginVertical();
                //draw a new one. 
                type = (TraitType)EditorGUILayout.EnumPopup("Trait type to create: ", type);
                traitName = EditorGUILayout.TextField("Trait name: :", traitName);

                EditorGUILayout.EndVertical();
            }

        }
        protected override void NewLayout()
        {
            GUILayout.Space(25);

            if (GUILayout.Button("Create as New"))
            {
                string name = traitName;
                if (string.IsNullOrEmpty(name))
                {
                    EditorUtility.DisplayDialog("Name Required", "A name is required in order to create new", "Okay");
                    temp = null;
                    return;
                }
                switch (type)
                {
                    case TraitType.Stat:
                        temp = ScriptableObject.CreateInstance<StatTraitModifier>();
                        break;
                    case TraitType.Resource:
                        temp = ScriptableObject.CreateInstance<MaxResourceTraitModifier>();
                        break;
                    case TraitType.ElementAttack:
                        temp = ScriptableObject.CreateInstance<ElementAttackTraitModifier>();
                        break;
                    case TraitType.ElementResist:
                        temp = ScriptableObject.CreateInstance<ElementResistTraitModifier>();
                        break;

                    //case TraitType.AbilityMod:
                    //    temp = ScriptableObject.CreateInstance<StatTrait>();//not yet implemented
                    //    break;

                }
                if (temp == null)
                {
                    EditorUtility.DisplayDialog("Type Required", "A name and type is both required.", "Okay");
                    temp = null;
                    return;
                }
                EquipmentTrait trait = (EquipmentTrait)temp;
                trait.SetTraitName(traitName);
                TryCreateNew(name, temp);

                ReloadDatabase();
                temp = null;
                traitName = string.Empty;
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

        protected override void MakeBlankCopy()
        {
            if (temp != null)
            {
                DestroyImmediate(temp);
            }
            temp = null;
        }
    }
}