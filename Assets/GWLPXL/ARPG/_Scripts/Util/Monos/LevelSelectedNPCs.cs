using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Demo.com
{


    public class LevelSelectedNPCs : MonoBehaviour, IInteract
    {
        public EnemyAttributes[] ThingsToLevel;
        public bool DoInteraction(GameObject interactor)
        {
            for (int i = 0; i < ThingsToLevel.Length; i++)
            {
                int nextLevel = ThingsToLevel[i].GetRuntimeAttributes().MyLevel + 1;
                ThingsToLevel[i].GetRuntimeAttributes().LevelUp(nextLevel);
            }
            return true;

        }

        public bool IsInRange(GameObject interactor)
        {
            return true;
        }


    }

}