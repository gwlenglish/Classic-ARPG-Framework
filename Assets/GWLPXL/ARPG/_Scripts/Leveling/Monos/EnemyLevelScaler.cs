


using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Leveling.com
{


    public class EnemyLevelScaler : MonoBehaviour, IScale
    {

        protected IActorHub hub;
        
        public int GetUNScaledLevel()
        {
            return hub.MyStats.GetRuntimeAttributes().MyLevel;
        }

        public int GetScaledLevel()
        {
            return Formulas.GetEnemyLevel(hub.MyStats.GetRuntimeAttributes().MyLevel);
        }

        public void SetUNScaledLevel(int unscaled)
        {
            hub.MyStats.GetRuntimeAttributes().LevelUp(unscaled);
        }

        public void SetActorHub(IActorHub newHub) => hub = newHub;

      
    }
}
