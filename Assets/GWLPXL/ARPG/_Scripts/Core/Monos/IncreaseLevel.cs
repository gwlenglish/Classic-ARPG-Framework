
using GWLPXL.ARPGCore.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Leveling.com
{

    /// <summary>
    /// possibly revise into scriptable object
    /// </summary>
    public class IncreaseLevel : MonoBehaviour
    {
        public void LevelUp(GameObject obj)
        {
            IActorHub hub = obj.GetComponent<IActorHub>();
            if (hub == null) return;

            ILevel leveler = hub.Level;
            if (leveler != null)
            {
                leveler.EarnXP(leveler.GetNextLevelXP());
            }
        }
    }
}