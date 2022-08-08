
using UnityEngine;

namespace GWLPXL.ARPGCore.com
{

    /// <summary>
    /// interaction occurs on trigger enter
    /// </summary>
    public class OnTriggerEnter2DInteract : MonoBehaviour
    {
      

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IInteract interact = collision.GetComponent<IInteract>();
            if (interact != null)
            {
                interact.DoInteraction(this.gameObject);
            }
        }
    }
}