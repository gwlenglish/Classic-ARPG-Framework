using GWLPXL.ARPGCore.PlayerInput.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.Util.com
{


    public class AppQuitter : MonoBehaviour, ISystemInput
    {
        public KeyCode QuitApp;
        public bool GetQuitApp()
        {
            return UnityEngine.Input.GetKeyDown(QuitApp);
        }

        private void Update()
        {
            if (GetQuitApp())
            {
                Application.Quit();
            }
        }



    }
}
