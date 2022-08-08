
using UnityEngine;
using GWLPXL.ARPGCore.Leveling.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Looting.com
{


    public class SearchableDropLoot : MonoBehaviour, IDropLoot
    {
        [SerializeField]
        DropLootVars vars;

        bool dropped = false;
        IScale scaler = null;

        public void SetVars(DropLootVars newVars) => vars = newVars;
        private void Awake()
        {
            scaler = GetComponent<IScale>();
        }
        public void DropLoot()
        {
            if (dropped) return;
            float multi = 1;
            int level = 1;
            if (vars.UseScaling)
            {
                multi = scaler.GetScaledLevel();
            }
            Item item = vars.LootDrops.GetRandomDrop(Mathf.FloorToInt(level * multi));
            ILoot newLoot = LootHandler.DropLoot(item, vars.DropLocation.position, DungeonMaster.Instance.GetLootPrefab(), vars.Delay);
            dropped = true;

            
        }

        public void SetLootDrop(LootDrops newDrops) => vars.LootDrops = newDrops;
     

       
    }
}