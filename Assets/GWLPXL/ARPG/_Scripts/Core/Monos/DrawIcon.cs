


using UnityEngine;

namespace GWLPXL.ARPGCore.DebugHelpers.com
{


    public class DrawIcon : MonoBehaviour
    {
        public string IconPath = string.Empty;


       
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, IconPath, true);

          


        }
#endif
    }
}
