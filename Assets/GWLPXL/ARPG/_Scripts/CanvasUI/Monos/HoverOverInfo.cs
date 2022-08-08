
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
    
    public class HoverOverInfo : MonoBehaviour, IDescribeEquipment
    {
        [Header("Equipped")]
        [SerializeField]
        protected Transform equippedPanel = null;
        [SerializeField]
        protected Image equippedImage = null;
        [SerializeField]
        protected TextMeshProUGUI equippedText = null;
        [SerializeField]
        protected Image comparisonImage = null;
        [Header("Highlighted")]
        [SerializeField]
        protected Transform highlightedPanel = null;
        [SerializeField]
        protected Image highlightedEquipmentImage = null;
        [SerializeField]
        protected TextMeshProUGUI highlightedText = null;
        [SerializeField]
        protected Image highlighted = null;

        [Header("Colors")]
        [SerializeField]
        protected Color neutral = Color.white;
        [SerializeField]
        protected Color transparent = new Color(255, 255, 255, 0);
        protected virtual void Awake()
        {
            equippedImage.color = transparent;
            highlighted.color = transparent;
            equippedImage.sprite = null;
            highlighted.sprite = null;
        }
        /// <summary>
        /// my equipment
        /// </summary>
        /// <param name="description"></param>
        public virtual void DescribeHighlightedEquipment(string description)
        {
            highlightedText.SetText(description);
           
        }

        public virtual void DescribeEquippedEquipment(string description)
        {
            equippedText.SetText(description);
        }

        public virtual void DisableComparisonPanel()
        {
            highlightedPanel.gameObject.SetActive(false);
        }

        public virtual void EnableComparisonPanel()
        {
            highlightedPanel.gameObject.SetActive(true);
        }

        public virtual void SetHighlightedItem(Item highlighteditem)
        {
            if (highlighteditem == null)
            {
                highlighted.sprite = null;
                highlighted.color = transparent;
                DescribeHighlightedEquipment(string.Empty);
                highlightedPanel.gameObject.SetActive(false);
                return;
            }
            highlightedPanel.gameObject.SetActive(true);
            highlighted.color = neutral;
            highlighted.sprite = highlighteditem.GetSprite();
            DescribeHighlightedEquipment(highlighteditem.GetUserDescription());

        }

        public virtual void SetMyEquipment(Item myequipment)
        {
            if (myequipment == null)
            {
                equippedImage.sprite = null;
                equippedImage.color = transparent;
                DescribeEquippedEquipment(string.Empty);
                equippedPanel.gameObject.SetActive(false);
                return;
            }
            equippedPanel.gameObject.SetActive(true);
            equippedImage.color = neutral;
            equippedImage.sprite = myequipment.GetSprite();
            DescribeEquippedEquipment(myequipment.GetUserDescription());
        }
    }
}