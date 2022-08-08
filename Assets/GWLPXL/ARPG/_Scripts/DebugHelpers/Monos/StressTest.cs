
using UnityEngine;

namespace GWLPXL.ARPGCore.DebugHelpers.com
{


    public class StressTest : MonoBehaviour
    {
        public Transform pos;
        public GameObject Object;
        public int Amount;
        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < Amount; i++)
            {
                GameObject obj = Instantiate(Object);
                obj.transform.position = pos.position;
            }
        }


    }
}
