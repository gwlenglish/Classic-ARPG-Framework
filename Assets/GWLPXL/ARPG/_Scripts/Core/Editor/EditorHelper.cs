using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace GWLPXL.ARPGCore.com
{


    public class EditorInspectorDraw
    {
        public UnityEditor.Editor Editor = null;
        public bool Redraw = true;
        public Object cachemap = null;
        Vector2 sv;
        public EditorInspectorDraw()
        {

        }
        public void Draw(Object target, bool disabled = false)
        {
            if (target == null)
            {
                cachemap = null;
                return;
            }
            if (cachemap != target)
            {
                Redraw = true;
                cachemap = target;
            }

            if (Redraw)
            {
                Editor = UnityEditor.Editor.CreateEditor(cachemap);
                Editor.CreateInspectorGUI();
            }

            EditorGUI.BeginDisabledGroup(disabled);
            sv = EditorGUILayout.BeginScrollView(sv);
            Editor.OnInspectorGUI();
            EditorGUILayout.EndScrollView();
            EditorGUI.EndDisabledGroup();
        }

    }

    //neutral editor helper
    public static class EditorHelper
    {
        public static int LargeFont = 22;
        public static int MediumFont = 16;
        public static int SmallFont = 12;
        public static int MediumSpace = 25;
        public static int SmallSpace = 10;
        public static int LargeSpace = 50;
        public static int tinySpace = 5;

        static Color remove = Color.red;
        static Color confirm = Color.green;
        static Color select = Color.blue;
        static Color header = Color.blue;
        const string selecthtml = "#FFB900";
        const string removehtml = "#B900FF";
        const string confirmhtml = "#00FFB9";
        const string headerhtml = "#77caff";
        public static void DrawProperty(SerializedProperty prop)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(prop);
            GUILayout.Space(10);
            GUILayout.EndHorizontal();
        }

        public static void DrawLabel(string lable)
        {
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField(lable, EditorStyles.boldLabel);
        }
        public static void DrawLabel(string lable, GUIStyle style)
        {
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField(lable, style);
        }


        public static void DrawHelpBox(string message, int space = 5)
        {
            EditorGUILayout.Space(space);
            EditorGUILayout.HelpBox(message, MessageType.Info);
        }

        public static EditorInspectorDraw CreateEditor()
        {
            return new EditorInspectorDraw();
        }
        public static int PopupSelection(int selection, string[] options)
        {
            selection = EditorGUILayout.Popup(selection, options);
            return selection;
        }
        public static int TabSelection(int selection, string[] options, int perrow)
        {
            selection = GUILayout.SelectionGrid(selection, options, perrow);
            return selection;
        }
        public static void SmallGUISpace()
        {
            GUILayout.Space(SmallSpace);
        }
        public static void MediumGUISpace()
        {
            GUILayout.Space(MediumSpace);
        }

        public static void LargeGUISpace()
        {
            GUILayout.Space(LargeSpace);
        }

        public static void TinyGUISpace()
        {
            GUILayout.Space(tinySpace);
        }
        public static bool RemoveButton(string thingtoremove)
        {
            return GUILayout.Button("Remove " + thingtoremove);
        }
        public static bool AddButton(string thingtoadd)
        {
            return GUILayout.Button("Add " + thingtoadd);


        }

        public static Color GetSelectColor(string htmloverride = "")
        {
            if (string.IsNullOrWhiteSpace(htmloverride))
            {
                htmloverride = selecthtml;
            }
            ColorUtility.TryParseHtmlString(htmloverride, out select);
            GUI.color = select;
            return select;
        }
        public static Color GetRemoveColor(string htmloverride = "")
        {
            if (string.IsNullOrWhiteSpace(htmloverride))
            {
                htmloverride = removehtml;
            }
            ColorUtility.TryParseHtmlString(htmloverride, out remove);
            GUI.color = remove;
            return remove;
        }
        public static Color GetHeaderColor(string htmloverride = "")
        {
            if (string.IsNullOrWhiteSpace(htmloverride))
            {
                htmloverride = headerhtml;
            }
            ColorUtility.TryParseHtmlString(htmloverride, out header);
            GUI.color = header;
            return remove;
        }
        public static Color GetConfirmColor(string htmloverride = "")
        {
            if (string.IsNullOrWhiteSpace(htmloverride))
            {
                htmloverride = confirmhtml;
            }
            ColorUtility.TryParseHtmlString(htmloverride, out confirm);
            GUI.color = confirm;
            return confirm;
        }

        public static void ResetGUIColor()
        {
            GUI.color = Color.white;
        }
        static bool button = false;
        public static bool Button(string thingtoadd, GUILayoutOption[] options = null, bool removecolor = true)
        {
            button = GUILayout.Button(thingtoadd, options);
            if (removecolor)
            {
                ResetGUIColor();
            }
            return button;


        }
    }
}