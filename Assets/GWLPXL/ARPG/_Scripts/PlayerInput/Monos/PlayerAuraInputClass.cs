using GWLPXL.ARPGCore.Auras.com;
using GWLPXL.ARPGCore.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.PlayerInput.com
{
    /// <summary>
    /// Simple class that handles Aura inputs
    /// </summary>
   
    public class PlayerAuraInputClass : MonoBehaviour, IPlayerAuraInput, IGameplayInput
    {
        [SerializeField]
        public PlayerAuraInput[] playerAuraInputs = new PlayerAuraInput[1]
        {
            new PlayerAuraInput(string.Empty, KeyCode.Alpha1, 0)
        };

        bool allowd = true;
        IActorHub actorhub;
        public void SetActorHub(IActorHub hub)
        {
            actorhub = hub;
        }

        public Aura GetFirstAuraToggle()
        {
            if (allowd == false) return null;

            string buttonName = string.Empty;
            for (int i = 0; i < playerAuraInputs.Length; i++)
            {
                buttonName = playerAuraInputs[i].AuraButton;
                if (string.IsNullOrEmpty(buttonName))
                {
                    if (Input.GetKeyDown(playerAuraInputs[i].KeyCode))
                    {
                        return actorhub.MyAuraUser.GetAuraControllerRuntime().GetEquippedAuraAtSlot(playerAuraInputs[i].AuraSlot);
                    }
                }
                else
                {
                    if (Input.GetButtonDown(buttonName) || Input.GetKeyDown(playerAuraInputs[i].KeyCode))
                    {
                        return actorhub.MyAuraUser.GetAuraControllerRuntime().GetEquippedAuraAtSlot(playerAuraInputs[i].AuraSlot);
                    }
                }
              
            }
            return null;
        }

        public void AllowInput() => allowd = true;


        public void DisableInput() => allowd = false;

       
    }
}