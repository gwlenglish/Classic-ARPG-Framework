using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Leveling.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Demo.com
{

    /// <summary>
    /// level self
    /// </summary>
    public class LevelOnInteract : MonoBehaviour, IInteract
    {
        public bool DoInteraction(GameObject interactor)
        {
            ILevel leveler = interactor.GetComponent<ILevel>();
            if (leveler != null)
            {
                leveler.EarnXP(leveler.GetNextLevelXP());
                return true;
            }
            return false;
        }

        public bool IsInRange(GameObject interactor)
        {
            return true;
        }
    }
}
