
using UnityEngine;

namespace GWLPXL.ARPGCore.Util.com
{

    [RequireComponent(typeof(GWLPXL.ARPGCore.com.EnemyHealth))]
    public class EnableSceneObjectOnDeath : MonoBehaviour
    {
        public GameObject[] ToEnable = new GameObject[0];

        private void OnEnable()
        {
            GetComponent<GWLPXL.ARPGCore.com.EnemyHealth>().OnDeath += EnableGO;
        }

        private void OnDisable()
        {
            GetComponent<GWLPXL.ARPGCore.com.EnemyHealth>().OnDeath -= EnableGO;
        }

        void EnableGO()
        {
            for (int i = 0; i < ToEnable.Length; i++)
            {
                ToEnable[i].SetActive(true);
            }
        }
    }
}