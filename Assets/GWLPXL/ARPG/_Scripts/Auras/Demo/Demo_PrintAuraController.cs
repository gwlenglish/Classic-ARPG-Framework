
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

namespace GWLPXL.ARPGCore.Auras.com
{

    /// <summary>
    /// An example of how to print which Auras are currently applied on a Aura Controller. 
    /// </summary>
    public class Demo_PrintAuraController : MonoBehaviour
    {
        public AuraController AuraReceiver;
        public Text Text;
        StringBuilder sb = new StringBuilder();
        string linebreak = "\n";


        void Update()
        {
            if (AuraReceiver == null)
            {
                Debug.LogError("Can't print auras without a receiver", this);
                return;
            }

            PrintAura();
        }

        /// <summary>
        /// currently shows all auras that were once applied.
        /// In your own, you might want to not print the ones that are false.
        /// </summary>
        private void PrintAura()
        {
            IReadOnlyDictionary<Aura, bool> auras = AuraReceiver.GetAppliedAuras();
            sb.Clear();
            sb.Append("Aura Name, Applied" + linebreak + linebreak);
            foreach (var kvp in auras)
            {
                sb.Append(kvp.Key.AuraData.AuraName + " , " + kvp.Value.ToString() + linebreak);
            }
            Text.text = sb.ToString();
        }
    }
}