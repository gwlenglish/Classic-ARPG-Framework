
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Leveling.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Looting.com
{
    public class EnemyDrop : MonoBehaviour, IDropLoot
    {
        [SerializeField]
        protected EnemyDropLootEvents dropLootEvents = new EnemyDropLootEvents();
        [SerializeField]
        protected LootDrops possibleDrops;
        [SerializeField]
        protected float delay = .25f;
        protected IScale scale = null;

        protected virtual void Awake()
        {
           scale  = GetComponent<IScale>();
        }
        public virtual void DropLoot()
        {
            if (possibleDrops == null || possibleDrops.AllPossibleItems.Count == 0) return;

            int ofLevel = 1;
            if (scale != null)
            {
                ofLevel = scale.GetScaledLevel();
            }
            int level = Formulas.GetILevelMulti(ofLevel);
            Item item = possibleDrops.GetRandomDrop(level);
            ILoot newLoot = LootHandler.DropLoot(item, transform.position, DungeonMaster.Instance.GetLootPrefab(), delay, Random.insideUnitSphere);
            if (dropLootEvents != null)
            {
                dropLootEvents.SceneEvents.OnLootDropped.Invoke(newLoot);
            }
        }

        public virtual void SetLootDrop(LootDrops newDrops)
        {
            possibleDrops = newDrops;
        }
    }
}