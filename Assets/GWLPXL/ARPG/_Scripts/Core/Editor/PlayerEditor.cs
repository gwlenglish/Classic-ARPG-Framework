using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GWLPXL.ARPGCore.com
{


    [CustomEditor(typeof(Player))]
    public class PlayerEditor : UnityEditor.Editor
    {
        
        public override void OnInspectorGUI()
        {
            Player player = (Player)target;
            if (Application.isPlaying)
            {
                //playmode
            }
            else
            {
                //editor
                ShowEditorVersion(player);
            }
            base.OnInspectorGUI();
        }


        protected virtual void ShowEditorVersion(Player player)
        {
            
        }



    }
}