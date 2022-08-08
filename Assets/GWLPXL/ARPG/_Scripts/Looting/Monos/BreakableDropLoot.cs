using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;
using GWLPXL.ARPGCore.Leveling.com;
namespace GWLPXL.ARPGCore.Looting.com
{
    [System.Serializable]
    public class DropLootVars
    {
        public LootDrops LootDrops = null;
        public Transform DropLocation = null;
        public float Delay = .25f;
        public bool UseScaling = true;
        public DropLootVars(LootDrops drops, Transform droplocation, float delay, bool usescaling)
        {
            LootDrops = drops;
            DropLocation = droplocation;
            Delay = delay;
            UseScaling = usescaling;
        }
    }

    [RequireComponent(typeof(Breakable))]
    public class BreakableDropLoot : MonoBehaviour, IDropLoot
    {
        [SerializeField]
        ActorDropLootEvents droplootEvents = new ActorDropLootEvents();
        [SerializeField]
        DropLootVars vars;

        bool dropped = false;
        ILootFX[] lootFX = new ILootFX[0];
        IScale scaling = null;

        public void SetVars(DropLootVars newVars)
        {
            vars = newVars;
        }
        private void Awake()
        {
            lootFX = GetComponents<ILootFX>();
            scaling = GetComponent<IScale>();
        }

        private void Start()
        {
            for (int i = 0; i < lootFX.Length; i++)
            {
                lootFX[i].EnableFX();
            }
        }
        public void DropLoot()
        {
            if (vars.LootDrops == null || vars.LootDrops.AllPossibleItems.Count == 0 || dropped) return;

            float multi = 1;

            if (vars.UseScaling)
            {
                multi = scaling.GetScaledLevel();
            }

            int level = Formulas.GetILevelMulti(Mathf.FloorToInt(scaling.GetUNScaledLevel() * multi));
            Item item = vars.LootDrops.GetRandomDrop(level);
            ILoot newLoot = LootHandler.DropLoot(item, vars.DropLocation.position, DungeonMaster.Instance.GetLootPrefab(), vars.Delay);
            dropped = true;
            for (int i = 0; i < lootFX.Length; i++)
            {
                lootFX[i].DisableFX();
            }

            droplootEvents.SceneEvents.OnLootDropped.Invoke(newLoot);
            //Formulas.GetILevel(GetInitialLevel()));
            //Vector3 randomPos = Random.insideUnitCircle * 5;
            //randomPos.y = this.transform.position.y;
            //loot.transform.position = randomPos;
        }

        public void SetLootDrop(LootDrops newDrops)
        {
            vars.LootDrops = newDrops;
        }
    }
}