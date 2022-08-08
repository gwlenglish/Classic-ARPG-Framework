
using UnityEngine;
using TMPro;
using GWLPXL.ARPGCore.Abilities.com;
using System.Text;
using UnityEngine.UI;
using GWLPXL.ARPGCore.com;
using System.Collections.Generic;
namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public class EquippedAbilityButton : MonoBehaviour, IEquippedAbilitySlotUI, IReceiveAbilityDraggable, ITick
    {
        [SerializeField]
        EquippedAbilityButtonEvents events = new EquippedAbilityButtonEvents();
        [SerializeField]
        Sprite defaultsprite = null;
        [SerializeField]
        string interactButton = "Fire1";
        [SerializeField]
        TextMeshProUGUI text = null;
        [SerializeField]
        Image image = null;

        IAbilityUser user;
        int myslot;
        Ability inSlot;
        IAbilityDraggableManager draggable = null;
        StringBuilder sb = new StringBuilder();
        RectTransform rect = null;
        AbilityCooldownTimer coolvars;

        bool cooling = false;
        float timer = 0;
        protected virtual void Awake()
        {
            rect = GetComponent<RectTransform>();
        }
        protected virtual void OnDestroy()
        {
            RemoveTicker();
            if (user == null) return;
            user.GetRuntimeController().OnAbilityEquip -= OnSlotUpdate;
            user.GetRuntimeController().OnAbilityCooldownStart -= CooldownShow;
            user.GetRuntimeController().OnAbilityCooldownEnd -= CooldownStop;
            user.GetRuntimeController().OnAbilityStart -= HideGraphic;

        }

        protected virtual void Start()
        {
            AddTicker();
        }
        public void SetAbilitySlot(IAbilityUser forUser, int slot)
        {
            if (user != null)
            {
                user.GetRuntimeController().OnAbilityEquip -= OnSlotUpdate;
                user.GetRuntimeController().OnAbilityCooldownStart -= CooldownShow;
                user.GetRuntimeController().OnAbilityCooldownEnd -= CooldownStop;
                user.GetRuntimeController().OnAbilityStart -= HideGraphic;
            }
            user = forUser;
            myslot = slot;
            user.GetRuntimeController().OnAbilityEquip += OnSlotUpdate;
            user.GetRuntimeController().OnAbilityCooldownStart += CooldownShow;
            user.GetRuntimeController().OnAbilityCooldownEnd += CooldownStop;
            user.GetRuntimeController().OnAbilityStart += HideGraphic;
            UpdateSlot();
        }

        protected virtual void HideGraphic(Ability ability)
        {
            if (ability == inSlot)
            {
                image.fillAmount = 0;
            }
        }
        protected virtual void CooldownStop(AbilityCooldownTimer vars)
        {
            if (vars != null && inSlot == vars.Vars.skill)
            {
                cooling = false;
                coolvars = null;
                image.fillAmount = 1;
            }
        }
        protected virtual void CooldownShow(AbilityCooldownTimer vars)
        {
            if (vars != null && vars.Vars.skill == inSlot)
            {
                coolvars = vars;
                cooling = true;
                image.fillAmount = 0;
                timer = 0;
                Debug.Log("COOLDOWN SHOW");
            }
        }
        protected virtual void OnSlotUpdate(Ability ab, int slot)
        {
            if (slot == myslot)
            {
                if (coolvars != null && ab != inSlot)//instant stop showing the cooldown if we switched
                {
                    CooldownStop(coolvars);
                }
                UpdateSlot();
            }
        }
       protected virtual void UpdateLabel()
        {
            sb.Clear();
            if (inSlot != null)
            {
                sb.Append(inSlot.GetName());
            }
            text.SetText(sb.ToString());
        }

        protected virtual void UpdateGraphic()
        {
            if (inSlot != null)
            {
                if (inSlot.Data.Sprite == null)
                {
                    image.sprite = defaultsprite;
                }
                else
                {
                    image.sprite = inSlot.Data.Sprite;

                }
            }
            else
            {
                image.sprite = null;
            }
        }
        public void UpdateSlot()
        {
            if (GetInstance().activeInHierarchy == false) return;
            inSlot = user.GetRuntimeController().GetEquippedAbility(myslot);
           
            UpdateLabel();
            UpdateGraphic();
            events.SceneEvents.OnUpdated.Invoke();
            events.SceneEvents.OnEquippedSlotUpdated.Invoke(myslot, user);
        }

     
        protected virtual void LateUpdate()
        {
            TryPlaceDraggableInSlot();

        }

        /// <summary>
        /// late update so it can happen when the game is paused
        /// </summary>
        protected virtual void TryPlaceDraggableInSlot()
        {
            if (draggable == null) return;

            Vector2 localMousePosition = rect.InverseTransformPoint(Input.mousePosition);
            if (rect.rect.Contains(localMousePosition))
            {
                if (Input.GetButtonDown(interactButton))
                {
                    TryPlaceInSlot();
                }
            }
        }

        protected virtual void TryPlaceInSlot()
        {
            if (draggable.GetDraggableInstance() == null) return;
            DraggableAbilityVars vars = draggable.GetDraggableInstance().GetDraggableAbility();
            vars.AbilityUser.GetRuntimeController().EquipAbility(vars.Ability, myslot);
            draggable.DisableDraggable();

            //if we have a draggable, get it and try to equip it here.

        }

        public GameObject GetInstance() => this.gameObject;

        public void SetDraggableManager(IAbilityDraggableManager draggablemanager) => draggable = draggablemanager;

        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            if (cooling == false) return;
            timer += Time.deltaTime;
            image.fillAmount = timer / coolvars.Vars.Duration;
           // Debug.Log("TICKING " + timer / coolvars.Vars.Duration);
        }

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }

        public float GetTickDuration()
        {
            return Time.deltaTime;
        }
    }
}