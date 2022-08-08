
using UnityEngine;

namespace GWLPXL.ARPGCore.Auras.com
{

    /// <summary>
    /// Example of how to refresh Auras, for instance after loading into a new scene if the player isn't persistent.
    /// </summary>
    public class Demo_RefreshAuras : MonoBehaviour
    {
        public KeyCode RefreshKey;
        public AuraController Controller;
        public GameObject DemoPlayer;


        // Update is called once per frame
        void Update()
        {
            if ( Controller == null)
            {
                Debug.LogError("Can't without controller...", this);
                return;
            }

            if (Input.GetKeyDown(RefreshKey))
            {
                Controller.RefreshAuras(DemoPlayer.GetComponent<ITakeAura>());
            }
        }
    }
}