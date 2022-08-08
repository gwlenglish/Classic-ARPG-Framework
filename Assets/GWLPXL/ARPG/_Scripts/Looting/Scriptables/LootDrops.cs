
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Statics.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Looting.com
{

   
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Loot/NEW_LootDropList")]

    ///to do, refactor the loot table to able to push button in editor and only do once since these are shared uniques
    public class LootDrops : ScriptableObject, ISaveJsonConfig
    {
      
        [SerializeField]
        TextAsset config = null;
        [SerializeField]
        [Tooltip("Will override the lootprefab set in the dungeon master. Can remain null if one is set in the dungeon master.")]
        GameObject lootPrefabOverride = null;
        public LootID ID = new LootID("NULL", 0, null);
        public List<Item> AllPossibleItems = new List<Item>();
        public List<LootRange> LootTable = new List<LootRange>();
        int currentProbabilityWeightMaximum = 0;

        public LootID GetID() => ID;
        

       
        protected virtual void OnEnable()
        {
            CreateLootTable();


        }
        
        /// <param name="ofLevel"></param>
        /// <returns></returns>
        public virtual Item GetRandomDrop(int ofLevel)
        {
            Item item = DetermineRarity();
            if (item is Equipment)
            {
                //ARPGDebugger.DebugMessage("is equipment");
                Equipment equipment = item as Equipment;
                equipment.AssignEquipmentTraits(ofLevel);


            }
            return item;
        }
        public virtual GameObject GetRandomDrop(Vector3 atLocation, int ofLevel)
        {
            GameObject prefab = null;

            if (lootPrefabOverride != null)
            {
                prefab = lootPrefabOverride;
            }
            else
            {
                prefab = DungeonMaster.Instance.GetLootPrefab();
            }
            GameObject newDrop = Instantiate(prefab, atLocation, Quaternion.identity) as GameObject;
            Item item = DetermineRarity();

            if (item is Equipment)
            {
                //ARPGDebugger.DebugMessage("is equipment");
                Equipment equipment = item as Equipment;
                equipment.AssignEquipmentTraits(ofLevel);


            }
            newDrop.GetComponent<ILoot>().GetLootOptions().DroppedItem = item as Item;//determines rarity, then grabs a random from that rarity list
            return newDrop;
        }


        protected virtual Item DetermineRarity()
        {
            Item item = RandomRoll();
            Item itemCopy = Instantiate(item);
            if (item is QuestItem)//grab the original, dont make a copy.
            {
                itemCopy = item;
            }
            return itemCopy;
        }
        //use this for the item trait drops
        protected virtual Item RandomRoll()
        {
            Item dropped = null;
            int roll = Random.Range(0, currentProbabilityWeightMaximum);
            for (int i = 0; i < LootTable.Count; i++)
            {
                if (roll > LootTable[i].RangeFrom && roll <= LootTable[i].RangeTo)
                {
                    dropped = LootTable[i].Item;
                    return dropped;
                }
            }

            //should never make it here, but just in case
            int maxCount = 100;
            while (dropped == null)
            {
                roll = Random.Range(0, currentProbabilityWeightMaximum);
                for (int i = 0; i < LootTable.Count; i++)
                {
                    if (roll >= LootTable[i].RangeFrom && roll < LootTable[i].RangeTo)
                    {
                        dropped = LootTable[i].Item;
                        return dropped;
                    }
                    if (i > maxCount)
                    {
                        break;
                    }
                }
            }

            ARPGDebugger.DebugMessage("Did not find valid loot on the loot table " + this.name, this);
            return dropped;
        }


        public virtual void CreateLootTable()
        {
            if (AllPossibleItems != null && AllPossibleItems.Count > 0)
            {
                currentProbabilityWeightMaximum = 0;
                LootTable.Clear();
                // Sets the weight ranges of the selected items.
                for (int i = 0; i < AllPossibleItems.Count; i++)
                {
                    Item item = AllPossibleItems[i];
                    if (item == null)
                    {
                        ARPGDebugger.DebugMessage("Item is NULL on loot table " + this.name + " at entry " + i.ToString(), this);
                        continue;
                    }
                    else if (item.GetRarity() == null)
                    {
                        ARPGDebugger.DebugMessage("Item Rary is NULL on ITEM " + item.GetGeneratedItemName(), item);
                        continue;

                    }
                    int rangeFrom = currentProbabilityWeightMaximum;
                    currentProbabilityWeightMaximum += item.GetRarity().GetWeight();
                    int rangeTo = currentProbabilityWeightMaximum;
                    LootRange lootrange = new LootRange(rangeFrom, rangeTo, item);
                    LootTable.Add(lootrange);
                }

                for (int i = 0; i < LootTable.Count; i++)
                {
                    float percent = (float)(LootTable[i].Item.GetRarity().GetWeight() / (float)currentProbabilityWeightMaximum) * (float)Formulas.Hundred;
                    LootTable[i].RelativePercent = percent;
                    LootTable[i].Description = LootTable[i].Item.GetGeneratedItemName() + "_" + percent.ToString();
                }

                LootTable.Sort((p1, p2) => p1.RelativePercent.CompareTo(p2.RelativePercent));
            }
        }
        public virtual void AddItemToDropList(Item newitem)
        {
            if (AllPossibleItems.Contains(newitem) == false)
            {
                AllPossibleItems.Add(newitem);
            }
        }

        public void SetTextAsset(TextAsset textAsset)
        {
            config = textAsset;
        }

        public TextAsset GetTextAsset()
        {
            return config;
        }

        public Object GetObject()
        {
            return this;
        }




    }

    #region helper classes

    [System.Serializable]
    public class LootRange
    {
        public string Description;
        [Tooltip("For the designer to see % chance to drop.")]
        public float RelativePercent;
        public int RangeFrom;
        public int RangeTo;
        public Item Item;
        public LootRange(int rangeFrom, int rangeTo, Item _item)
        {
            RangeFrom = rangeFrom;
            RangeTo = rangeTo;
            Item = _item;
        }
    }
    #endregion
}