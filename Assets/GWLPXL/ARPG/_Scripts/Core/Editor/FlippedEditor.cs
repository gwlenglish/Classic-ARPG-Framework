using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace GWLPXL.ARPGCore.com
{

    /// <summary>
    /// used to display runtime scriptable object data while in playmode
    /// </summary>
    public abstract class FlippedEditor : UnityEditor.Editor
    {
        protected EditorInspectorDraw runtimeed;

        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                ShowRuntimeVersion();
            }
            else
            {
                ShowEditorVersion();
            }
        }
        protected virtual void ShowEditorVersion()
        {
            base.DrawDefaultInspector();
        }

        protected abstract UnityEngine.Object GetRuntimeObject();

        protected virtual void ShowRuntimeVersion()
        {
            if (runtimeed == null)
            {
                runtimeed = EditorHelper.CreateEditor();
            }

            runtimeed.Draw(GetRuntimeObject());
        }
    }
}