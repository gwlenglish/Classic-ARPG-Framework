
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Portals.com;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GWLPXL.ARPGCore.Demo.com
{


    public class EnterSceneOnClick : MonoBehaviour, IInteract, IPortal
    {
        public string ScenName;
        public int ThisScenePosition = 1;
        public int NextScenePosition;
        public bool InfiniteRange;
        public float Range;
        [SerializeField]
        TextMeshPro sceneText;
        private void Awake()
        {
            sceneText.text = "Enter: " + ScenName;
        }
        public bool DoInteraction(GameObject invUser)
        {
            DungeonMaster.Instance.Last.Position = NextScenePosition;
            DungeonMaster.Instance.Last.SceneName = SceneManager.GetActiveScene().name;
            DungeonMaster.Instance.LoadNewDungeonScene(ScenName, false);
            return true;
        }

        public bool IsInRange(GameObject invUser)
        {
            if (InfiniteRange)
            {
                return true;
            }
            Vector3 dir = invUser.transform.position - this.transform.position;
            float dstqrd = dir.sqrMagnitude;
            if (dstqrd <= Range * Range)
            {
                return true;
            }
            return false;
        }

        public int GetScenePosition()
        {
            return ThisScenePosition;
        }

        public Vector3 GetLocation()
        {
            return transform.position;
        }

        public string GetSceneName()
        {
            return ScenName;
        }
    }

}