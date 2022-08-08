
using UnityEngine;

namespace GWLPXL.ARPGCore.Looting.com
{


    public class DisableCollider : MonoBehaviour, ILootFX
    {
        [SerializeField]
        Collider[] toDisable = new Collider[0];

        public void DisableFX()
        {
            for (int i = 0; i < toDisable.Length; i++)
            {
                toDisable[i].enabled = false;
            }
        }

        public void EnableFX()
        {
            for (int i = 0; i < toDisable.Length; i++)
            {
                toDisable[i].enabled = true;
            }
        }
    }
}