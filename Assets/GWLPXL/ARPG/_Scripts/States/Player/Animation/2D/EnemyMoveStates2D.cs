
using UnityEngine;


namespace GWLPXL.ARPGCore.States.com
{
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/Player/2D/Locomotion/Enemy_States")]
    public class EnemyMoveStates2D : ScriptableObject
    {
        public EnemyMovementState2D[] Moving = new EnemyMovementState2D[0];
        public EnemyMovementState2D[] Idle = new EnemyMovementState2D[0];
    }
}