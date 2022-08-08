using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Util.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
   public interface IAbilityDraggableManager
    {
        void CreateDraggable(int fromSlot);
        void DisableDraggable();
        IDraggableAbility GetDraggableInstance();

    }

    public interface IAbilityInventoryUI
    {
        void EnableUI(bool isEnabled);
        void SetUser(IActorHub newUser);
        void ToggleUI();
        bool GetEnabled();
    }
    public class AbilityInventory_UI : MonoBehaviour, IAbilityInventoryUI, ILabel, IAbilityDraggableManager 
    {
        [SerializeField]
        bool freezeDungeon = true;
        [SerializeField]
        GameObject mainPanel = null;
        [SerializeField]
        GameObject abilityInventoryPrefab = null;
        [SerializeField]
        Transform abilityInventoryPanel = null;
        [SerializeField]
        Transform abilityInventoryContent = null;
        [Header("Draggable")]
        [SerializeField]
        DraggableAbilityEvents draggableEvents = new DraggableAbilityEvents();
        [SerializeField]
        GameObject draggablePrefab = null;
        [SerializeField]
        Transform draggablePanel = null;
        GameObject draggableInstance = null;
        IDraggableAbility idraggable = null;
        IAbilityInventorySlotUI[] inventorySlots = new IAbilityInventorySlotUI[0];
        IActorHub hub = null;
        private void Awake()
        {
            abilityInventoryPanel.gameObject.SetActive(false);
        }

        public void CreateDraggable(int fromSlot)
        {
            Ability ability = hub.MyAbilities.GetRuntimeController().GetCopyAllLearned()[fromSlot];
            if (ability == null) return;
            if (draggableInstance == null)
            {
                draggableInstance = Instantiate(draggablePrefab, draggablePanel);
                idraggable = draggableInstance.GetComponent<IDraggableAbility>();
            }
            idraggable.SetDraggable(ability, hub.MyAbilities);
            draggableInstance.SetActive(true);
            draggableEvents.SceneEvents.OnDraggableEnabled.Invoke();
        }

        public void DisableDraggable()
        {
            draggableInstance.SetActive(false);
            idraggable.SetDraggable(null, null);
            draggableEvents.SceneEvents.OnDraggableDisabled.Invoke();
        }
        public void SetUser(IActorHub newUser)
        {
            hub = newUser;
            IAbilityUser abilityUser = hub.MyAbilities;
            CreatePlayerInventoryAbilitylBars(abilityUser);
        }

       

        void CreatePlayerInventoryAbilitylBars(IAbilityUser player)
        {
            if (player == null)
            {
                abilityInventoryPanel.gameObject.SetActive(false);
                return;
            }
            ClearInventorySlots();
            IniInvetnorySlots(player);
            UnsubSubs(player);
            Subs(player);

        }
        void ClearInventorySlots()
        {
            if (inventorySlots.Length > 0)
            {
                for (int i = 0; i < inventorySlots.Length; i++)
                {
                    Destroy(inventorySlots[i].GetInstance());
                }
            }
        }
        void IniInvetnorySlots(IAbilityUser player)
        {
            Ability[] equipped = player.GetRuntimeController().GetCopyAllLearned();
            inventorySlots = new IAbilityInventorySlotUI[equipped.Length];


            for (int i = 0; i < inventorySlots.Length; i++)
            {

                GameObject value = Instantiate(abilityInventoryPrefab, abilityInventoryContent);
                IAbilityInventorySlotUI slotui = value.GetComponent<IAbilityInventorySlotUI>();
                slotui.SetAbilitySlot(player, i, this);
                inventorySlots[i] = slotui;
            }

            abilityInventoryPanel.gameObject.SetActive(true);


        }

        private void Subs(IAbilityUser newUser)
        {
            if (newUser != hub.MyAbilities && newUser != null)
            {
                newUser.GetRuntimeController().OnLearnedAbility += UpdateLabels;
                newUser.GetRuntimeController().OnForgetAbility += UpdateLabels;
            }

        }

        private void UnsubSubs(IAbilityUser newUser)
        {
            if (newUser != hub.MyAbilities && hub.MyAbilities != null)
            {
                //unsub
                hub.MyAbilities.GetRuntimeController().OnLearnedAbility -= UpdateLabels;
                hub.MyAbilities.GetRuntimeController().OnForgetAbility -= UpdateLabels;
                //clear
            }

        }

        void UpdateLabels(Ability ab, int slot)
        {
            UpdateLabel();
        }

        public void UpdateLabel()
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                inventorySlots[i].UpdateSlot();
            }
        }

        private void OnDestroy()
        {
            if (hub.MyAbilities == null) return;
            hub.MyAbilities.GetRuntimeController().OnLearnedAbility -= UpdateLabels;
            hub.MyAbilities.GetRuntimeController().OnForgetAbility -= UpdateLabels;


        }

        public IDraggableAbility GetDraggableInstance() => idraggable;

        public void EnableUI(bool isEnabled) => mainPanel.SetActive(isEnabled);
       

     
        public void ToggleUI()
        {
            mainPanel.SetActive(!mainPanel.activeInHierarchy);
            if (freezeDungeon && mainPanel.activeInHierarchy)
            {
                DungeonMaster.Instance.GetDungeonUISceneRef().SetDungeonState(0);
            }
            else if (freezeDungeon && !mainPanel.activeInHierarchy)
            {
                DungeonMaster.Instance.GetDungeonUISceneRef().SetDungeonState(1);

            }
        }

        public bool GetEnabled() => mainPanel.activeInHierarchy;

        public void SetActorHub(IActorHub newhub) => hub = newhub;
      
    }
}