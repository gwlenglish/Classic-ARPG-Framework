
using TMPro;
using UnityEngine;

namespace GWLPXL.ARPGCore.CanvasUI.com
{


    public class RegenText : MonoBehaviour, IFloatingText
    {
        [SerializeField]
        TextMeshProUGUI text = null;
        public void SetColor(Color newColor)
        {
            text.color = newColor;
        }

        public void SetFont(TMP_FontAsset font)
        {
            text.font = font;
        }

        public void SetText(string newText)
        {
            text.SetText(newText);
        }
    }
}