using GWLPXL.ARPG._Scripts.Editor.com;
using UnityEditor;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.Data.com
{
    public static class ArpgStyle
    {
        public static float DefaultMenuWidth = 210f;
        
        public static float ItemOffset = 15f;
        public static float ItemIndentOffset = 15f;
        public static float ItemTriangleSize = 30f;
        //public Color ItemDefaultColor = Color.grey;
        
        public static Color ItemRootColor = new Color(0f, 0f, 0f, 0.2f);
        public static Color ItemSelectedColor = new Color(0.243f, 0.373f, 0.588f, 1f);
        public static Color ItemMouseOverColor = new Color(1f, 1f, 1f, 0.03f);

        public static float BorderPadding = 15f;
        public static Color BorderLineColorFirst = new Color(0.0f, 0.0f, 0.0f, 0.2f);
        public static Color BorderLineColorSecond = new Color(1f, 1f, 1f, 1f);

        public static Color DragAndDropZoneColor = new Color(0f, 0f, 0f, 0.1f);
        
        public static GUIStyle ToolbarButton => new GUIStyle(EditorStyles.toolbarButton)
        {
            fixedHeight = 0.0f,
            alignment = TextAnchor.MiddleCenter,
            stretchHeight = true,
            stretchWidth = false
        };

        public static GUIStyle ToolbarSearchTextField => GUI.skin.FindStyle("ToolbarSeachTextField");
        //public Color ItemSelectedInactiveColor = new Color(0.838f, 0.838f, 0.838f, 0.134f);
        
        // public static GUIStyle DefaultLabelStyle;
        // public static GUIStyle SelectedLabelStyle;

        private static Texture2D _itemTriangleDark;
        public static Texture2D ItemTriangleDark
        {
            get
            {
                if (_itemTriangleDark == null)
                {
                    _itemTriangleDark = ArpgEditorHelper.LoadTextureFromBase64("iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAQAAABKfvVzAAAAJklEQVQ4y2NgGAUjE9Qz/EeD9aRpqSfNlnrSHFZPml/qRyN0WAEAkTAY4yMq31kAAAAASUVORK5CYII=");
                }
                return _itemTriangleDark;
            }
        }
        
        private static Texture2D _itemTriangleExpandDark;
        public static Texture2D ItemTriangleExpandDark
        {
            get
            {
                if (_itemTriangleExpandDark == null)
                {
                    _itemTriangleExpandDark = ArpgEditorHelper.LoadTextureFromBase64("iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAQAAABKfvVzAAAAKElEQVQ4y2NgGAXDCtQDIUnK/wNhPWnKidaCUE6UFlTlJDlsFAwvAACz7Rjjjz/MZAAAAABJRU5ErkJggg==");
                }
                return _itemTriangleExpandDark;
            }
        }
        
        private static Texture2D _itemTriangleLight;
        public static Texture2D ItemTriangleLight
        {
            get
            {
                if (_itemTriangleLight == null)
                {
                    _itemTriangleLight = ArpgEditorHelper.LoadTextureFromBase64("iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAQAAABKfvVzAAAAJklEQVQ4y2NgGAUjEvyv/48O6knTUk+aLfWkOayeNL/Uj8bosAIAO142xaTZcBAAAAAASUVORK5CYII=");
                }
                return _itemTriangleLight;
            }
        }
        
        private static Texture2D _itemTriangleExpandLight;
        public static Texture2D ItemTriangleExpandLight
        {
            get
            {
                if (_itemTriangleExpandLight == null)
                {
                    _itemTriangleExpandLight = ArpgEditorHelper.LoadTextureFromBase64("iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAQAAABKfvVzAAAAKUlEQVQ4y2NgGAXDCfyv/19PmnIQqCdNObFakJQTowVNOSkOGwXDDAAAdec2xVuJJDAAAAAASUVORK5CYII=");
                }
                return _itemTriangleExpandLight;
            }
        }
    }
}