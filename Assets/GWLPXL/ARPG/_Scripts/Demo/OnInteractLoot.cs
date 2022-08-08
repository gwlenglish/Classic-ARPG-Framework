using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Looting.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.Demo.com
{


    public class OnInteractLoot : MonoBehaviour, IDropLoot, IInteract
    {

        public LootDrops LootList = null;
        public Transform DropLocation = null;
        public float InteractDistance = 2;
        public int AtLevel = 1;
        [Tooltip("Random will override the AtLevel parameter")]
        public bool RandomLevel = true;
        public Vector2Int RandomLevelRange = new Vector2Int(1, 99);
        GameObject previous = null;


        public bool DoInteraction(GameObject interactor)
        {
            bool performed = false;

            DropLoot();
            performed = true;
            return performed;
        }

        public void DropLoot()
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


        public bool IsInRange(GameObject interactor)
        {
            Vector3 dir = interactor.transform.position - this.transform.position;
            float sqrdst = dir.sqrMagnitude;
            if (sqrdst <= InteractDistance * InteractDistance)
            {
                return true;
            }
            return false;
        }

        public void SetLootDrop(LootDrops newDrops)
        {
            LootList = newDrops;
        }
    }
}