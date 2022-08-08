
using GWLPXL.ARPGCore.Attributes.com;

using GWLPXL.ARPGCore.Types.com;
using GWLPXL.ARPGCore.Util.com;
using TMPro;
using UnityEngine;
using System.Text;
using GWLPXL.ARPGCore.com;
namespace GWLPXL.ARPGCore.Demo.com
{
    [RequireComponent(typeof(TextMeshPro))]
    public class NameLabelResource : MonoBehaviour, ILabel
    {
        public TextMeshPro Text = null;
        public ResourceType Show = ResourceType.None;
        public bool ShowLevel = true;

        IActorHub hub = null;
        StringBuilder sb = new StringBuilder();
        bool hasAttributes = false;
        private void Awake()
        {

            if (Text == null)
            {
                Text = GetComponent<TextMeshPro>();
            }
        }
        public void UpdateLabel()
        {
            if (Text == null)
            {
                Debug.LogError(this.gameObject.name + " needs a Text Reference");
                return;
            }
          

            if (Show != ResourceType.None && hub != null)
            {
                //eventually refactor into a static class that displays the info
                sb.Clear();
                sb.Append(hub.MyStats.GetRuntimeAttributes().ActorName);
                sb.Append("\n");
                if (ShowLevel)
                {
                    sb.Append("Level: " + hub.MyStats.GetRuntimeAttributes().MyLevel.ToString());
                }
                sb.Append("\n");
                sb.Append(hub.MyStats.GetRuntimeAttributes().Description.GetResourceTotalDescription(Show));
                Text.SetText(sb.ToString());
            }
            else
            {
                //do nothing, leave text static
            }
   
        }

        public void SetActorHub(IActorHub newHub) => hub = newHub;
       
    }
}