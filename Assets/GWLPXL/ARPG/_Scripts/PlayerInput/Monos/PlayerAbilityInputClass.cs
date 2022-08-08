
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

            if (string.IsNullOrEmpty(abilityInputs.ModiferKeys.ForceAbility.ForceAbilityButton))
            {
                switch (abilityInputs.ModiferKeys.ForceAbility.Type)
                {
                    case InputAbilityType.ToggleOnPressed:
                        bool pressed = Input.GetKeyDown(abilityInputs.ModiferKeys.ForceAbility.ForceAbilityKey);
                        if (pressed)
                        {
                            abilityInputs.ModiferKeys.ForceAbility.Active = !abilityInputs.ModiferKeys.ForceAbility.Active;
                        }
                        break;
                    case InputAbilityType.OnHeld:
                        abilityInputs.ModiferKeys.ForceAbility.Active =  Input.GetKey(abilityInputs.ModiferKeys.ForceAbility.ForceAbilityKey);
                        break;
                }
            }
            else
            {
                switch (abilityInputs.ModiferKeys.ForceAbility.Type)
                {
                    case InputAbilityType.ToggleOnPressed:
                        bool pressed = Input.GetKeyDown(abilityInputs.ModiferKeys.ForceAbility.ForceAbilityKey) || Input.GetKeyDown(abilityInputs.ModiferKeys.ForceAbility.ForceAbilityButton);
                        if (pressed)
                        {
                            abilityInputs.ModiferKeys.ForceAbility.Active = !abilityInputs.ModiferKeys.ForceAbility.Active;
                        }
                        break;
                    case InputAbilityType.OnHeld:
                        abilityInputs.ModiferKeys.ForceAbility.Active =  Input.GetKey(abilityInputs.ModiferKeys.ForceAbility.ForceAbilityKey) || Input.GetButton(abilityInputs.ModiferKeys.ForceAbility.ForceAbilityButton);
                        break;

                }
            }
            return abilityInputs.ModiferKeys.ForceAbility.Active;
        }
        public Ability GetFirstBasicAttack()
        {
            if (allowed == false) return null;

            string buttonName = abilityInputs.AbilityInputs.BasicAttackButton;
            if (string.IsNullOrEmpty(buttonName))//if button isn't assigned, go to key
            {
                if (Input.GetKey(abilityInputs.AbilityInputs.BasicAttackKey))
                {
                    return hub.MyAbilities.GetRuntimeController().GetBasicAttack(hub);
                }
            }
            else
            {
                if (Input.GetButton(abilityInputs.AbilityInputs.BasicAttackButton) || Input.GetKey(abilityInputs.AbilityInputs.BasicAttackKey))
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
                if (string.IsNullOrEmpty(buttonName))
                {
                    if (Input.GetKey(abilityInputs.AbilityInputs.Inputs[i].AbilityInputKey))
                    {
                        if (hub.MyAbilities.GetRuntimeController().GetEquippedAbility(abilityInputs.AbilityInputs.Inputs[i].AbilitySlot) == forability)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (Input.GetButton(abilityInputs.AbilityInputs.Inputs[i].AbilityInputButton) || Input.GetKey(abilityInputs.AbilityInputs.Inputs[i].AbilityInputKey))
                    {
                        if (hub.MyAbilities.GetRuntimeController().GetEquippedAbility(abilityInputs.AbilityInputs.Inputs[i].AbilitySlot) == forability)
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
                if (string.IsNullOrEmpty(buttonName))
                {
                    if (Input.GetKey(abilityInputs.AbilityInputs.Inputs[i].AbilityInputKey))
                    {
                        return hub.MyAbilities.GetRuntimeController().GetEquippedAbility(abilityInputs.AbilityInputs.Inputs[i].AbilitySlot);
                    }
                }
                else
                {
                    if (Input.GetButton(abilityInputs.AbilityInputs.Inputs[i].AbilityInputButton) || Input.GetKey(abilityInputs.AbilityInputs.Inputs[i].AbilityInputKey))
                    {
                        return hub.MyAbilities.GetRuntimeController().GetEquippedAbility(abilityInputs.AbilityInputs.Inputs[i].AbilitySlot);
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