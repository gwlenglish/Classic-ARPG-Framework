using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace GWLPXL.ARPGCore.CanvasUI.com
{

    public interface ILootText
    {
        void SetText(string text);
        void SetColor(Color newColor);
        void SetFont(TMP_FontAsset font);
    }

    public class LootText : MonoBehaviour, ILootText
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

        public void SetText(string text)
        {
            this.text.SetText(text);
        }

       
    }
}
