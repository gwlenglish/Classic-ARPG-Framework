
using UnityEngine;
using UnityEngine.UI;
namespace GWLPXL.ARPGCore.ProgressTree.com
{

    /// <summary>
    /// Demo for how to unlock a tier.
    /// </summary>
    public class DemoUnlockTierButton : MonoBehaviour
    {
        public int TierToToggle;
        public ProgressTreeHolder TheTree;
        public Text Text;
        public Image Image;
        bool toggle;
        public void ToggleTier()
        {
            toggle = TheTree.GetTierAvailable(TierToToggle);
            TheTree.SetTierAvailable(TierToToggle, !toggle);
        }

        //ideally you udate only when necessary, but this works for demo
        private void LateUpdate()
        {
            Text.text = "Tier " + TierToToggle + " is available: " + "\n" +  TheTree.GetTierAvailable(TierToToggle);
            if (TheTree.GetTierAvailable(TierToToggle))
            {
                Image.color = Color.green;
            }
            else
            {
                Image.color = Color.white;
            }
        }

    }
}