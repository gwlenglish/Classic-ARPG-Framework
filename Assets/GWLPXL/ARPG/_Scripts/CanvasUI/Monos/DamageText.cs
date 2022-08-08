
using UnityEngine;
using TMPro;
namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public interface IFloatingText
    {
        void SetFont(TMP_FontAsset font);
        void SetColor(Color newColor);
        void SetText(string newText);
    }

    public class DamageText : MonoBehaviour, IFloatingText
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