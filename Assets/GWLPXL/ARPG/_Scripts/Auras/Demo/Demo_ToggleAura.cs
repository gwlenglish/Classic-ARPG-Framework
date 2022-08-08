
using UnityEngine;


namespace GWLPXL.ARPGCore.Auras.com
{

    /// <summary>
    /// Example of toggling an Aura through an Aura Controller. 
    /// </summary>
    public class Demo_ToggleAura : MonoBehaviour
    {
        public KeyCode ToggleKey;
        public Aura Aura;
        public AuraController Controller;
        public GameObject DemoPlayer;
     

        // Update is called once per frame
        void Update()
        {
            if (Aura == null || Controller == null)
            {
                Debug.LogError("Can't toggle an aura without an aura and receiver...", this);
                return;
            }

            if (Input.GetKeyDown(ToggleKey))
            {
                Controller.ToggleEquippedAura(Aura, DemoPlayer.GetComponent<ITakeAura>());
            }
        }
    }
}