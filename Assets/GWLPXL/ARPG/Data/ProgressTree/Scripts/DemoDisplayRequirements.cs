using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
namespace GWLPXL.ARPGCore.ProgressTree.com
{


    public class DemoDisplayRequirements : MonoBehaviour
    {
        public ProgressTreeHolder TheTree;
        public TreeNodeHolder DataNode;
        public Image SkilLRequiredImage;
        public Text SkilLRequiredText;

        StringBuilder sb = new StringBuilder();
      

        //not great to do it in late update, but quick and dirty
        private void LateUpdate()
        {
            NodeRequirements[] requirements = DataNode.Requirements;
            if (requirements.Length == 0)
            {
                //SkilLRequiredImage.enabled = false;
                SkilLRequiredText.text = string.Empty;
                Color newcolor = SkilLRequiredImage.color;
                newcolor.a = 0;
                SkilLRequiredImage.color = newcolor;
                return;
            }
            else
            {
                Color newcolor = SkilLRequiredImage.color;
                newcolor.a = 1;
                SkilLRequiredImage.color = newcolor;
            }
          
            sb.Clear();
            sb.Append("Requirements" + "\n");
            for (int i = 0; i < requirements.Length; i++)
            {
                string name = requirements[i].RequiredNode.ID.Name;
                int requiredAMount = requirements[i].RequiredInvestment;
                int invested = TheTree.GetNodeInvestment(requirements[i].RequiredNode);
                requiredAMount -= invested;
                if (requiredAMount <= 0)
                {
                    //unlocked!
                    requiredAMount = 0;
                }
                sb.Append("\n" + "needs "  + requiredAMount.ToString() +" more points in " + name + " to unlock");
            }

            SkilLRequiredText.text = sb.ToString();
        }
    }
}