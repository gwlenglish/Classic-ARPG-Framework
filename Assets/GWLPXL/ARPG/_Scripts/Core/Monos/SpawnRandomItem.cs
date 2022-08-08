using GWLPXL.ARPGCore.Looting.com;
using UnityEngine;


namespace GWLPXL.ARPGCore.com
{

    /// <summary>
    /// possibly revise into a Scriptable Object, easier to plug into events
    /// </summary>
    public class SpawnRandomItem : MonoBehaviour
    {
        public LootDrops LootList;
        public Transform DropLocation;
        public int AtLevel = 1;
        [Tooltip("If true, will use random range below. If false, will use level set above.")]
        public bool RandomLevel = true;
        public Vector2Int RandomLevelRange = new Vector2Int(1, 99);
        GameObject previous;

        // Start is called before the first frame update
        public void Spawn()
        {
            if (previous != null)
            {
                Destroy(previous.gameObject);
            }
            Vector3 pos = DropLocation.position;
            int level = AtLevel;
            if (RandomLevel)
            {
                level = Random.Range(RandomLevelRange.x, RandomLevelRange.y + 1);
            }
            GameObject newLoot = LootList.GetRandomDrop(pos, level);
            ILoot loot = newLoot.GetComponent<ILoot>();
            loot.GetLootOptions().DelayBeforeActive = 0;
            previous = newLoot;
        }
    }
}