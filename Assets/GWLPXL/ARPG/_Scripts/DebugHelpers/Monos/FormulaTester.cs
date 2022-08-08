#if UNITY_EDITOR
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Statics.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.DebugHelpers.com
{

    public class FormulaTester : MonoBehaviour
    {
        public Player player;
        public KeyCode PlayerAttackDmg;

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(PlayerAttackDmg))
            {
                //DungeonMaster.Instance.Debug = DebugMessages.Enabled;
                int dmg = CombatStats.GetTotalPlayerAttackDamage(player.MyStats, player.MyInventory, player.MyAbilities);
                print(dmg);
            }


        }

    }
}
#endif