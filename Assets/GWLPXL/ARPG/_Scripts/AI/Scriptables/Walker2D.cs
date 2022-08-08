
using UnityEngine;
using GWLPXL.ARPGCore.States.com;
using System;


namespace GWLPXL.ARPGCore.AI.com
{


    [System.Serializable]
    public class Walker2DVars
    {
        public GenericAnimate2D[] WalkerStates = new GenericAnimate2D[0];
        public GenericAnimate2D[] IdleStates = new GenericAnimate2D[0];
        public float Speed = 1;
        public AnimationCurve AccelCurve = null;
        public float AccelTime = 1;
        public float StoppingDistance = 1;
    }


    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/AI/Walker2D")]

    public class Walker2D : AIStateSO
    {

        public Walker2DVars Vars;

        public override IState GetState(IAIEntity forEntity)
        {
            return stateDic[forEntity];
        }


        public override void SetState(IStateMachine onMachine, IAIEntity forEntity)
        {

            Walker2DState state = new Walker2DState(forEntity, Vars);
            Func<bool> HasWalkingTarget() => () => this.GetTransition(forEntity);
            onMachine.AddAnyTransition(state, HasWalkingTarget());
            stateDic.Add(forEntity, state);
        }


    }


    public class Walker2DState : IState
    {
        public IAIEntity Entity;
        Walker2DVars vars;
        float acceltimer = 0;
        public Walker2DState(IAIEntity entity, Walker2DVars vars)
        {
            this.vars = vars;
            Entity = entity;
        }


        public void Enter()
        {
            acceltimer = 0;

        }

        public void Exit()
        {

        }

        public void Tick()
        {
            if (Entity.GetMoveTarget() == null)
            {
                Debug.LogWarning("In Move state but no target to move towards");
                return;
            }
            Vector3 target = Entity.GetMoveTarget().transform.position;
            Vector3 direction = target - Entity.GetActorHub().MyTransform.position;
            float sqrdmag = direction.sqrMagnitude;
            direction.Normalize();



            Rigidbody2D rb2d = Entity.GetActorHub().MyStateMachine.Get2D().GetRigidbody();

            if (sqrdmag > vars.StoppingDistance * vars.StoppingDistance)//keep walking
            {
                acceltimer += Time.deltaTime;
                Entity.GetActorHub().MyStateMachine.Get2D().SetWalkingDirection(direction);
                Entity.GetActorHub().MyStateMachine.Get2D().SetFacingDirection(direction);

                GlobalMoveDirection move = Entity.GetActorHub().MyStateMachine.Get2D().GetWalkingDirection();
                for (int i = 0; i < vars.WalkerStates.Length; i++)
                {
                    if (move == vars.WalkerStates[i].MovementDirection)
                    {
                        Entity.GetActorHub().MyAnimator.Play(vars.WalkerStates[i].AnimatorStateName);
                        break;
                    }
                }

               
                float percent = vars.AccelCurve.Evaluate(acceltimer / vars.AccelTime);
                Vector3 desired = rb2d.transform.position + (direction * vars.Speed * Time.deltaTime * percent);
                Entity.GetActorHub().MyMover.SetDesiredDestination(desired,  vars.StoppingDistance);
               // rb2d.MovePosition(rb2d.transform.position + (direction * vars.Speed * Time.deltaTime * percent));
              
            }

            else//stop walking
            {
                acceltimer = 0;
                Entity.GetActorHub().MyStateMachine.Get2D().SetFacingDirection(direction);
                GlobalFacingDirection face = Entity.GetActorHub().MyStateMachine.Get2D().GetFacingDirection();

                for (int i = 0; i < vars.IdleStates.Length; i++)
                {
                    if (face == vars.IdleStates[i].FacingDirection)
                    {
                        Entity.GetActorHub().MyAnimator.Play(vars.IdleStates[i].AnimatorStateName);
                        break;
                    }
                }


            }




        }
    }
}