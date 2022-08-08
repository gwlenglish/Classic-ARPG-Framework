using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.PlayerInput.com
{
    [System.Serializable]
    public class PlayerCanvasInput
    {
        [Tooltip("Button will take priority over keycode. If Empty, keycode will take priority. ")]
        public string CanvasToggleButton = string.Empty;
        [Tooltip("Should really prefer input strings with whatever system you're using, but this is a nice quick way for debug and PC play.")]
        public KeyCode CanvasToggleKeyCod = KeyCode.None;
        public CanvasType ForCanvas;
        public PlayerCanvasInput(string button, KeyCode key, CanvasType canvas)
        {
            CanvasToggleButton = button;
            CanvasToggleKeyCod = key;
            ForCanvas = canvas;
        }
    }
}