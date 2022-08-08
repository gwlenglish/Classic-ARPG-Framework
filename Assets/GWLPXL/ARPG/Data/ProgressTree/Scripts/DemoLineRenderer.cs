
using UnityEngine;

namespace GWLPXL.ARPGCore.ProgressTree.com
{


    public class DemoLineRenderer : MonoBehaviour
    {
        public Canvas Canvas;
        public LineRenderer LineRenderer;
        public GameObject[] Line;
        public ProgressTreeHolder TheTree;
        public TreeNodeHolder PreReq;
       
        private void Start()
        {
            Vector3[] pos = new Vector3[2];


            pos[0] = Line[0].transform.position;
            pos[1] = Line[1].transform.position;
            LineRenderer.SetPositions(pos);
        }


        //probably bad practice, but this is a quick way to get the line to adjust even if the screen changes size. Normally just do it when the canvas is toggled on, but the line renderers are just placeholders anymway.
        private void LateUpdate()
        {
            if (TheTree.GetUnlockStatus(PreReq))
            {
                LineRenderer.enabled = true;
                Vector3[] pos = new Vector3[2];
                pos[0] = Line[0].transform.position;
                pos[1] = Line[1].transform.position;

                LineRenderer.SetPositions(pos);
            }
            else
            {
                LineRenderer.enabled = false;
            }
           
        }
    }
}