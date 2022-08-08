
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Abilities.com;

using GWLPXL.ARPGCore.States.com;


using UnityEngine;
using UnityEngine.UI;
using GWLPXL.ARPGCore.PlayerInput.com;
using GWLPXL.ARPGCore.Saving.com;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
   
/// <summary>
/// used to tick the dungeon / control the dungeon flow
/// </summary>
    public class PlayerDungeon_UI : MonoBehaviour, IDungeonUI
    {

        public DungeonState DungeonState;
        IStateMachine dungeonStateMachine = new IStateMachine();
        ActiveDungeonState active = null;
        InActiveDungeonState inactive = null;
        RefreshDungeonState refresh = null;
        [SerializeField]
        GameObject MainPanel = null;


        #region fields
        AbilityController abilityC = null;
        GraphicRaycaster gray = null;
        #endregion


        protected virtual void Awake()
        {
            gray = GetComponent<GraphicRaycaster>();
        }


        
        void Start()
        {
            //transform.root.parent.gameObject.SetActive(true);
            MainPanel.SetActive(true);
            active = new ActiveDungeonState(this);
            inactive = new InActiveDungeonState();
            refresh = new RefreshDungeonState(this, 1);
            InitalizeDungeon();

        }

        public void InitalizeDungeon()
        {
            this.gameObject.SetActive(true);
            if (this.transform.parent != null)
            {
                this.transform.parent.gameObject.SetActive(true);
            }
            SetDungeonState((int)DungeonState.Active);
            dungeonStateMachine.SetState(active);
        }


        //calls into the enemy AI and performs their behaviors, controls all the enemy AI in the scene
        void Update()
        {
            dungeonStateMachine.Tick();//controls the tick manager
        }

        /// <summary>
        /// 0 = inactive
        /// 1 = active
        /// 2 = refresh
        /// </summary>
        /// <param name="newState"></param>
        public void SetDungeonState(int newState)
        {
            DungeonState state = (DungeonState)newState;
            DungeonState = state;
            switch (DungeonState)
            {
                case DungeonState.InActive:
                    dungeonStateMachine.SetState(inactive);
                    break;
                case DungeonState.Active:
                    PlayerSceneReference[] playerRef = DungeonMaster.Instance.GetAllSceneReferences();
                    for (int i = 0; i < playerRef.Length; i++)
                    {
                        IActorHub hub = playerRef[i].SceneRef.GetComponent<IActorHub>();//eventually cache
                        IPlayerCanvasInputToggle canvstoggle = hub.InputHub.CanvasInputs;
                        //IPlayerCanvasInputToggle canvstoggle = playerRef[i].SceneRef.gameObject.GetComponent<IPlayerCanvasInputToggle>();
                        if (canvstoggle != null)
                        {
                            if (canvstoggle.HasFreezeMoverCanvasEnabled() == true)
                            {
                                dungeonStateMachine.SetState(inactive);
                                return;//dont change the state
                            }
                        }
                    }
                     dungeonStateMachine.SetState(active);
                    
                    break;
                case DungeonState.Refresh:
                    dungeonStateMachine.SetState(refresh);
                        break;
            }

        }


        public bool IsDungeonActive()
        {
            return DungeonState == DungeonState.Active;
        }

        public GraphicRaycaster GetGraphicRaycaster()
        {
            return gray;
        }


    }


}

