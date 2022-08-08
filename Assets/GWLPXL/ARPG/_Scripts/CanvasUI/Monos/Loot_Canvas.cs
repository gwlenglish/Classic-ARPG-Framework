using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Looting.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
   
    public class Loot_Canvas : MonoBehaviour, ILootCanvas, ITick
    {
        [SerializeField]
        GameObject MainPanel = null;
        [Header("Loot UI")]
        public Transform LootTextPanel = null;
        public float DistanceToShowLoot = 10;
        public bool UseDeltaTimeTickRate = true;
        public float TickRate = .02f;
        Dictionary<ILoot, GameObject> lootTextDic = new Dictionary<ILoot, GameObject>();

        Camera main = null;
        Transform player;//works for one player
        private void Awake()
        {
            
            main = Camera.main;
        }

 

        private void Start()
        {
            AddTicker();
        }

        private void OnDestroy()
        {
            RemoveTicker();
        }
        /// <summary>
        /// it's not buggy.
        /// </summary>
        public void LootObjectsControl()
        {
            if (DungeonMaster.Instance.GetAllSceneReferences().Length == 0) return;
            if (player == null)
            {
                player = DungeonMaster.Instance.GetAllSceneReferences()[0].SceneRef.transform;//first scene ref
            }

            ILoot loot = null;
            GameObject textObj = null;
            Vector3 distance = Vector3.zero;
            float sqrdMag = 0;
            Vector3 worldPos = Vector3.zero;
            Vector3 newPos = Vector3.zero;

           
            foreach (var kvp in lootTextDic)
            {
                loot = kvp.Key;
                textObj = kvp.Value;
                distance = player.transform.position - loot.GetInstance().position;//null...
                sqrdMag = distance.sqrMagnitude;

                //if we are too far away from the player, turn off the loot text
                if (sqrdMag > (DistanceToShowLoot * DistanceToShowLoot))
                {
                    textObj.SetActive(false);

                }
                else
                {
                    //if we are within range, turn it on and update the screen position
                    textObj.SetActive(true);
                    worldPos = loot.GetInstance().position;
                    newPos = main.WorldToScreenPoint(worldPos);
                    textObj.transform.position = newPos;
                }

            }
        }






        public void CreateLootTextUI(ILoot key)
        {
            //rarity so holds the color, font, and text prefab
            string lootName = key.GetLootOptions().DroppedItem.GetGeneratedItemName();
            Rarity itemRarity = key.GetLootOptions().DroppedItem.GetRarity();

            ItemRarityType rarity = itemRarity.GetItemRarity();
            Color color = itemRarity.GetRarityColor();
            TMP_FontAsset font = itemRarity.GetTMFont();
            GameObject prefab = itemRarity.GetLootTextPrefab();
            GameObject newObj = Instantiate(prefab, LootTextPanel);
            ILootText tmText = newObj.GetComponent<ILootText>();
            tmText.SetText(lootName);
            tmText.SetColor(color);
            tmText.SetFont(font);

            Vector3 newPos = main.WorldToScreenPoint(key.GetInstance().position);
            newObj.transform.position = newPos;
            lootTextDic.Add(key, newObj);
        }


        public void RemoveLootText(ILoot key)
        {
            if (lootTextDic.ContainsKey(key))
            {
                GameObject value = lootTextDic[key];
                Destroy(value);
                lootTextDic.Remove(key);
            }
           
        }

        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            LootObjectsControl();
        }

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);

        }

        public float GetTickDuration()
        {
            if (UseDeltaTimeTickRate) return Time.deltaTime;
            return TickRate;
        }

        public bool IsActive() => MainPanel.activeInHierarchy;
        
    }
}