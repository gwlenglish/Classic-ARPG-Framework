
using UnityEngine;

namespace GWLPXL.ARPGCore.Util.com
{


    public class LazyInActive : MonoBehaviour
    {
        public GameObject[] Objs;
        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < Objs.Length; i++)
            {
                Objs[i].SetActive(false);
            }
        }


    }
}