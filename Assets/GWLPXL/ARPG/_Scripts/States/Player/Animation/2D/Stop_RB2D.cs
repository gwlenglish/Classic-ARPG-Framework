using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.States.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/Player/2D/Locomotion/RB2D/Stop")]

    public class Stop_RB2D : Movement2D
    {
        public override void DoLocomotion(IStateMachineEntity forEntity, float dt)
        {
            //forEntity.GetRigidbody().MovePosition((Vector2)forEntity.GetRigidbody().transform.position + (Vector2)Vars.Direction * Vars.Speed * dt);
        }

       
    }
}