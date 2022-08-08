using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.AI.com;
using GWLPXL.ARPGCore.Animations.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Auras.com;
using GWLPXL.ARPGCore.CanvasUI.com;
using GWLPXL.ARPGCore.Classes.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Factions.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Leveling.com;
using GWLPXL.ARPGCore.Movement.com;
using GWLPXL.ARPGCore.ProgressTree.com;
using GWLPXL.ARPGCore.Quests.com;
using GWLPXL.ARPGCore.States.com;
using GWLPXL.ARPGCore.StatusEffects.com;
using GWLPXL.ARPGCore.Util.com;
using GWLPXL.ARPGCore.Wearables.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.com
{


    /// <summary>
    /// one stop shop to define an enemy
    /// </summary>
    public class Enemy : MonoBehaviour, IActorHub
    {
        public EnemyActorEvents ActorEvents;
        [Header("Abilities")]
        public GameObject AbilitySystem = null;
        [Header("Stats, Inventory, leveling, classes, clothing")]
        public GameObject ActorSystem = null;
        [Header("AI")]
        public GameObject AISystem = null;
        [Header("Animator")]
        public Animator Animator = null;
        [Header("Auras")]
        public GameObject AuraSystem = null;
        [Header("Melee, Projectile, Health")]
        public GameObject CombatSystem = null;
        [Header("Enemy UI Info")]
        public GameObject EnemyInfo = null;
        [Header("Factions")]
        public GameObject FactionSystem = null;
        [Header("Locomotion")]
        public GameObject MovementSystem = null;
        [Header("Status Effects")]
        public GameObject StatusSystem = null;

        public Animator MyAnimator { get; set; }
        public Transform MyTransform { get; set; }

        public IUseFloatingText DungUser { get; set; }
        public IAttributeUser MyStats { get; set; }
        public IReceiveDamage MyHealth { get; set; }
        public IMover MyMover { get; set; }
        public IAnimate MyAnim { get; set; }
        public IAbilityUser MyAbilities { get; set; }
        public IGiveXP XPGiver { get; set; }
        public IEventController EventController { get; set; }
        public IPlayerControlled PlayerControlled { get; set; }
        public IInventoryUser MyInventory { get; set; }
        public IWearClothing Clothing { get; set; }
        public IUseAura MyAuraUser { get; set; }
        public ITakeAura MyAuraTaker { get; set; }
        public ILevel Level { get; set; }
        public IClassUser MyClass { get; set; }
        public IQuestUser MyQuests { get; set; }
        public ISubscribeEvents Eventcontroller { get; set; }
        public IMeleeCombatUser MyMelee { get; set; }
        public IProjectileCombatUser MyProjectiles { get; set; }
        public IRecieveStatusChanges MyStatusEffects { get; set; }
        public IPlayerInputHub InputHub { get; set; }
        public IStateMachineEntity MyStateMachine { get; set; }
        public INavMeshMover NavMeshAgent { get; set; }
        public IFactionMember MyFaction { get; set; }

        [Range(1, 99)]
        [SerializeField]
        public int InitialLevel = 1;


        protected virtual void Awake()
        {
            PlayerControlled = null;
            NavMeshAgent = MovementSystem.GetComponent<INavMeshMover>();

            MyFaction = FactionSystem.GetComponent<IFactionMember>();

            MyStateMachine = AISystem.GetComponent<IStateMachineEntity>();
            MyStateMachine.SetActorHub(this);
            DungUser = CombatSystem.GetComponent<IUseFloatingText>();
            DungUser.SetActorHub(this);
            MyStatusEffects = StatusSystem.GetComponent<IRecieveStatusChanges>();
            MyStatusEffects.SetActorHub(this);
            MyHealth = CombatSystem.GetComponent<IReceiveDamage>();
            MyHealth.SetUser(this);
            MyAbilities = AbilitySystem.GetComponent<IAbilityUser>();
            MyAbilities.SetActorHub(this);
            MyMover = MovementSystem.GetComponent<IMover>();
            if (MyMover != null)
            {
                MyMover.SetActorHub(this);
            }

            DungUser = CombatSystem.GetComponent<IUseFloatingText>();
            DungUser.SetActorHub(this);

            MyAnimator = GetComponentInChildren<Animator>();
            InputHub = null;
            MyTransform = this.transform;
  
            MyMelee = CombatSystem.GetComponent<IMeleeCombatUser>();
            MyMelee.SetActorHub(this);
            MyProjectiles = CombatSystem.GetComponent<IProjectileCombatUser>();
            MyStats = ActorSystem.GetComponent<IAttributeUser>();

            XPGiver = CombatSystem.GetComponent<IGiveXP>();
            MyAnim = GetComponentInChildren<IAnimate>();//deprecate
            EventController = GetComponent<IEventController>();

            MyAuraTaker = AuraSystem.GetComponent<ITakeAura>();
            MyAuraUser = AuraSystem.GetComponent<IUseAura>();
            MyAuraUser.SetActorHub(this);
            MyAuraTaker.SetActorHub(this);
            MyInventory = ActorSystem.GetComponent<IInventoryUser>();

            IKillTracked[] killtracked = GetComponentsInChildren<IKillTracked>();
            for (int i = 0; i < killtracked.Length; i++)
            {
                killtracked[i].SetActorHub(this);
            }

        }


        protected virtual void Start()
        {
            if (MyInventory != null)
            {
                MyInventory.GetInventoryRuntime().SetMyUser(MyInventory);
                MyInventory.GetInventoryRuntime().SetMyActorStats(MyStats);
            }


            if (EnemyInfo != null)
            {
                IResourceBar[] rbars = EnemyInfo.GetComponentsInChildren<IResourceBar>();
                for (int i = 0; i < rbars.Length; i++)
                {
                    rbars[i].SetActorHub(this);
                }

                ILabel[] labels = EnemyInfo.GetComponentsInChildren<ILabel>();
                for (int i = 0; i < labels.Length; i++)
                {
                    labels[i].SetActorHub(this);
                }
            }
            IUseCanvas[] canvasLinks = GetComponentsInChildren<IUseCanvas>();
            for (int i = 0; i < canvasLinks.Length; i++)
            {
                canvasLinks[i].SetUserToCanvas();
            }

          
            if (EventController != null)
            {
                EventController.SubEvents();
            }

            IScale scaler = CombatSystem.GetComponent<IScale>();
     
            if (scaler != null)
            {
                scaler.SetActorHub(this);
                MyStats.GetRuntimeAttributes().LevelUp(InitialLevel);
                int scaled = scaler.GetScaledLevel();
                MyStats.GetRuntimeAttributes().LevelUp(scaled);
            }
            else
            {
                MyStats.GetRuntimeAttributes().LevelUp(InitialLevel);
            }

            ActorEvents.SceneEvents.OnActorInitialized.Invoke(this);
        }


        protected virtual void OnDestroy()
        {
       
            if (EventController != null)
            {
                EventController.UnSubEvents();
            }
        }






















    }
}