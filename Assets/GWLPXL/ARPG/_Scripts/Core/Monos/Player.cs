using GWLPXL.ARPGCore.Animations.com;
using GWLPXL.ARPGCore.Auras.com;
using GWLPXL.ARPGCore.CanvasUI.com;
using GWLPXL.ARPGCore.Classes.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Leveling.com;
using GWLPXL.ARPGCore.Movement.com;
using GWLPXL.ARPGCore.Portals.com;
using GWLPXL.ARPGCore.Quests.com;
using GWLPXL.ARPGCore.Wearables.com;
using UnityEngine;
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.ProgressTree.com;
using GWLPXL.ARPGCore.PlayerInput.com;
using System.Collections.Generic;
using GWLPXL.ARPGCore.StatusEffects.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Shopping.com;
using GWLPXL.ARPGCore.States.com;
using GWLPXL.ARPGCore.Factions.com;

namespace GWLPXL.ARPGCore.com
{

    /// <summary>
    /// one stop shop to define a player
    /// 
    /// new player design not implemented yet.
    /// </summary>
    /// 

    public interface IPlayerInputHub
    {
        IPlayerAuraInput AuraInputs { get; set; }
        IAbilityPlayerInput AbilityInputs { get; set; }
        IPlayerMouseInput MouseInputs { get; set; }
        IPlayerCanvasInputToggle CanvasInputs { get; set; }
        IPlayerMovementInput MoveInputs { get; set; }

    }

    public interface IPlayerCanvasHub
    {
        IUseAbilityTreeCanvas AbilityCanvas { get; set; }
        IUseInvCanvas InvCanvas { get; set; }
        IUseSaveCanvas SaveCanvas { get; set; }
        IUseAbilityTreeCanvas AbilityTreeCanvas { get; set; }
        IUseQuesterCanvas QuestCanvas { get; set; }
        IUseAbilityInventory AbilityInventory { get; set; }
        IUseEnchanterCanvas EnchanterCanvas { get; set; }
        IUseShopKeeperCanvas Shopcanvas { get; set; }
        IUseSocketSmithCanvas SocketCanvas { get; set; }
        IUseCanvas[] CanvasLinks { get; set; }
    }

    public interface IActorHub
    {
        Animator MyAnimator { get; set; }
        Transform MyTransform { get; set; }
        INavMeshMover NavMeshAgent { get; set; }
        IPlayerControlled PlayerControlled { get; set; }
        IPlayerInputHub InputHub { get; set; }

        IInventoryUser MyInventory { get; set; }
        IAttributeUser MyStats { get; set; }
        IReceiveDamage MyHealth { get; set; }
        IMeleeCombatUser MyMelee { get; set; }
        IProjectileCombatUser MyProjectiles { get; set; }
        IUseFloatingText DungUser { get; set; }
        IWearClothing Clothing { get; set; }
        IAbilityUser MyAbilities { get; set; }
        IUseAura MyAuraUser { get; set; }
        ITakeAura MyAuraTaker { get; set; }
        IFactionMember MyFaction { get; set; }
        IClassUser MyClass { get; set; }

        IMover MyMover { get; set; }
        IAnimate MyAnim { get; set; }
        ILevel Level { get; set; }
        IQuestUser MyQuests { get; set; }
        IRecieveStatusChanges MyStatusEffects {get;set;}
        IStateMachineEntity MyStateMachine { get; set; }
        ISubscribeEvents Eventcontroller { get; set; }
    }

    public class Player : MonoBehaviour, IPlayerControlled, IActorHub, IPlayerCanvasHub, IPlayerInputHub
    {
        public PlayerActorEvents ActorEvents;
        [Header("Abilities")]
        public GameObject AbilitySystem;
        [Header("Stats, Inventory, leveling, classes, clothing")]
        public GameObject ActorSystem;
        [Header("Animator")]
        public GameObject Animator;
        [Header("Auras")]
        public GameObject AuraSystem;
        [Header("Canvases")]
        public GameObject CanvasSystem;
        [Header("Melee, Projectile, Health")]
        public GameObject CombatSystem;
        [Header("Factions")]
        public GameObject FactionSystem;
        [Header("Inputs")]
        public GameObject InputSystem;
        [Header("Locomotion")]
        public GameObject MovementSystem;
        [Header("Questing Info")]
        public GameObject QuestSystem;
        [Header("Shopping Info")]
        public GameObject ShopSystem;
        [Header("StateMachine")]
        public GameObject StateMachineSystem;
        [Header("Status Effects")]
        public GameObject StatusSystem;

        public Transform MyTransform { get; set; }
        public Animator MyAnimator { get; set; }
        public int PlayerNumber { get; } = 1;
        public IInventoryUser MyInventory { get; set; }
        public IAttributeUser MyStats { get; set; }
        public IReceiveDamage MyHealth { get; set; }
        public IUseInvCanvas InvCanvas { get; set; }
        public IUseAbilityTreeCanvas AbilityCanvas { get; set; }
        public IUseFloatingText DungUser { get; set; }

