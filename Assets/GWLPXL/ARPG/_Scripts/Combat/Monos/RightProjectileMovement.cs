using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Combat.com
{

    [RequireComponent(typeof(IProjectile))]
    public class RightProjectileMovement : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Rigidbody2D>().velocity = transform.right * GetComponent<IProjectile>().GetProjectileData().ProjectileVars.Speed;//right is the 2d forward facing

        }


    }
}