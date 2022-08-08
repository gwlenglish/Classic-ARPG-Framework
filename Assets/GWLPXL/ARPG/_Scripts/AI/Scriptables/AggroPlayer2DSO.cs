
using UnityEngine;
using GWLPXL.ARPGCore.States.com;
using System;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.AI.com
{

    [System.Serializable]
    public class AggroPlayer2DVars
    {

        public float AggroRadius = 3f;
        public float StoppingD = 1;
        public float Speed = 1;
        public float Dt = .02f;
        public bool UseDeltaTime = true;

        public AggroPlayer2DVars(float dt, float stoppingDistance, float speed, float aggroRadius, bool useDeltaTime)
        {
            this.StoppingD = stoppingDistance;
            this.Speed = speed;
            this.Dt = dt;
            UseDeltaTime = useDeltaTime;
            this.AggroRadius = aggroRadius;
        }
    }

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/AI/2D/AggroPlayer")]

    public class AggroPlayer2DSO : AIStateSO
    {

        public AggroPlayer2DVars Vars;

        public override IState GetState(IAIEntity forEntity)
        {
            return stateDic[forEntity];
        }

       

        public override void SetState(IStateMachine onMachine, IAIEntity forEntity)
        {

            AggroPlayer2D state = new AggroPlayer2D(forEntity, null, Vars);
            Func<bool> HasPlayerTarget() => () => this.GetTransition(forEntity);
            onMachine.AddAnyTransition(state, HasPlayerTarget());
            stateDic.Add(forEntity, state);
        }

        
    }

    [System.Serializable]
    public class AggroPlayer2D : IState
    {
        public IAIEntity Entity;
        public Transform Target;
        AggroPlayer2DVars vars;

        Rigidbody2D entityRb;
        public AggroPlayer2D(IAIEntity entity, Transform target, AggroPlayer2DVars vars)
        {
            this.vars = vars;
            Entity = entity;
            Target = target;
        }


        public void Enter()
        {
            if (vars.UseDeltaTime)
            {
                vars.Dt = Time.deltaTime;
            }

            entityRb = Entity.GetActorHub().MyTransform.GetComponent<Rigidbody2D>();

        }

        public void Exit()
        {

        }

        public void Tick()
        {
            if (Target == null)//if no target, find closest player
            {
                //find closest
                PlayerSceneReference[] refs = DungeonMaster.Instance.GetAllSceneReferences();
                float closestd = Mathf.Infinity;
                Transform closest = null;
                for (int i = 0; i < refs.Length; i++)
                {
                    Vector2 dir2 = (Vector2)refs[i].SceneRef.transform.position - (Vector2)Entity.GetActorHub().MyTransform.position;
                    float sqrdd = dir2.sqrMagnitude;
                    if (sqrdd < closestd)
                    {
                        closestd = sqrdd;
                        closest = refs[i].SceneRef.transform;
                    }
                }
                Target = closest;
            }

            if (Target == null) return;//still no target, dont do anything


            Vector2 dir = (Vector2)Target.position - (Vector2)Entity.GetActorHub().MyTransform.position;
            float sqrdMag = dir.sqrMagnitude;
            if (sqrdMag > this.vars.AggroRadius) return;//if closest player isn't in range, exit

            if (sqrdMag <= vars.StoppingD * vars.StoppingD) return;//if we are close enough, exit

            dir.Normalize();
            entityRb.MovePosition((Vector2)Entity.GetActorHub().MyTransform.position + dir * vars.Speed * vars.Dt);//move towards target
        }
    }


}