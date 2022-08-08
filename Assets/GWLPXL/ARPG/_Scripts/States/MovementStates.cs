
using UnityEngine;


namespace GWLPXL.ARPGCore.States.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/Player/3D/Locomotion/States")]

    public class MovementStates : ScriptableObject
    {
        public PlayerMovementState[] Moving = new PlayerMovementState[0];
        public PlayerMovementState[] Idle = new PlayerMovementState[0];
    }
}
