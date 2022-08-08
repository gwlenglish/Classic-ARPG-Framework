using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Util.com
{


    public class FollowPlayerRotate : MonoBehaviour
    {
        public Vector3 offset;
        public float CameraSpeed = 1;
        public float Sensitivity = .1f;
        public float FowardLook = 1;
        Vector3 avg = Vector3.zero;

        // The target we are following
        public Transform target;
        // The distance in the x-z plane to the target
        //So this would be your offset
        public float distance = 10.0f;
        // the height we want the camera to be above the target
        public float height = 5.0f;
        // How much we 
        public float heightDamping = 2.0f;
        public float rotationDamping = 3.0f;
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

            // Early out if we don't have a target
            if (!target) return;

            // Calculate the current rotation angles
            float wantedRotationAngle = target.eulerAngles.y;
            float wantedHeight = target.position.y + height;

            float currentRotationAngle = transform.eulerAngles.y;
            float currentHeight = transform.position.y;

            // Damp the rotation around the y-axis
            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

            // Damp the height
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

            // Convert the angle into a rotation
            var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            // Set the position of the camera on the x-z plane to:
            // distance meters behind the target
            transform.position = target.position;
            transform.position -= currentRotation * Vector3.forward * distance;

            // Set the height of the camera
            transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

            // Always look at the target
            transform.LookAt(target);
        }
    }
}