        public IWearClothing Clothing { get; set; }
        public IAbilityUser MyAbilities { get; set; }
        public IUseAura MyAuraUser { get; set; }
        public ITakeAura MyAuraTaker { get; set; }
        public ILevel Level { get; set; }
        public IClassUser MyClass { get; set; }

        public IMover MyMover { get; set; }
        public IAnimate MyAnim { get; set; }
        public IQuestUser MyQuests { get; set; }

        public IUseCanvas[] CanvasLinks { get; set; }
        public ISubscribeEvents Eventcontroller { get; set; }
        public IPlayerControlled PlayerControlled { get; set; }

        public IPlayerAuraInput AuraInputs { get ; set; }
        public IAbilityPlayerInput AbilityInputs { get; set; }
        public IPlayerMouseInput MouseInputs { get; set; }
        public IMeleeCombatUser MyMelee { get; set; }
        public IProjectileCombatUser MyProjectiles { get; set; }

        public IRecieveStatusChanges MyStatusEffects { get; set; }



        public IPlayerCanvasInputToggle CanvasInputs { get; set; }
        public IUseSaveCanvas SaveCanvas { get; set; }
        public IUseAbilityTreeCanvas AbilityTreeCanvas { get; set; }
        public IUseQuesterCanvas QuestCanvas { get; set; }
        public IUseAbilityInventory AbilityInventory { get; set; }
        public IUseShopKeeperCanvas Shopcanvas { get; set; }
        public IPlayerMovementInput MoveInputs { get; set; }
        public IPlayerInputHub InputHub { get; set; }
        public IStateMachineEntity MyStateMachine { get; set; }
        public INavMeshMover NavMeshAgent { get; set; }
        public IShopper Shopper { get; set; }
        public IFactionMember MyFaction { get; set; }
        public IPlayerCanvasHub CanvasHub { get; set; }
        public IUseEnchanterCanvas EnchanterCanvas { get; set; }
        public IUseSocketSmithCanvas SocketCanvas { get; set; }
        #region private

        bool newplayer = false;

        public IPlayerInputHub GetInputHub()
        {
            return this;
        }
        protected virtual void Awake()
        {
            NavMeshAgent = MovementSystem.GetComponent<INavMeshMover>();

            MyFaction = FactionSystem.GetComponent<IFactionMember>();

            EnchanterCanvas = CanvasSystem.GetComponent<IUseEnchanterCanvas>();
            SocketCanvas = CanvasSystem.GetComponent<IUseSocketSmithCanvas>();

            MyStateMachine =
            StateMachineSystem.GetComponent<IStateMachineEntity>();
            MyStateMachine.SetActorHub(this);

            DungUser = CombatSystem.GetComponent<IUseFloatingText>();
            DungUser.SetActorHub(this);

            MyAnimator = Animator.GetComponent<Animator>();
            MyTransform = this.transform;
            InputHub = this;
            CanvasInputs = InputSystem.GetComponent<IPlayerCanvasInputToggle>();
            CanvasInputs.SetInputHub(this);
            MoveInputs = InputSystem.GetComponent<IPlayerMovementInput>();
           
            AuraInputs = InputSystem.GetComponent<IPlayerAuraInput>();
            AuraInputs.SetActorHub(this);
           
            AbilityInputs = InputSystem.GetComponent<IAbilityPlayerInput>();
            AbilityInputs.SetActorHub(this);
            MouseInputs = InputSystem.GetComponent<IPlayerMouseInput>();

            MyStatusEffects = StatusSystem.GetComponent<IRecieveStatusChanges>();
            MyStatusEffects.SetActorHub(this);
            MyHealth = CombatSystem.GetComponent<IReceiveDamage>();
            MyHealth.SetUser(this);
            MyAbilities = AbilitySystem.GetComponent<IAbilityUser>();
            MyAbilities.SetActorHub(this);

            PlayerControlled = GetComponent<IPlayerControlled>();
            if (PlayerControlled != null)
            {
                Shopper = ShopSystem.GetComponent<IShopper>();

                Shopper.SetActorHub(this);
            }
            MyQuests = QuestSystem.GetComponent<IQuestUser>();

     

            MyMover = MovementSystem.GetComponent<IMover>();
            if (MyMover != null)
            {
                MyMover.SetActorHub(this);
            }
           

            Clothing = ActorSystem.GetComponent<IWearClothing>();
            Clothing.SetActorHub(this);

            Level = ActorSystem.GetComponent<ILevel>();
            MyClass = ActorSystem.GetComponent<IClassUser>();
            MyStats = ActorSystem.GetComponent<IAttributeUser>();
            MyInventory = ActorSystem.GetComponent<IInventoryUser>();

           
            MyMelee = CombatSystem.GetComponent<IMeleeCombatUser>();
            MyMelee.SetActorHub(this);
            MyProjectiles = CombatSystem.GetComponent<IProjectileCombatUser>();

            MyAuraUser = AuraSystem.GetComponent<IUseAura>();
            MyAuraUser.SetActorHub(this);
            MyAuraTaker = AuraSystem.GetComponent<ITakeAura>();
            MyAuraTaker.SetActorHub(this);
            CanvasHub = this;
            InvCanvas = CanvasSystem.GetComponent<IUseInvCanvas>();
             AbilityCanvas = CanvasSystem.GetComponent<IUseAbilityTreeCanvas>();
            SaveCanvas = CanvasSystem.GetComponent<IUseSaveCanvas>();
            AbilityTreeCanvas = CanvasSystem.GetComponent<IUseAbilityTreeCanvas>();
            QuestCanvas = CanvasSystem.GetComponent<IUseQuesterCanvas>();
            AbilityInventory = CanvasSystem.GetComponent<IUseAbilityInventory>();
            Shopcanvas = ShopSystem.GetComponent<IUseShopKeeperCanvas>();//not seperated at the moment between canvas and user

            CanvasLinks = CanvasSystem.GetComponents<IUseCanvas>();

            MyAnim = Animator.GetComponent<IAnimate>();

           
        }
       
