

using GWLPXL.ARPGCore.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Util.com
{
    [System.Serializable]
    public class ModifyDungeonVariables
    {
        [SerializeField]
        float increaseModLevelMulti = 0;
        [SerializeField]
        float increaseMobExperienceMultiplier = 0;
        [SerializeField]
        float increaseItemLevelMultipler = 0;
        public void ModifyDungeonVars(float moblevel, float xpmmulti, float itemmulti)
        {
            DungeonVariables variables = DungeonMaster.Instance.Variables;
            variables.MobLevelMultiplier += moblevel;
            variables.ItemLevelMultiplier += itemmulti;
            variables.MobXPMultiplier += xpmmulti;
        }
    }

    public class OnTriggerRepeatDungeon : MonoBehaviour
    {
        [SerializeField]
        float increaseModLevelMulti = 0;
        [SerializeField]
        float increaseMobExperienceMultiplier = 0;
        [SerializeField]
        float increaseItemLevelMultipler = 0;

        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponentInParent<Player>();
            Debug.Log(other.name);
            if (player != null)
            {
                IncreaseDungeonVariables();
                DungeonMaster.Instance.ReloadScene();
            }
        }

        private void IncreaseDungeonVariables()
        {
            DungeonVariables variables = DungeonMaster.Instance.Variables;
            variables.MobLevelMultiplier += increaseModLevelMulti;
            variables.ItemLevelMultiplier += increaseItemLevelMultipler;
            variables.MobXPMultiplier += increaseMobExperienceMultiplier;
        }
    }
}