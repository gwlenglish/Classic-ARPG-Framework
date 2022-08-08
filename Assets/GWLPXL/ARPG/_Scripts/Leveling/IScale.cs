
using GWLPXL.ARPGCore.com;
namespace GWLPXL.ARPGCore.Leveling.com
{


    public interface IScale
    {
        void SetUNScaledLevel(int unscaled);
        int GetScaledLevel();
        int GetUNScaledLevel();
        void SetActorHub(IActorHub newHub);


    }
}