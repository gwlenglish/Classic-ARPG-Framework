using GWLPXL.ARPGCore.Items.com;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace GWLPXL.ARPGCore.CanvasUI.com
{
    /// <summary>
    /// interface for equipment display for generation
    /// </summary>
    public interface IDisplayEquipmentGenerate
    {
        void DisplayEquipment(Equipment newEquipment);

    }
    /// <summary>
    /// example class for displaying generated equipment
    /// </summary>
    public class EquipmentGenerateDisplay_UI : MonoBehaviour, IDisplayEquipmentGenerate
    {
        [SerializeField]
        Image eqImage = null;
        [SerializeField]
        TextMeshProUGUI nameText = null;
        [SerializeField]
        TextMeshProUGUI descriptionText = null;
        
        public void DisplayEquipment(Equipment newEquipment)
        {
            eqImage.sprite = newEquipment.GetSprite();
            nameText.SetText(newEquipment.GetGeneratedItemName());
            descriptionText.SetText(newEquipment.GetUserDescription());
        }
    }
}