using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Movement.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.States.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.AI.com
{


    public class EnemySimpleBrain2D : MonoBehaviour, ITick
    {
        public GameObject BlackboardObject;
        IAIEntity bb;
        Vector3 direction;
        public float AggroRange = 10;
        GameObject target;
        public float TickRate = .02f;
        public float AttackRate = 3;
        bool cooling;
        IActorHub hub;
        I2DStateMachine machine;
        IMover mover;
        bool dead;
        private void Awake()
        {
            machine = GetComponent<I2DStateMachine>();
            hub = GetComponent<IActorHub>();
            if (BlackboardObject == null)
            {
                bb = GetComponent<IAIEntity>();
            }
            else
            {
                bb = BlackboardObject.GetComponent<IAIEntity>();
            }
            mover = hub.MyMover;
            hub.MyHealth.OnDied += Died;
        }

        void Died(CombatResults results)
        {
            dead = true;
        }
        private void Start()
        {
            AddTicker();
        }
        private void OnDestroy()
        {
            RemoveTicker();
        }
        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            if (dead) return;

            direction = new Vector3(0, 0, 0);
            float sqrdmag = 0;
            PlayerSceneReference[] players = DungeonMaster.Instance.GetAllSceneReferences();
            for (int i = 0; i < players.Length; i++)
            {
                Vector3 dir = players[i].SceneRef.transform.position - this.transform.position;
                sqrdmag = dir.sqrMagnitude;
                if (sqrdmag <= AggroRange * AggroRange)
                {
                    target = players[i].SceneRef.gameObject;
                    direction = dir;
                }
            }

            bb.SetMoveTarget(target);
            bb.SetDirection(direction.normalized);
            Ability ability = bb.GetActiveAbility();

            if (Vector3.Distance(target.transform.position, this.transform.position) < ability.GetRange() && cooling == false)
            {
                bb.SetAttackTarget(target);
                mover.SetDesiredDestination(target.transform.position, ability.GetRange());
                Vector3 dir = target.transform.position - this.transform.position;
                dir.Normalize();
                machine.SetFacingDirection(dir);
                machine.SetWalkingDirection(dir);
                bool success = hub.MyAbilities.TryCastAbility(ability);
                if (success)
                {
                    cooling = true;
                    GenericTimer timer = new GenericTimer(AttackRate, EndCooling);
                }
            }
            else
            {
                //chase
                bb.SetMoveTarget(target);
                Vector3 dir = target.transform.position - this.transform.position;
                dir.Normalize();
                machine.SetFacingDirection(dir);
                machine.SetWalkingDirection(dir);
                mover.SetDesiredDestination(target.transform.position, ability.GetRange());
            }


            if (cooling)
            {

            }

        }


        void EndCooling()
        {
            cooling = false;
        }
          public float GetTickDuration() => TickRate;
      

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }
    }
}