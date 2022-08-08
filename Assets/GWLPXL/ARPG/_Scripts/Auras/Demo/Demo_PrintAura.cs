
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace GWLPXL.ARPGCore.Auras.com
{


    public class Demo_PrintAura : MonoBehaviour
    {
        public Aura Aura;
        public Text Text;

        StringBuilder sb = new StringBuilder();
        readonly string linebreak = "\n";

        private void Update()
        {
            if (Aura == null || Text == null)
            {
                Debug.LogError("Needs an Aura and a Text to Print the info", this);
                return;
            }
            PrintAuraAffected();
        }

        void PrintAuraAffected()
        {
            sb.Clear();
            //start with the aura name
            sb.Append("Aura: <color=blue>" + Aura.AuraData.AuraName + "</color>" + linebreak);
            //get the user and affected
            Dictionary<ITakeAura, Dictionary<ITakeAura, bool>> affected = Aura.GetAllOthersApplied();


            if (affected.Count == 0)
            {
                //no one is using this aura.
                sb.Append("NONE" + linebreak);
            }

            foreach (var kvp in affected)
            {
                if (Aura.HasAuraInstance(kvp.Key) == false)
                {
                    //it's not currently active, so dont print it. 
                    continue;
                }
                //the user of the aura
                sb.Append(linebreak + "Aura User: " + "<color=red>" + kvp.Key.GetGameObjectInstance().name + "</color>" + linebreak);

                //the other affected
                Dictionary<ITakeAura, bool> affectedOthers = kvp.Value;
                List<ITakeAura> affectedList = affectedOthers.Keys.ToList();
                sb.Append(linebreak + "Other Affected Users: " + linebreak);

                for (int j = 0; j < affectedList.Count; j++)
                {
                    sb.Append("<color=yellow>" + affectedList[j].GetGameObjectInstance().name + "</color>");
                    sb.Append(", ");
                }

                sb.Append(linebreak + linebreak);
            }

            string copy = sb.ToString();
            Text.text = copy;
            //now print
            //now linebreak

        }
    }
}