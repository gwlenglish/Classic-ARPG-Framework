
using UnityEngine;

using System;


namespace GWLPXL.ARPGCore.States.com
{

   

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/Player/2D/Animation/Walk_DownLeft")]

    public class Aggro_2D : PlayerMovementState2D
    {


        public override bool GetTransition(IStateMachineEntity forEntity)
        {
            return forEntity.GetAI().GetAttackTarget() != null;

 
        }

        public override void SetState(IStateMachine onMachine, IStateMachineEntity forEntity)
        {
            AnimateGenericMove2D state = new AnimateGenericMove2D(forEntity, Vars);
            Func<bool> HasWalkingTarget() => () => this.GetTransition(forEntity);
            onMachine.AddAnyTransition(state, HasWalkingTarget());
            stateDic.Add(forEntity, state);
        }
    }


}