using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Looting.com;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GWLPXL.ARPGCore.Demo.com
{
    [System.Serializable]
    public class DropLocation
    {
        public Transform Location;
        public int Level;
    }

    public class LootMultipleOnClick : MonoBehaviour, IInteract, IDropLoot
    {
        public TextMeshPro Text;
        public LootDrops LootList;
        public Color ClickColor;
        public DropLocation[] Drops = new DropLocation[0];
        List<GameObject> previous = new List<GameObject>();
        bool spawning;
        MeshRenderer rend;
        Color originl;
        private void Awake()
        {
            //Text.text = AtLevels.ToString();
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
            if (previous.Count > 0)
            {
                for (int i = 0; i < previous.Count; i++)
                {
                    Destroy(previous[i]);
                }
                previous.Clear();
            }

            for (int i = 0; i < Drops.Length; i++)
            {
                Vector3 pos = Drops[i].Location.position;
                GameObject newLoot = LootList.GetRandomDrop(pos, Drops[i].Level);
                previous.Add(newLoot);
            }
            StartCoroutine(WaitToClick(1f));

        }
        IEnumerator WaitToClick(float duration)
        {
            yield return new WaitForSeconds(duration);
            spawning = false;
            rend.material.color = originl;
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