        protected virtual void Start()
        {
            newplayer = DungeonMaster.Instance.DefineNewPlayer(this, PlayerNumber);
            if (newplayer == false)
            {
                Destroy(this.gameObject);
                return;
            }


           

                MyInventory.GetInventoryRuntime().SetMyUser(MyInventory);
                MyInventory.GetInventoryRuntime().SetMyActorStats(MyStats);


                if (Clothing != null)
                {
                    Clothing.UpdateClothing(MyInventory);
                }


                for (int i = 0; i < CanvasLinks.Length; i++)
                {
                    CanvasLinks[i].SetActorHub(this);
                    CanvasLinks[i].SetUserToCanvas();
                }

                IUseInvCanvas inv = GetComponent<IUseInvCanvas>();
                if (inv != null)
                {
                    InvCanvas = inv;
                }
                IUseFloatingText dung = GetComponent<IUseFloatingText>();
                if (dung != null)
                {
                    DungUser = dung;
                }

                if (MyMover != null)
            {
                MyMover.SetUpMover();

            }

            IPortalUser portalUser = GetComponent<IPortalUser>();
                if (portalUser != null)
                {
                    portalUser.Travel();
                }

                IScenePersist[] loaders = GetComponents<IScenePersist>();
                for (int i = 0; i < loaders.Length; i++)
                {
                    loaders[i].Load();
                }

            IAbilityController controller = InputSystem.GetComponent<IAbilityController>();
            if (controller != null)
            {
                controller.SetActorHub(this);
            }


            MyStats.GetRuntimeAttributes().OnLevelUpBegin += OnLevelUpBegin;
            MyStats.GetRuntimeAttributes().OnLevelUpEnd += OnLevelUpEnd;

            MyStats.GetRuntimeAttributes().LevelUp(MyStats.GetRuntimeAttributes().MyLevel);

            ActorEvents.SceneEvents.OnActorInitialized.Invoke(this);

        }

        protected virtual void OnLevelUpBegin()
        {
            MyInventory.GetInventoryRuntime().RemoveAllTraits(MyStats);
        }
        protected virtual void OnLevelUpEnd()
        {
            MyInventory.GetInventoryRuntime().ApplyAllTraits(MyStats);
            MyStats.GetRuntimeAttributes().RestoreAllResources();
        }
        /// <summary>
        /// need to fix me
        /// </summary>
        /// <param name="level"></param>
        protected virtual void ReApplyTraits(int level)
        {
            MyInventory.GetInventoryRuntime().ReApplyAllTraits(MyStats);
            MyAuraUser.GetAuraControllerRuntime().RefreshAuras(MyAuraTaker);
            MyStats.GetRuntimeAttributes().RestoreAllResources();
         
        }

    

        protected virtual void OnDestroy()
        {
            if (newplayer == false) return;
            if (Eventcontroller != null)
            {
                Eventcontroller.UnSubscribeEvents();
            }
            MyStats.GetRuntimeAttributes().OnLevelUp -= ReApplyTraits;

            IScenePersist[] loaders = GetComponents<IScenePersist>();
            for (int i = 0; i < loaders.Length; i++)
            {
                loaders[i].Save();
            }
        }

        #endregion

        #region public








        public int GetPlayerNumber()
        {
            return PlayerNumber;
        }

        public GameObject GetGameObject()
        {
            return this.gameObject;
        }

        public IActorHub GetActorHub() => this;

      



        #endregion





    }

}