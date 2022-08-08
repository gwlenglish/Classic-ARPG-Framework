using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Items.com
{
    [System.Serializable]
    public class AffixOverrides
    {
        [Tooltip("Overrides prefixes. Leave empty to disable.")]
        public List<string> Prefixes = new List<string>();
        [Tooltip("Overrides suffixes. Leave empty to disable.")]
        public List<string> Suffixes = new List<string>();
        [Tooltip("Overrides nouns. Leave empty to disable.")]
        public List<string> Nouns = new List<string>();
    }
    /// <summary>
    /// Enchanter actor example
    /// </summary>
    /// 
    [System.Serializable]
    public class EnchanterVars
    {
        public List<EquipmentEnchant> Enchants = new List<EquipmentEnchant>();
        public float InteractRange = 3;
        public EnchantingStation EnchantingStation = new EnchantingStation();
        public AffixReaderSO AffixReader = default;
        public bool RenameItemOnEnchant = true;
        public RenameType Type = RenameType.Suffix;
        
       
    }
    public class Enchanter : MonoBehaviour, IInteract
    {
        public EnchanterVars EnchanterVars = new EnchanterVars();
        public UnityEnchanterEvents EnchantEvents;
        public GameObject EnchanterUIPrefab;
        GameObject uiinstance = null;
        IEnchanterCanvas canvas;

        #region callbacks
        protected virtual void Start()
        {
            if (EnchanterUIPrefab != null)
            {
                uiinstance = Instantiate(EnchanterUIPrefab);
                canvas = uiinstance.GetComponent<IEnchanterCanvas>();
            }

            EnchanterVars. EnchantingStation.OnEnchanted += Enchanted;
            EnchanterVars. EnchantingStation.OnStationSetup += StationReady;
            EnchanterVars. EnchantingStation.OnStationClosed += StationClosed;
        }

        protected  virtual void OnDestroy()
        {
            EnchanterVars.EnchantingStation.OnEnchanted -= Enchanted;
            EnchanterVars.EnchantingStation.OnStationSetup -= StationReady;
            EnchanterVars. EnchantingStation.OnStationClosed -= StationClosed;
        }
        #endregion

        protected virtual void DebugMessage(string message, UnityEngine.Object ctx)
        {
            ARPGDebugger.DebugMessage(message, ctx);
        }
        #region scene events
        protected virtual void StationClosed(EnchantingStation station)
        {
            EnchantEvents.SceneEvents.OnStationClosed?.Invoke(station);

            DebugMessage("Station Closed", this.gameObject);
        }
        protected virtual void StationReady(EnchantingStation station)
        {
            EnchantEvents.SceneEvents.OnStationSetup?.Invoke(station);
            DebugMessage("Station Ready", this.gameObject);
        }
        protected virtual void Enchanted(Equipment equipment)
        {
            EnchantEvents.SceneEvents.OnEquipmentEnchanted?.Invoke(equipment);
            DebugMessage("Item Enchanted " + equipment.GetGeneratedItemName(), this.gameObject);
        }
        #endregion
        protected virtual IUseEnchanterCanvas CheckPreconditions(GameObject obj)
        {
            IActorHub actor = obj.GetComponent<IActorHub>();
            if (actor == null || actor.PlayerControlled == null || actor.PlayerControlled.CanvasHub.EnchanterCanvas == null)
            {
                DebugMessage("Can't enchant without an inventory", this);
                return null;
            }
            return actor.PlayerControlled.CanvasHub.EnchanterCanvas;
        }
        public bool DoInteraction(GameObject interactor)
        {
            return TryDoInteraction(interactor);
        }

        protected virtual bool TryDoInteraction(GameObject interactor)
        {
            IUseEnchanterCanvas user = CheckPreconditions(interactor);
            if (user == null) return false;
            ActorInventory inv = interactor.GetComponent<IActorHub>().MyInventory.GetInventoryRuntime();
            EnchanterVars.EnchantingStation.SetupStation(inv, EnchanterVars.Enchants, EnchanterVars.AffixReader, EnchanterVars.RenameItemOnEnchant, EnchanterVars.Type);
            EnchanterVars.EnchantingStation.AffixReaderSO = EnchanterVars.AffixReader;

            if (canvas != null)
            {
                canvas.SetStation(EnchanterVars.EnchantingStation);
                canvas.Open(user);
            }
            return true;
        }

        public bool IsInRange(GameObject interactor)
        {
            return TryRange(interactor);
        }

        protected virtual bool TryRange(GameObject interactor)
        {
            Vector3 dir = interactor.transform.position - this.transform.position;
            float sqrd = dir.sqrMagnitude;
            if (sqrd <= EnchanterVars.InteractRange * EnchanterVars.InteractRange)
            {
                return true;
            }
            return false;
        }

    }
}