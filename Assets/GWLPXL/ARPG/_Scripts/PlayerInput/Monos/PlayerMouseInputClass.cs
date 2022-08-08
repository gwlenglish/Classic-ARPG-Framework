
using UnityEngine;
namespace GWLPXL.ARPGCore.PlayerInput.com
{
    public class PlayerMouseInputClass : MonoBehaviour, IPlayerMouseInput, IGameplayInput
    {
        [SerializeField]
        protected MouseInput[] mouseInputs = new MouseInput[2] { new MouseInput("Fire1"), new MouseInput("Fire2") };

        protected bool allowed = true; 
        public bool GetMouseButtonTwo()
        {
            if (allowed == false) return allowed;
            if (1 > mouseInputs.Length - 1) return false;//second input not added
            return Input.GetButton(mouseInputs[1].MouseInputButton);

        }

        public bool GetMouseButtonTwoDown()
        {
            if (allowed == false) return allowed;
            if (1 > mouseInputs.Length - 1) return false;//second input not added
            return Input.GetButtonDown(mouseInputs[1].MouseInputButton);
        }

        public bool GetMouseButtoneOne()
        {
            if (allowed == false) return allowed;
            if (mouseInputs.Length == 0) return false;//first button not set
            return Input.GetButton(mouseInputs[0].MouseInputButton);
        }

        public bool GetMouseButtonOneDown()
        {
            if (allowed == false) return allowed;
            if (mouseInputs.Length == 0) return false;//first button not set
            return Input.GetButtonDown(mouseInputs[0].MouseInputButton);
        }

    

        public Vector3 GetMousePosition()
        {
            return Input.mousePosition;
        }

        public void AllowInput() => allowed = true;


    public void DisableInput() => allowed = false;

    }
}