
using UnityEngine;
using GWLPXL.ARPGCore.States.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;

namespace GWLPXL.ARPGCore.AI.com
{
    
    /// <summary>
    /// 
    /// </summary>
    public class EnemyStateMachine : MonoBehaviour, ITick, IStateMachineEntity
    {
        public GameObject BlackBoard = null;
        public AIStateSO[] States = new AIStateSO[0];

        protected IStateMachine machine;
        protected IActorHub hub;
        protected IAIEntity ai;
        protected I2DStateMachine state2d = null;
        public I2DStateMachine Machine2D { get; set; }
        bool moving;
        protected virtual void Awake()
        {
            state2d = GetComponent<I2DStateMachine>();
            if (BlackBoard == null) ai = GetComponent<IAIEntity>();
            if (BlackBoard != null) ai = BlackBoard.GetComponent<IAIEntity>();
        }
        protected virtual void Start()
        {
            machine = new IStateMachine();
            for (int i = 0; i < States.Length; i++)
            {
                States[i].SetState(machine, ai);
            }
       
            AddTicker();
        }

      
        protected virtual void OnDestroy()
        {
      
            RemoveTicker();
        }
        public virtual void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public virtual void DoTick()
        {
            machine.Tick();
            if (moving)
            {
               
            }
           // Debug.Log("Enemy" + machine.GetCurrentlyRunnnig());
          
        }

        public virtual float GetTickDuration()
        {
            return Time.deltaTime;
        }

        public virtual void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }

        public virtual Transform GetInstance() => this.transform;

     

        public virtual IActorHub GetActorHub() => hub;

        public virtual I2DStateMachine Get2D() => state2d;
     

        public virtual void SetActorHub(IActorHub newHub)
        {
            hub = newHub;
        }

        public virtual IAIEntity GetAI()
        {
            return ai;
        }
    }
}