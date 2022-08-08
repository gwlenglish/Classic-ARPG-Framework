
using UnityEngine;

namespace GWLPXL.ARPGCore.DebugHelpers.com
{

    /// <summary>
    /// draws a sphere using unity gizmos and attached sphere collider
    /// </summary>
    /// 
    [RequireComponent(typeof(SphereCollider))]
    public class DrawSphere : MonoBehaviour
    {
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position + GetComponent<SphereCollider>().center, GetComponent<SphereCollider>().radius);
        }
    }
}