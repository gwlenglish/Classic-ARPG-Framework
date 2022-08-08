using GWLPXL.ARPGCore.Traits.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using GWLPXL.ARPGCore.Items.com;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public interface IEnchantUIElement
    {
        void SetEnchant(EquipmentEnchant item);
        EquipmentEnchant GetEnchant();
    }

    public class EnchantUIElement : MonoBehaviour, IEnchantUIElement
    {
        public TextMeshProUGUI EnchantNameText = null;
        public TextMeshProUGUI EnchantDescriptionText = null;
        public Image ItemImage;
        public string EmptyText = string.Empty;
        public Sprite EmptySprite = null;
        protected EquipmentEnchant enchant;

        protected virtual void Awake()
        {
            Setup(enchant);
        }
      

        public void SetEnchant(EquipmentEnchant enchant)
        {
            Setup(enchant);

        }

        protected virtual void Setup(EquipmentEnchant enchant)
        {
            this.enchant = enchant;
            if (enchant == null)
            {
                ItemImage.sprite = EmptySprite;
                ItemImage.enabled = false;
                EnchantNameText.SetText(EmptyText);
                EnchantDescriptionText.SetText(EmptyText);
            }
            else
            {
                ItemImage.sprite = enchant.Sprite;
                ItemImage.enabled = true;
                EnchantNameText.SetText(enchant.EnchantName);
                EnchantDescriptionText.SetText(enchant.EnchantDescription);
            }
        }

        public EquipmentEnchant GetEnchant()
        {
            return this.enchant;
        }
    }
}