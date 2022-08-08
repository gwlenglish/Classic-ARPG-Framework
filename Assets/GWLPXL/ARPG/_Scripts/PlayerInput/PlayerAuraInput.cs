using UnityEngine;

namespace GWLPXL.ARPGCore.PlayerInput.com
{
    [System.Serializable]
    public class PlayerAuraInput
    {
        public string AuraButton = string.Empty;
        public KeyCode KeyCode = KeyCode.None;
        public int AuraSlot = 0;

        public PlayerAuraInput(string button, KeyCode key, int slot)
        {
            AuraButton = button;
            KeyCode = key;
            AuraSlot = slot;
        }
    }
}