
using UnityEngine;

namespace GWLPXL.ARPGCore.Auras.com
{

    /// <summary>
    /// Example of how to use Gizmos to visualize the aura range.
    /// </summary>
    public class Demo_AuraRangeVisualizer : MonoBehaviour
    {
        public Aura Aura = null;
        public bool Toggle = false;
        private void OnDrawGizmos()
        {
            if (Toggle == false) return;
            if (Aura == null) return;
            if (Aura.AuraData.AuraPrefab != null && Aura.HasAOE() == false) return;
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, Aura.AuraData.AreaRadius);
         
         
        }
    }
}