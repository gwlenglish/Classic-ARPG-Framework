
using UnityEngine;
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.PlayerInput.com
{
    /// <summary>
    /// Simple class that just collects inputs for basic attacks and abilities. 
    /// </summary>

    public class PlayerAbilityInputClass : MonoBehaviour, IAbilityPlayerInput, ITick, IGameplayInput
    {
        [SerializeField]
        [Tooltip("Buttons will have preference. To not check buttons, leave the button field empty.")]
        protected PlayerAbilityInput abilityInputs = new PlayerAbilityInput();


        protected bool allowed = true;
        protected IActorHub hub;
       
        //inputs
        protected virtual void Start()
        {
            AddTicker();
        }
        protected virtual void OnDestroy()
        {
            RemoveTicker();
        }
        public void AllowInput() => allowed = true;


        public void DisableInput() => allowed = false;
      

        public bool GetForceAbility() =>abilityInputs.ModiferKeys.ForceAbility.Active;

        protected virtual bool SetForceAbility()
        {
            if (allowed == false) return allowed;

            string button = abilityInputs.ModiferKeys.ForceAbility.ForceAbilityButton;
            KeyCode key = abilityInputs.ModiferKeys.ForceAbility.ForceAbilityKey;
            InputAbilityType type = abilityInputs.ModiferKeys.ForceAbility.Type;

            if (string.IsNullOrWhiteSpace(button) == false)
            {
                switch (type)
                {
                    case InputAbilityType.ToggleOnPressed:
                        bool pressed = Input.GetButton(button);
                        if (pressed)
                        {
                            abilityInputs.ModiferKeys.ForceAbility.Active = !abilityInputs.ModiferKeys.ForceAbility.Active;
                        }
                        break;
                    case InputAbilityType.OnHeld:
                        abilityInputs.ModiferKeys.ForceAbility.Active = Input.GetButton(button);
                        break;
                }
            }


            if (key != KeyCode.None)
            {
                switch (type)
                {
                    case InputAbilityType.ToggleOnPressed:
                        bool pressed = Input.GetKey(key);
                        if (pressed)
                        {
                            abilityInputs.ModiferKeys.ForceAbility.Active = !abilityInputs.ModiferKeys.ForceAbility.Active;
                        }
                        break;
                    case InputAbilityType.OnHeld:
                        abilityInputs.ModiferKeys.ForceAbility.Active = Input.GetKey(key);
                        break;
                }
            }


            return abilityInputs.ModiferKeys.ForceAbility.Active;
        }
        public Ability GetFirstBasicAttack()
        {
            if (allowed == false) return null;

            string buttonName = abilityInputs.AbilityInputs.BasicAttackButton;
            KeyCode code = abilityInputs.AbilityInputs.BasicAttackKey;
            if (string.IsNullOrWhiteSpace(buttonName) == false)
            {
                if (Input.GetButton(buttonName))
                {
                    return hub.MyAbilities.GetRuntimeController().GetBasicAttack(hub);
                }
            }


            if (code != KeyCode.None)
            {
                if (Input.GetKey(code))
                {
                    return hub.MyAbilities.GetRuntimeController().GetBasicAttack(hub);
                }
            }
          
           
            return null;
        }

       public bool GetAbilityInput(Ability forability)
        {
            for (int i = 0; i < abilityInputs.AbilityInputs.Inputs.Length; i++)
            {
                string buttonName = abilityInputs.AbilityInputs.Inputs[i].AbilityInputButton;
                KeyCode code = abilityInputs.AbilityInputs.Inputs[i].AbilityInputKey;
                int abilitySlot = abilityInputs.AbilityInputs.Inputs[i].AbilitySlot;
                if (string.IsNullOrWhiteSpace(buttonName) == false)
                {
                    if (Input.GetButton(buttonName))
                    {
                        if (hub.MyAbilities.GetRuntimeController().GetEquippedAbility(abilitySlot) == forability)
                        {
                            return true;
                        }
                    }
                }

  
                if (code != KeyCode.None)
                {
                    if (Input.GetKey(code))
                    {
                        if (hub.MyAbilities.GetRuntimeController().GetEquippedAbility(abilitySlot) == forability)
                        {
                            return true;
                        }
                    }
                }
              
             

            }

            return false;
        }
        public Ability GetFirstAbilityInput()
        {
            if (allowed == false) return null;

            for (int i = 0; i < abilityInputs.AbilityInputs.Inputs.Length; i++)
            {
                string buttonName = abilityInputs.AbilityInputs.Inputs[i].AbilityInputButton;
                KeyCode code = abilityInputs.AbilityInputs.Inputs[i].AbilityInputKey;
                int abilitySlot = abilityInputs.AbilityInputs.Inputs[i].AbilitySlot;
                if (string.IsNullOrWhiteSpace(buttonName) == false)
                {
                    if (Input.GetButton(buttonName))
                    {
                        return hub.MyAbilities.GetRuntimeController().GetEquippedAbility(abilitySlot);
                      
                    }
                }


                if (code != KeyCode.None)
                {
                    if (Input.GetKey(code))
                    {
                        return hub.MyAbilities.GetRuntimeController().GetEquippedAbility(abilitySlot);
                    }
                }

               
            }
            return null;
        }

        public void AddTicker() => TickManager.Instance.AddTicker(this);
      

        public void DoTick()
        {
            SetForceAbility();
        }

        public void RemoveTicker() => TickManager.Instance.RemoveTicker(this);


        public float GetTickDuration() => Time.deltaTime;




        public void SetActorHub(IActorHub newhub) => hub = newhub;
       










        //

    }
}