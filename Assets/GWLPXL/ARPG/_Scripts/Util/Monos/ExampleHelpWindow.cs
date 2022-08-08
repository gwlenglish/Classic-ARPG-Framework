using GWLPXL.ARPGCore.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Util.com
{


    [AddComponentMenu("")] // Don't display in add component menu
    public class ExampleHelpWindow : MonoBehaviour
    {
        public bool Show = true;
        public string m_Title;
        [TextArea(minLines: 10, maxLines: 50)]
        public string m_Description;

    
        private const float kPadding = 40f;

        private void OnGUI()
        {
            if (Show)
            {
                Vector2 size = GUI.skin.label.CalcSize(new GUIContent(m_Description));
                Vector2 halfSize = size * 0.5f;

                float maxWidth = Mathf.Min(Screen.width - kPadding, size.x);
                float left = Screen.width * 0.5f - maxWidth * 0.5f;
                float top = Screen.height * 0.4f - halfSize.y;

                Rect windowRect = new Rect(left, top, maxWidth, size.y);
                GUILayout.Window(400, windowRect, (id) => DrawWindow(id, maxWidth), m_Title);

                TickManager.Instance.Paused = true;
            }
        }

        private void DrawWindow(int id, float maxWidth)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label(m_Description);
            GUILayout.EndVertical();
            if (GUILayout.Button("Got it!"))
            {
                Show = false;
                TickManager.Instance.Paused = false;

            }
        }
    }
}
