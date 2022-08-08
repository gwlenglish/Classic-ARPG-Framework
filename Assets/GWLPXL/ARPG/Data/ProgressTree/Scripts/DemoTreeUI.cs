
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace GWLPXL.ARPGCore.ProgressTree.com
{
    /// <summary>
    /// maybe unnecessary
    /// </summary>

    public class DemoTreeUI : MonoBehaviour
    {
        public ProgressTreeHolder TheTree;

        public TextMeshProUGUI Points;

       
        private void LateUpdate()
        {
            Points.text = "Points available to invest:" + "\n" + TheTree.PointsAvailable.ToString();
        }
    }
}