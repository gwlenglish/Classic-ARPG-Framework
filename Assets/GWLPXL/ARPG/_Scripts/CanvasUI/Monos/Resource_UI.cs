using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Types.com;
using GWLPXL.ARPGCore.Util.com;
using UnityEngine;
using System.Text;
using TMPro;
using UnityEngine.UI;
namespace GWLPXL.ARPGCore.CanvasUI.com
{

    public class Resource_UI : MonoBehaviour, ILabel, IResourceBar, IActorUI
    {

        [SerializeField]
        TextMeshProUGUI healthText = null;
        [SerializeField]
        Image fillImage = null;
        [SerializeField]
        ResourceType type = ResourceType.Health;
        IAttributeUser attributeUser;
        StringBuilder sb = new StringBuilder();
        IActorHub hub = null;
        public void SetResource(ResourceType type) => this.type = type;
       

        public void SetUI(IActorHub hub)
        {
            attributeUser = hub.MyStats;
            attributeUser.GetRuntimeAttributes().OnResourceChanged += UpdateResource;
            

        }

        void UpdateResource(int which)
        {
            if (which == (int)type)
            {
                UpdateBar();
                UpdateLabel();
            }
        }
        public void UpdateBar()
        {
            fillImage.fillAmount = attributeUser.GetRuntimeAttributes().Percents.GetResourcePercent(type);
        }

        public void UpdateLabel()
        {
            sb.Clear();
            sb.Append(attributeUser.GetRuntimeAttributes().Description.GetResourceTotalDescription(type));
            healthText.SetText(sb.ToString());
        }


        private void OnDestroy()
        {
            if (attributeUser == null) return;
            attributeUser.GetRuntimeAttributes().OnResourceChanged -= UpdateResource;

        }

        public void SetActorHub(IActorHub newHub)
        {
            hub = newHub;
        }
    }
}