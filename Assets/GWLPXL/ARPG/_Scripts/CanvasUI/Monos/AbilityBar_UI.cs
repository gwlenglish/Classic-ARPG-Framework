using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Util.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.CanvasUI.com
{


    public class AbilityBar_UI : MonoBehaviour, IActorUI, ILabel, IReceiveAbilityDraggable
    {
        [SerializeField]
        Transform equippedBarPanel = null;
        [SerializeField]
        GameObject abilityButtonPrefab = null;
        [SerializeField]
        [Tooltip("If TRUE, will auto create buttons based on the number of possible equipped slots on the ability controller. " +
            "If FALSE, it will search for the buttons under the equippedBarPanel.")]
        bool autoCreate = false;
        IEquippedAbilitySlotUI[] equippedSlots = new IEquippedAbilitySlotUI[0];
        IActorHub hub = null;
        IAbilityDraggableManager draggable = null;
        private void Awake()
        {
            equippedBarPanel.gameObject.SetActive(false);
        }

        public void SetDraggableManager(IAbilityDraggableManager draggablemanager) => draggable = draggablemanager;
     
        public void SetUI(IActorHub player)
        {
            hub = player;
            IAbilityUser newPlayer = hub.MyAbilities;
            CreatePlayerEquippedAbilitylBars(newPlayer);
        }

     
      


        //assumes only left and right mouse buttons can use skills
        void CreatePlayerEquippedAbilitylBars(IAbilityUser player)
        {
            if (player == null)
            {
                Debug.LogError("Ability User is null, but trying to set Ability Bar");
                equippedBarPanel.gameObject.SetActive(false);
                return;
            }
            ClearEquippedSlots();
           
            IniEquippedSlots(player);
            UnsubSubs(player);
            Subs(player);

        }

     

     
        void IniEquippedSlots(IAbilityUser player)
        {

            Ability[] equipped = player.GetRuntimeController().GetEquippedCopy();
            if (autoCreate)
            {
                equippedSlots = new IEquippedAbilitySlotUI[equipped.Length];
                for (int i = 0; i < equippedSlots.Length; i++)
                {

                    GameObject value = Instantiate(abilityButtonPrefab, equippedBarPanel);
                    IReceiveAbilityDraggable _draggable = value.GetComponent<IReceiveAbilityDraggable>();
                    if (draggable != null)
                    {
                        _draggable.SetDraggableManager(draggable);
                    }
                    IEquippedAbilitySlotUI slotui = value.GetComponent<IEquippedAbilitySlotUI>();
                    slotui.SetAbilitySlot(player, i);
                    equippedSlots[i] = slotui;
                }
            }
            else
            {

                equippedSlots = gameObject.GetComponentsInChildren<IEquippedAbilitySlotUI>(true);
                for (int i = 0; i < equippedSlots.Length; i++)
                {
                    if (equippedSlots[i] == null) continue;
                    equippedSlots[i].GetInstance().SetActive(false);
                }
                ARPGDebugger.DebugMessage("Equipped Slots " + equippedSlots.Length, this);
                for (int i = 0; i < equipped.Length; i++)
                {
                    IReceiveAbilityDraggable _draggable = equippedSlots[i].GetInstance().GetComponent<IReceiveAbilityDraggable>();
                    if (draggable != null)
                    {
                        _draggable.SetDraggableManager(draggable);
                    }
                    equippedSlots[i].SetAbilitySlot(player, i);
                    equippedSlots[i].GetInstance().SetActive(true);
                }
            }


            equippedBarPanel.gameObject.SetActive(true);


        }
        void ClearEquippedSlots()
        {
            if (equippedSlots.Length > 0)
            {
                for (int i = 0; i < equippedSlots.Length; i++)
                {
                    Destroy(equippedSlots[i].GetInstance());
                }
            }
        }
        private void Subs(IAbilityUser newUser)
        {
            if (newUser != hub.MyAbilities && newUser != null)
            {
                newUser.GetRuntimeController().OnLearnedAbility += UpdateLabels;
                newUser.GetRuntimeController().OnForgetAbility += UpdateLabels;
                newUser.GetRuntimeController().OnAbilityEquip += UpdateLabels;
                newUser.GetRuntimeController().OnAbilityUnEquip += UpdateLabels;
            }
          
        }

        private void UnsubSubs(IAbilityUser newUser)
        {
            if (newUser != hub.MyAbilities  && hub.MyAbilities != null)
            {
                //unsub
                hub.MyAbilities.GetRuntimeController().OnAbilityEquip -= UpdateLabels;
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
            for (int i = 0; i < equippedSlots.Length; i++)
            {
                equippedSlots[i].UpdateSlot();
            }
        }

        private void OnDestroy()
        {
            if (hub.MyAbilities == null) return;
            hub.MyAbilities.GetRuntimeController().OnAbilityEquip -= UpdateLabels;
            hub.MyAbilities.GetRuntimeController().OnLearnedAbility -= UpdateLabels;
            hub.MyAbilities.GetRuntimeController().OnForgetAbility -= UpdateLabels;


        }

        public void SetActorHub(IActorHub newhub) => hub = newhub;
      
    }
}