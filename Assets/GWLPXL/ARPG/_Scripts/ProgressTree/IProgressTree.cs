namespace GWLPXL.ARPGCore.ProgressTree.com
{
    public interface IProgressTree
    {
        void EnableUI(bool isEnabled);
        void SetUser(IUseAbilityTreeCanvas newUser);
        void ToggleUI();
        bool GetEnabled();

    }
}