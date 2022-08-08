
using UnityEngine;

namespace GWLPXL.ARPGCore.Combat.com
{

    [RequireComponent(typeof(IProjectile))]
    [RequireComponent(typeof(Rigidbody))]
    public class ForwardProjectileMovement : MonoBehaviour
    {
        
        public void StartMove()
        {
            GetComponent<Rigidbody>().velocity = transform.forward * GetComponent<IProjectile>().GetProjectileData().ProjectileVars.Speed;
        }
        // Start is called before the first frame update
        void Start()
        {
            StartMove();
        }

      
    }
}