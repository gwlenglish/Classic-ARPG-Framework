
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Looting.com;
using System.Collections;
using TMPro;
using UnityEngine;

namespace GWLPXL.ARPGCore.Demo.com
{

    public class LootOnClick : MonoBehaviour, IDropLoot, IInteract
    {
        public TextMeshPro Text;
        public LootDrops LootList;
        public Transform DropLocation;
        public Color ClickColor;
        public int AtLevel;
        GameObject previous;
        bool spawning;
        MeshRenderer rend;
        Color originl;
        private void Awake()
        {
            Text.text = "Level:" + "\n" + AtLevel.ToString();
            rend = GetComponent<MeshRenderer>();
            originl = rend.material.color;
        }
        public bool DoInteraction(GameObject interactor)
        {
            if (spawning) return false;
            spawning = true;
            rend.material.color = ClickColor;
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
            GameObject newLoot = LootList.GetRandomDrop(pos, AtLevel);
            ILoot loot = newLoot.GetComponent<ILoot>();
            loot.GetLootOptions().DelayBeforeActive = 0;
            previous = newLoot;
        }
      

        public bool IsInRange(GameObject interactor)
        {
            return true;
        }

        public void SetLootDrop(LootDrops newDrops)
        {
            LootList = newDrops;
        }
    }
}