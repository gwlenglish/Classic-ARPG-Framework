
using UnityEngine;

using System;


namespace GWLPXL.ARPGCore.States.com
{

   

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/Player/2D/Animation/Walk_DownRight")]

    public class WalkDownRight_2D : PlayerMovementState2D
    {


        public override bool GetTransition(IStateMachineEntity forEntity)
        {
            return forEntity.GetActorHub().InputHub.MoveInputs.GetHorizontalRaw() > 0 && forEntity.GetActorHub().InputHub.MoveInputs.GetVerticalRaw() < 0;

           
        }

        bool AICondition(IStateMachineEntity forEntity)
        {
            return forEntity.GetAI().GetDirection().x > 0 && forEntity.GetAI().GetDirection().y < 0;
        }
        public override void SetState(IStateMachine onMachine, IStateMachineEntity forEntity)
        {
            AnimateGenericMove2D state = new AnimateGenericMove2D(forEntity, Vars);

            if (forEntity.GetAI() != null)
            {
                Func<bool> AICondition() => () => this.AICondition(forEntity);
                onMachine.AddAnyTransition(state, AICondition());
                stateDic.Add(forEntity, state);

            }
            else
            {
                Func<bool> HasWalkingTarget() => () => this.GetTransition(forEntity);
                onMachine.AddAnyTransition(state, HasWalkingTarget());
                stateDic.Add(forEntity, state);
            }
         
        }
    }


}