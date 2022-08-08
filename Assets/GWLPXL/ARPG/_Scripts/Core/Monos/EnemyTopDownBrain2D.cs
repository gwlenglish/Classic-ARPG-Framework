
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Movement.com;

using GWLPXL.ARPGCore.States.com;
using System;
using UnityEngine;

namespace GWLPXL.ARPGCore.com
{


    public class EnemyTopDownBrain2D : MonoBehaviour, ITick
    {
 
        [SerializeField]
        [Tooltip("Enemy will chase the player")]
        float aggroRange = 3;
        [SerializeField]
        [Tooltip("Enemy will stop chasing and go back to its starting position")]
        float loseAggroRange = 7;
        [SerializeField]
        [Tooltip("How often to calculate new behaviors in seconds.")]
        float updateRate = .25f;
        [SerializeField]
        bool chaseOnDamaged = true;
        [SerializeField]
        float chaseDuration = 10f;

        IStateMachine locoMachine;
        EnemyChasing2D chasing2d;
        EnemyIdle2D idle2d;

        IMover mover;
        IReceiveDamage health;
        IAttributeUser myself;
        IActorHub hub;

        private void Awake()
        {
            hub = GetComponent<IActorHub>();
            myself = GetComponent<IAttributeUser>();
            mover = GetComponent<IMover>();
            health = GetComponent<IReceiveDamage>();
           
        }



        void SetDestination(IActorHub target)
        {
            if (target == null || target == myself) return;
            chasing2d.chaseTarget = target.MyTransform;
            chasing2d.chaseTimer = 0;
           // locoMachine.SetState(chasing2d);


        }

       bool Chasing()
        {
            return chasing2d.chaseTarget != null && chasing2d.chaseTimer < chasing2d.chaseDuration;
        }
       
    
        private void Start()
        {
            idle2d = new EnemyIdle2D(hub, this.transform.position, aggroRange);
            chasing2d = new EnemyChasing2D(hub, GetTickDuration(), chaseDuration);

            Func<bool> StartChasing() => () => this.Chasing();
            Func<bool> StopChasing() => () => this.Chasing() == false;

            locoMachine = new IStateMachine();
           
            locoMachine.AddTransition(idle2d, chasing2d, StartChasing());
            locoMachine.AddTransition(chasing2d, idle2d, StopChasing());

            AddTicker();
            if (chaseOnDamaged)
            {
                EnemyHealth health = GetComponent<EnemyHealth>();
                if (health != null)
                {
                    health.OnDamagedMe += SetDestination;
                }
            }

            locoMachine.SetState(idle2d);
        }

        private void OnDestroy()
        {

            RemoveTicker();
            if (chaseOnDamaged)
            {
                EnemyHealth health = GetComponent<EnemyHealth>();
                if (health != null)
                {
                    health.OnDamagedMe -= SetDestination;
                }
            }
        }
        public void AddTicker() => TickManager.Instance.AddTicker(this);
     

        public void DoTick()
        {
            if (health.IsDead()) return;

            locoMachine.Tick();

        }



        public float GetTickDuration() => updateRate;
     



        public void RemoveTicker() => TickManager.Instance.RemoveTicker(this);
       
    }
}
