using GWLPXL.ARPGCore.CanvasUI.com;
using GWLPXL.ARPGCore.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.DebugHelpers.com;
namespace GWLPXL.ARPGCore.Items.com
{
    /// <summary>
    /// you probably almost never want to select both.
    /// </summary>
    public enum RenameType
    {
        Prefix = 100,
        Suffix = 101,
        Both = 102,
        None = 199
    }
    [System.Serializable]
    public class SocketSmithVars
    {

        public SocketStation Station = new SocketStation();
        public float InteractRange = 3;
        [HideInInspector]
        public SocketTypeReaderSO SocketReader = null;//not used at the moment.
        public AffixReaderSO AffixReader = null;
        public bool RenameItemsOnSocketChanged = true;
        public RenameType RenameType = RenameType.Prefix;
        [Tooltip("Leaving elements at 0 will disable the overrides.")]
        public AffixOverrides AffixOverrides;
        public SocketSmithVars(float interactrange, SocketTypeReaderSO reader)
        {
            InteractRange = interactrange;
            this.SocketReader = reader;
        }

    }

    public class SocketSmith : MonoBehaviour, IInteract
    {
        public SocketSmithVars SocketSmithVars;
        public UnitySocketSmithEvents SocketEvents;
        public GameObject SocketSmithUIPrefab = default;
        GameObject uiinstance = null;
        ISocketSmithCanvas canvas;

        protected virtual void OnDestroy()
        {
            if (canvas != null)
            {
                SocketSmithVars.Station.OnSmithOpen -= Subscribe;
                SocketSmithVars.Station.OnSmithClosed -= UnSubscribe;
            }
        }
        protected virtual void Start()
        {
            Setup();
        }

        protected virtual void Setup()
        {
            SocketSmithVars.Station = new SocketStation();
            SocketSmithVars.Station.OnSmithOpen += Subscribe;
            SocketSmithVars.Station.OnSmithClosed += UnSubscribe;
            if (SocketSmithUIPrefab != null)
            {
                uiinstance = Instantiate(SocketSmithUIPrefab);
                canvas = uiinstance.GetComponent<ISocketSmithCanvas>();
                
                canvas.Close();
                
            }

        }

        protected virtual void Subscribe()
        {
            SocketSmithVars.Station.OnAddSocketable += AddedSocketItem;
            SocketSmithVars.Station.OnStationSetup += StationReady;
            SocketSmithVars.Station.OnStationClosed += StationClosed;

            SocketSmithVars.Station.OnRemoveSocketable += RemovedSocketItem;
            SocketSmithVars.Station.OnFailToAddStorageIssue += FailedStorage;
        }
        protected virtual void UnSubscribe()
        {
            SocketSmithVars.Station.OnAddSocketable -= AddedSocketItem;
            SocketSmithVars.Station.OnStationSetup -= StationReady;
            SocketSmithVars.Station.OnStationClosed -= StationClosed;

            SocketSmithVars.Station.OnRemoveSocketable -= RemovedSocketItem;
            SocketSmithVars.Station.OnFailToAddStorageIssue -= FailedStorage;
        }
        protected virtual void FailedStorage()
        {
            ARPGDebugger.DebugMessage("Failed to add storage, inventory storage problem " , this);
        }
        protected virtual void RemovedSocketItem(Equipment equipment)
        {
            ARPGDebugger.DebugMessage("Socket changed on EQUIPMENT " + equipment.GetGeneratedItemName(), this);
        }
        protected virtual void StationClosed(SocketStation station)
        {
            SocketEvents.SceneEvents.OnStationClosed?.Invoke(station);
            ARPGDebugger.DebugMessage("Station Closed", this);
        }
        protected virtual void StationReady(SocketStation station)
        {
            SocketEvents.SceneEvents.OnStationSetup?.Invoke(station);
            ARPGDebugger.DebugMessage("Station Ready", this);
        }
        protected virtual  void AddedSocketItem(Equipment equipment)
        {
            SocketEvents.SceneEvents.OnEquipmentSocketed?.Invoke(equipment);
            ARPGDebugger.DebugMessage("Item Socketable Added " + equipment.GetGeneratedItemName(), this);
        }
        protected virtual IUseSocketSmithCanvas CheckPreconditions(GameObject obj)
        {
            IActorHub actor = obj.GetComponent<IActorHub>();
            if (actor == null || actor.PlayerControlled == null || actor.PlayerControlled.CanvasHub.EnchanterCanvas == null)
            {
                ARPGCore.DebugHelpers.com.ARPGDebugger.DebugMessage("Can't enchant without an inventory", this);
                return null;
            }
            return actor.PlayerControlled.CanvasHub.SocketCanvas;
        }
        public bool DoInteraction(GameObject interactor)
        {
            return TryOpenSocketSmith(interactor);
        }

        protected bool TryOpenSocketSmith(GameObject interactor)
        {
            IUseSocketSmithCanvas hub = CheckPreconditions(interactor);
            if (hub == null) return false;
            ActorInventory inv = interactor.GetComponent<IActorHub>().MyInventory.GetInventoryRuntime();
            SocketSmithVars.Station.SetupStation(inv, SocketSmithVars.AffixReader, SocketSmithVars.RenameItemsOnSocketChanged, SocketSmithVars.RenameType, SocketSmithVars.AffixOverrides);
            if (canvas != null)
            {
                canvas.SetStation(SocketSmithVars.Station);
                canvas.Open(hub);
            }
            return true;
        }

        public bool IsInRange(GameObject interactor)
        {
            return TryDetectRange(interactor);
        }

        protected virtual bool TryDetectRange(GameObject interactor)
        {
            Vector3 dir = interactor.transform.position - this.transform.position;
            float sqrdmag = dir.sqrMagnitude;
            if (sqrdmag <= SocketSmithVars.InteractRange * SocketSmithVars.InteractRange)
            {
                return true;
            }
            return false;
        }

    }
}