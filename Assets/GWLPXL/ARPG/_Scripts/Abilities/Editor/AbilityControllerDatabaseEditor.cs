#if UNITY_EDITOR

using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Statics.com;

using UnityEditor;
using UnityEngine;
namespace GWLPXL.ARPGCore.Abilities.com
{

    [CustomEditor(typeof(AbilityControllerDatabase))]
    public class AbilityControllerDatabaseEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            AbilityControllerDatabase abilityDatabase = (AbilityControllerDatabase)target;
           

            if (GUILayout.Button("Reload Database"))
            {
                DatabaseHandler.ReloadDatabase(abilityDatabase);
            }
            if (GUILayout.Button("Clear Database IDs"))
            {
                abilityDatabase.SetSlots(new AbilityControllerDatabaseSlot[0]);
            }
            if (GUILayout.Button("Open Ability Editor"))
            {
                EditorMethods.OpenDatabaseWindow(abilityDatabase);

            }

        }

       
       

       

       
    }
}

#endif