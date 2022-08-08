using GWLPXL.ARPGCore.com;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// used with equipmentgenerateclass to track generated traits
/// </summary>

namespace GWLPXL.ARPGCore.Traits.com
{
    [System.Serializable]
    public class TraitGenerate : IGenerate
    {
        public string Default;
        public Dictionary<string, EquipmentTrait> Traitsdic = new Dictionary<string, EquipmentTrait>();
        public Dictionary<EquipmentTrait, EquipmentTrait> TemplateEmpty = new Dictionary<EquipmentTrait, EquipmentTrait>();
        public List<string> GetKeysToList() => keys;
        List<string> keys = new List<string>();
        EquipmentTraitDatabase traitdb = null;
        public TraitGenerate(EquipmentTraitDatabase _items)
        {
            if (_items == null)
            {
                Traitsdic.Clear();
                return;
            }
            traitdb = _items;
            RefreshDictionary();
        }

        public void RefreshDictionary()
        {
            if (traitdb == null)
            {
                Debug.LogWarning("Database is empty, can't generate dictionary");
                return;
            }
            Traitsdic.Clear();
            keys.Clear();
            for (int i = 0; i < traitdb.Slots.Length; i++)
            {
                if (traitdb.GetDatabaseObjectBySlotIndex(i) is EquipmentTrait)
                {
                    string itemname = traitdb.Slots[i].Trait.GetTraitName();
                    Traitsdic.TryGetValue(itemname, out EquipmentTrait value);
                    if (value == null)
                    {
                        value = traitdb.Slots[i].Trait;
                    }
                    Traitsdic[itemname] = value;
                    keys.Add(itemname);

                    if (string.IsNullOrEmpty(Default))
                    {
                        Default = itemname;
                    }

                    EquipmentTrait emptyCopy = ScriptableObject.Instantiate(traitdb.GetDatabaseObjectBySlotIndex(i) as EquipmentTrait);

                    TemplateEmpty.Add(traitdb.GetDatabaseObjectBySlotIndex(i) as EquipmentTrait, emptyCopy);
                }
            }
        }
    }
}
