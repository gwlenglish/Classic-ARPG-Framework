
using GWLPXL.ARPGCore.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Util.com
{


    public class DungeonSceneLoader : MonoBehaviour
    {
        // Start is called before the first frame update
        public string StartingRoom;
        public bool WaitForLoad;
        void Start()
        {
            DungeonMaster.Instance.LoadNewDungeonScene(StartingRoom, WaitForLoad);
        }


    }
}
