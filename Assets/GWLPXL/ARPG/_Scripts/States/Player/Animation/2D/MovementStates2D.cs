
using UnityEngine;


namespace GWLPXL.ARPGCore.States.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/Player/2D/Locomotion/Player_States")]

    public class MovementStates2D : ScriptableObject
    {
        public PlayerMovementState2D[] Moving = new PlayerMovementState2D[0];
        public PlayerMovementState2D[] Idle = new PlayerMovementState2D[0];

    }


  
}