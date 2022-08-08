
using UnityEngine;
/// <summary>
///
/// </summary>
/// 
namespace GWLPXL.ARPGCore.Looting.com
{


    public class DisableMesh : MonoBehaviour, ILootFX
    {
        [SerializeField]
        MeshRenderer[] toDisable = new MeshRenderer[0];
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