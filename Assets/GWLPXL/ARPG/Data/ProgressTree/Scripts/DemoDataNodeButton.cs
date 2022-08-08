
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace GWLPXL.ARPGCore.ProgressTree.com
{


    public class DemoDataNodeButton : MonoBehaviour
    {
        public ProgressTreeHolder TheTree;
        public TreeNodeHolder NodeHolder;
        public Text Text;
        public bool AutoName = true;
        [SerializeField]
        Transform lineSouth = null;
        [SerializeField]
        Transform lineNorth = null;
        [SerializeField]
        Image skillImage = null;
        // Start is called before the first frame update


        private void Awake()
        {
            gameObject.name = NodeHolder.Description;
        }
        public void Invest()
        {
            TheTree.Invest(NodeHolder, 1);
        }
        public void Divest()
        {
            TheTree.Divest(NodeHolder, 1);


        }
        private void LateUpdate()
        {
            int invested = TheTree.GetNodeInvestment(NodeHolder);
            int tier = TheTree.GetNodeTier(NodeHolder);
            Text.text = "Tier: " + tier.ToString() + "\n" + NodeHolder.Description + "\n" + TheTree.GetAvailableStatus(NodeHolder) + " " + invested;

            if (TheTree.GetAvailableStatus(NodeHolder))
            {
                Color newcolor = skillImage.color;
                newcolor.a = 1;
                skillImage.color = newcolor;
            }
            else
            {
                Color newcolor = skillImage.color;
                newcolor.a = .1f;
                skillImage.color = newcolor;
            }
        }


        //just convenience naming
#if UNITY_EDITOR

        private void OnValidate()
        {
            if (AutoName)
            {
                if (NodeHolder != null)
                {
                    this.gameObject.name = "DataNodeButton_" + NodeHolder.ID.Name;
                    lineSouth.name = NodeHolder.ID.Name + " South";
                    lineNorth.name = NodeHolder.ID.Name + " North";
                }
                else
                {
                    this.gameObject.name = "DataNodeButton_NULL";
                    lineSouth.name = "NULL South";
                    lineNorth.name = "NULL North";
                }
            }
        }
#endif
    }


}