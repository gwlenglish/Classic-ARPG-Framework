
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GWLPXL.ARPGCore.PlayerInput.com
{
   
  public interface IPlayerMovementInput
    {
        float GetHorizontalRaw();
        float GetVerticalRaw();
    }

    public class PlayerMoverInputClass : MonoBehaviour, IPlayerMovementInput, IGameplayInput
    {
        public string MoveAxisHorizontal = "Horizontal";
        public string MoveAxisVertical = "Vertical";
        bool allowed = true;
        public void AllowInput()
        {
            allowed = true;
        }

        public void DisableInput()
        {
            allowed = false;
        }

        public float GetHorizontalRaw() => (allowed) ? UnityEngine.Input.GetAxisRaw(MoveAxisHorizontal) : 0;


        public float GetVerticalRaw() => (allowed) ?  UnityEngine.Input.GetAxisRaw(MoveAxisVertical) : 0;
       
    }
}