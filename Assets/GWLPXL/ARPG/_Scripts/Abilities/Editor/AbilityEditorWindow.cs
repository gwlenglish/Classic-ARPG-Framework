#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Abilities.com
{

    /// <summary>
    /// nah, a bit much. Not working for now, the editor is sufficient
    /// </summary>
    public class AbilityEditorWindow : EditorWindow, ICreatorWIndow
    {
        Vector2 scrollPos;
        GameDatabase gamedatabase;

        string[] abilities;
        int abilityypeSelect;

        public void SetGameDatabase(GameDatabase database)
        {
            gamedatabase = database;

        

        }


        private void OnGUI()
        {
            EditorGUILayout.BeginVertical("Box");
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);
            EditorGUILayout.LabelField("Ability Creator", EditorStyles.boldLabel);
            EditorGUILayout.Space(25);

           // abilityypeSelect = GUILayout.SelectionGrid(abilityypeSelect, abilities, 2);


            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
    }
}
#endif