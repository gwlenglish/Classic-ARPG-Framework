
using GWLPXL.ARPGCore.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Util.com
{

    /// <summary>
    /// eventually use cinemachine
    /// </summary>
    public class FollowPlayer : MonoBehaviour
    {

        public Vector3 offset;
        public float CameraSpeed = 1;
        public float Sensitivity = .1f;
        public float FowardLook = 1;
        Vector3 avg = Vector3.zero;
        private void LateUpdate()
        {
            if (DungeonMaster.Instance.GetAllSceneReferences().Length == 0) return;
            avg = Vector3.zero;
            Vector3 fwd = Vector3.zero;
            for (int i = 0; i < DungeonMaster.Instance.GetAllSceneReferences().Length; i++)
            {
                if (i == 0)
                {
                    fwd = DungeonMaster.Instance.GetAllSceneReferences()[i].SceneRef.transform.forward;
                }
                //just gets average
                avg += DungeonMaster.Instance.GetAllSceneReferences()[i].SceneRef.transform.position;
            }
            avg /= DungeonMaster.Instance.GetAllSceneReferences().Length;
            avg += offset;
            Vector3 lerp = Vector3.Lerp(transform.position, avg + (fwd * FowardLook), Time.deltaTime * CameraSpeed);
            transform.position = lerp;


        }
    }
}