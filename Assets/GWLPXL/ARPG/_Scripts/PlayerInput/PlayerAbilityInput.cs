
using UnityEngine;
namespace GWLPXL.ARPGCore.PlayerInput.com
{
    public enum InputAbilityType
    {
        ToggleOnPressed = 0,
        OnHeld = 1,
    }
    
    [System.Serializable]
    public class PlayerAbilityInput
    {
        public AbilityModifierKeys ModiferKeys = new AbilityModifierKeys();
        public AbilityInputs AbilityInputs = new AbilityInputs();

    }

    [System.Serializable]
    public class AbilityInputs
    {
        public string BasicAttackButton = "Fire1";
        public KeyCode BasicAttackKey = KeyCode.None;
        public PlayerAbilityInputSlot[] Inputs = new PlayerAbilityInputSlot[1];
    }
    [System.Serializable]
    public class AbilityModifierKeys
    {
        public ForceAbilityMod ForceAbility = new ForceAbilityMod();
    }

    [System.Serializable]
    public class ForceAbilityMod
    {
        [Tooltip("This will force the player to use an ability in the direction of the mouse, regardless of range.")]
        public string ForceAbilityButton = string.Empty;
        public KeyCode ForceAbilityKey = KeyCode.LeftShift;
        public InputAbilityType Type = InputAbilityType.OnHeld;
        public bool Active = false;
    }
}