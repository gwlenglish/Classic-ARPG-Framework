using System.Collections.Generic;
using UnityEngine;

using GWLPXL.ARPGCore.Traits.com;
using GWLPXL.ARPGCore.com;
/// <summary>
/// the equipment generate tracker, used with the equipmentgenerate class
/// </summary>

namespace GWLPXL.ARPGCore.Items.com
{
    [System.Serializable]
    public class EquipmentGenerate : IGenerate
    {
        public string Default { get; private set; }
        public Dictionary<string, Equipment> EquipmentDic = new Dictionary<string, Equipment>();

        public Dictionary<Equipment, Equipment> TemplateEmpty = new Dictionary<Equipment, Equipment>();
        public List<string> GetKeysToList() => keys;
        List<string> keys = new List<string>();
        ItemDatabase itemdb = null;
        public EquipmentGenerate(ItemDatabase _items)
        {
            if (_items == null)
            {
                Debug.LogWarning("Item database is null, can't generate items");
                EquipmentDic.Clear();
                return;
            }
            itemdb = _items;
            RefreshDictionary();
        }

        public void RefreshDictionary()
        {
            if (itemdb == null)
            {
                Debug.LogWarning("Database is empty, can't generate dictionary");
                return;
            }
            EquipmentDic.Clear();
            keys.Clear();
            for (int i = 0; i < itemdb.Slots.Length; i++)
            {
                if (itemdb.GetDatabaseObjectBySlotIndex(i) is Equipment)
                {
                    string itemname = itemdb.Slots[i].Item.GetBaseItemName();
                    EquipmentDic.TryGetValue(itemname, out Equipment value);
                    if (value == null)
                    {
                        value = itemdb.Slots[i].Item as Equipment;
                    }
                    EquipmentDic[itemname] = value;
                    keys.Add(itemname);
                    if (string.IsNullOrEmpty(Default))
                    {
                        Default = itemname;
                    }

                    Equipment emptyCopy = ScriptableObject.Instantiate(itemdb.GetDatabaseObjectBySlotIndex(i) as Equipment);
                    emptyCopy.GetStats().SetNativeTraits(new EquipmentTrait[0]);
                    TraitTier first = new TraitTier();
                    first.MaxNumberOfTraitsPerTier = 4;
                    first.ILevelMulti = 1;
                    TraitDrops newDrops = ScriptableObject.CreateInstance<TraitDrops>();
                    newDrops.PossibleTraits = new EquipmentTrait[0];
                    first.PossibleTierDrops = newDrops;
                    newDrops.CreateLootTable();
                    emptyCopy.SetTraitTiers(new TraitTier[1] { first });
                    TemplateEmpty.Add(itemdb.GetDatabaseObjectBySlotIndex(i) as Equipment, emptyCopy);
                }
            }
        }
    }
}
