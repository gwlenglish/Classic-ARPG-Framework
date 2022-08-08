#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Attributes.com;
    
namespace GWLPXL.ARPGCore.com
{

    [CustomEditor(typeof(ActorAttributesDatabase))]
    public class ActorAttributesDatabaseEditor : UnityEditor.Editor
    {


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ActorAttributesDatabase stats = (ActorAttributesDatabase)target;
            GUILayout.Space(25);
            if (GUILayout.Button("Reload Database"))
            {
                DatabaseHandler.ReloadDatabase(stats);
            }
            if (GUILayout.Button("Clear Database IDs"))
            {
                stats.SetSlots(new AttributesDatabaseSlot[0]);
            }
            if (GUILayout.Button("Open Attributes Editor"))
            {
                EditorMethods.OpenDatabaseWindow(stats);

            }

            EditorGUILayout.Space(25);

            EditorGUILayout.LabelField("Export/Import");
            if (GUILayout.Button("Export Database to Text"))
            {
                string result = DatabaseHandler.GetDBCSV(stats);
                //Debug.Log(result);
            }

            if (GUILayout.Button("Import Database to Text"))
            {
                DatabaseHandler.ImportDBCSV(stats);
                //Debug.Log(result);
            }
        }
    }
}
#endif