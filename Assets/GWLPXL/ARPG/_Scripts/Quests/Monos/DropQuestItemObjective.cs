
using UnityEngine;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Looting.com;
using GWLPXL.ARPGCore.com;
namespace GWLPXL.ARPGCore.Quests.com
{

    /// <summary>
    /// Guarenteed drop of Quest item.
    /// delete this
    /// </summary>
    public class DropQuestItemObjective : MonoBehaviour, IDropLoot
    {
        [SerializeField]
        protected UnityLootDropEvents lootEvents = new UnityLootDropEvents();
        [SerializeField]
        protected QuestItem questItem = null;
        [SerializeField]
        protected GameObject LootPrefabOverride = null;
        public virtual void DropLoot()
        {

            ILoot newLoot = LootHandler.DropLoot(questItem, transform.position, LootPrefabOverride);
            if (lootEvents != null)
            {
                lootEvents.OnLootDropped.Invoke(newLoot);
            }
        }

        public virtual void SetLootDrop(LootDrops newDrops)
        {
            return;//nothin ghere, this is just exactly a quest item
        }
    }
}