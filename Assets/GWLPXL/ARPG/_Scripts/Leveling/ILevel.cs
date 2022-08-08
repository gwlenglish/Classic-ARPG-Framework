
namespace GWLPXL.ARPGCore.Leveling.com
{



    public interface ILevel
    {
        void SetVisualizer(IVisualizeXP visualXP);
        int GetCurrentLevel();
        void EarnXP(int earnedAmount);
        int GetCurrentXP();
        int GetNextLevelXP();
        void LevelFlaire(int newLevel);
        bool Leveling();
    }
}