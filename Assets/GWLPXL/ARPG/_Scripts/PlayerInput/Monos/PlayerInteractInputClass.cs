
using UnityEngine;

namespace GWLPXL.ARPGCore.PlayerInput.com
{

    public interface IInteractPlayerInput
    {
        bool GetInteracted();
    }

    public class PlayerInteractInputClass : MonoBehaviour, IGameplayInput, IInteractPlayerInput
    {
        public string InteractButton = "Fire1";
        bool allow = true;
       
        public void AllowInput() => allow = true;


        public void DisableInput() => allow = false;
    


        public float GetTickDuration() => Time.deltaTime;

        public bool GetInteracted() => allow && Input.GetButtonDown(InteractButton);
       
    }
}