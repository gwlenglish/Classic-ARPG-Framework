using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.States.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/Player/2D/Locomotion/RB2D/RbMove")]

    public class RB2DMove : Movement2D
    {
        public override void DoLocomotion(IStateMachineEntity forEntity, float dt)
        {
            forEntity.Get2D().GetRigidbody().MovePosition((Vector2)forEntity.Get2D().GetRigidbody().transform.position + (Vector2)Vars.Direction.normalized * Vars.Speed * dt);
        }

       
    }
}