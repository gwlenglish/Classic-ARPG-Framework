using GWLPXL.ARPGCore.Looting.com;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public interface ILootCanvas
    {
        void CreateLootTextUI(ILoot key);
        void RemoveLootText(ILoot key);
        bool IsActive();

    }
}