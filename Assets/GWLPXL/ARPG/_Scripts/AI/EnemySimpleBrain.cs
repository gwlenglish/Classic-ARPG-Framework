using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;


namespace GWLPXL.ARPGCore.AI.com
{

    /// <summary>
    /// simple brain that aggros the last thing that hit it and idles after a certain amount of time
    /// </summary>
    public class EnemySimpleBrain : MonoBehaviour, ITick
    {
        public GameObject ActorHub = null;
        [Header("Detection")]
        [Tooltip("If greater than 0, will aggro based on sight.")]
        public float AggroDetectRange = 1;
        [Tooltip("Field of vision")]
        public float AggroSightAngle = 45;
        [Tooltip("Layers that will block sight completely.")]
        public LayerMask SightBlockingLayers;

        [Tooltip("The state to start the game in")]
        public string DefaultKey = "Aggro";
        [Header("State Keys")]
        [Tooltip("The key to indicate aggro")]
        public string AggroKey = "Aggro";
        [Tooltip("How long to remain aggro")]
        public float AggroDuration = 7f;
        [Tooltip("Idle")]
        public string IdleKey = "Idle";
        [Tooltip("Death")]
        public string DeathKey = "Death";
        [Tooltip("Hurt")]
        public string HurtKey = "Hurt";
        [Tooltip("Move")]
        public string MoveKey = "Move";
        [Range(0, 1f)]
        [Tooltip("How often to play the hurt state. At 1 (100%), you can stunlock so you maybe you don't want that.")]
        public float RandomizedHurtChance = .10f;
        [Tooltip("How long to stay in the hurt state?")]
        [SerializeField]
        protected float hurtduration = 1f;

        protected bool hurt = false;
        protected IAIEntity bb = null;
        protected float aggrotimer = 0;
        protected bool aggroed = false;
        protected bool dead;

        protected float hurtimer;
        protected virtual void Awake()
        {
            bb = GetComponent<IAIEntity>();
        }


        protected virtual void Start()
        {
            if (ActorHub != null)
            {
                EnemyHealth health = ActorHub.GetComponent<IActorHub>().MyHealth as EnemyHealth;
                if (health != null)
                {
                    health.OnDeath += Dead;
                    health.OnDamagedMe += AggroPlayer;
                    health.OnDamagedMe += TookDamage;
                }
            }

            for (int i = 0; i < DungeonMaster.Instance.GetAllSceneReferences().Length; i++)
            {
                if (bb.GetAttackTarget() == null)
                {
                    bb.SetAttackTarget(DungeonMaster.Instance.GetAllSceneReferences()[i].SceneRef.gameObject);
                }
                if (bb.GetMoveTarget() == null)
                {
                    bb.SetMoveTarget(DungeonMaster.Instance.GetAllSceneReferences()[i].SceneRef.gameObject);
                }

            }
            bb.SetStateKey(DefaultKey);
            AddTicker();
        }

        protected  void OnDestroy()
        {
            EnemyHealth health = ActorHub.GetComponent<IActorHub>().MyHealth as EnemyHealth;
            if (health != null)
            {
                health.OnDamagedMe -= AggroPlayer;
                health.OnDeath -= Dead;
                health.OnDamagedMe -= TookDamage;
            }
            RemoveTicker();
        }

        protected virtual void Dead()
        {
            dead = true;
            bb.SetStateKey(DeathKey);
        }

        protected virtual void Idle()
        {
            if (dead) return;
            bb.SetStateKey(IdleKey);
        }

        protected virtual void Move()
        {
            bb.SetStateKey(MoveKey);
        }

        protected virtual void TookDamage(IActorHub aggrotarget)
        {
            if (dead) return;
            int rando = Random.Range(0, 101);
            float percent = (float)rando / (float)Formulas.Hundred;
            //Debug.Log("Hurt Chance " + percent);
            if (percent <= RandomizedHurtChance)
            {
                hurt = true;
                bb.SetStateKey(HurtKey);
            }
            else
            {
                hurt = false;
            }

        }


        protected virtual void AggroPlayer(IActorHub aggrotarget)
        {
            if (dead) return;
            if (aggrotarget != null)
            {
                bb.SetAttackTarget(aggrotarget.MyTransform.gameObject);

            }
            Aggro();
        }

      
        protected virtual void Aggro()
        {
            bb.SetStateKey(AggroKey);
            aggroed = true;
            aggrotimer = 0;
        }

        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        protected virtual bool CheckAggro()
        {
            return bb.GetAttackTarget() != null && AggroSightAngle > 0 && AggroDetectRange > 0;
        }
        public virtual void DoTick()
        {
            if (dead) return;

            if (hurt)
            {
                hurtimer += GetTickDuration();

                if (hurtimer > hurtduration)
                {
                    hurt = false;
                    hurtimer = 0;
                    if (bb.GetMoveTarget() != null)
                    {
                        Move();
                    }
                    else if (bb.GetAttackTarget() != null)
                    {
                        Aggro();
                    }
                  
                }
                else
                {
                    return;
                }
            }



            if (CheckAggro())
            {
                //check sight and range
                Vector3 direction = bb.GetAttackTarget().transform.position - this.transform.position;
                float sqrdmag = direction.sqrMagnitude;
                if (sqrdmag <= AggroDetectRange * AggroDetectRange)
                {
                    if (CombatHelper.HasSight(bb.GetActorHub(), bb.GetAttackTarget().transform, EditorPhysicsType.Unity3D, AggroSightAngle))
                    {
                        if (CombatHelper.HasLineOfSight(bb.GetActorHub().MyTransform.gameObject, bb.GetAttackTarget(), SightBlockingLayers, AggroDetectRange + 1))//+1 is just a buffer.
                        {
                            Aggro();
                        }
                    }
                }

            }

            if (aggroed)
            {
                
                aggrotimer += GetTickDuration();
                if (aggrotimer >= AggroDuration)
                {
                    aggrotimer = 0;
                    aggroed = false;
                    Idle();
                    
                }
            }
        }

       
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AggroDetectRange);
        }
        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }

        public virtual float GetTickDuration() => Time.deltaTime;

    }
}