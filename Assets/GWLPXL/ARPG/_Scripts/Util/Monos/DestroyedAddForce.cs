
using UnityEngine;

namespace GWLPXL.ARPGCore.com
{

    /// <summary>
    /// simple script that adds a force on start, used for destroyed objs
    /// </summary>
    public class DestroyedAddForce : MonoBehaviour
    {
        public Vector3 Direction = Vector3.up;
        public float AddForce = 20;
        public ForceMode ForceMode = ForceMode.Impulse;
        Rigidbody[] bodies = new Rigidbody[0];
        private void Awake()
        {
            bodies = GetComponentsInChildren<Rigidbody>();
        }


        private void Start()
        {
            DoDestroy();
        }
        void DoDestroy()
        {
            for (int i = 0; i < bodies.Length; i++)
            {
                int randoX = Random.Range(-1, 2);
                int randoZ = Random.Range(-1, 2);
                Vector3 rando = new Vector3(randoX, 0, randoZ);
                Vector3 newDir = rando + Direction;
                bodies[i].AddForce(newDir * AddForce, ForceMode);
            }
        }

    }
}