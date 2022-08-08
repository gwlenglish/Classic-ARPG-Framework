namespace GWLPXL.ARPGCore.PlayerInput.com
{
    [System.Serializable]
    public class MouseInput
    {
        public string MouseInputButton = string.Empty;
        public MouseInput(string buttoninput)
        {
            MouseInputButton = buttoninput;
        }
    }
}