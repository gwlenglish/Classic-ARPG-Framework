

namespace GWLPXL.ARPGCore.Leveling.com
{

    //turn this into a game event?
    public interface IVisualizeXP
    {

        void LevelUpFlaire(int forLevel);
        bool LevelUpFlaireComplete();
        void ResetFlaireComplete();
    }
}