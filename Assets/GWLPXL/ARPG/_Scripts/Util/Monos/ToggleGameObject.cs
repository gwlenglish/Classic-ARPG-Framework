
using UnityEngine;

namespace GWLPXL.ARPGCore.Util.com
{



    public class ToggleGameObject : MonoBehaviour
    {
        public GameObject ToToggle;
        public bool StartActiveState;
        private void Awake()
        {
            if (ToToggle != null)
            {
                ToToggle.SetActive(StartActiveState);
            }
        }
        public void Toggle()
        {
            if (ToToggle == null) return;
            ToToggle.SetActive(!ToToggle.activeInHierarchy);
        }
    }
}