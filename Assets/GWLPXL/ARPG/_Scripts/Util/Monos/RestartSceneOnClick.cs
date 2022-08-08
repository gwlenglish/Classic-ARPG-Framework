
using GWLPXL.ARPGCore.com;
using TMPro;
using UnityEngine;

namespace GWLPXL.ARPGCore.Demo.com
{


    public class RestartSceneOnClick : MonoBehaviour, IInteract
    {
        [SerializeField]
        TextMeshPro text;

        private void Awake()
        {
            text.text = "Restart Scene";
        }
        public bool DoInteraction(GameObject invUser)
        {
            bool performed = false;
            DungeonMaster.Instance.ReloadScene();


            return performed;
        }

        public bool IsInRange(GameObject invUser)
        {
            return true;
        }
    }
}
