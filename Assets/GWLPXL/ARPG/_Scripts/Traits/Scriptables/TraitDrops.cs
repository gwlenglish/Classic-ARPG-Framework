

using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.DebugHelpers.com;

namespace GWLPXL.ARPGCore.Traits.com
{
  
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Equipment/Traits/NEW_TraitDropList")]
    //add the random weight system here, much like the items. Do the random thing as a static? Seems like we might use it elsewhere. 
    public class TraitDrops : ScriptableObject
    {
        public EquipmentTrait[] PossibleTraits = new EquipmentTrait[0];
        public List<TraitRange> TraitTable = new List<TraitRange>();
        int currentProbabilityWeightMaximum = 0;

        private void OnEnable()
        {
            CreateLootTable();
        }
        public void CreateLootTable()
        {
            if (PossibleTraits != null && PossibleTraits.Length > 0)
            {
                currentProbabilityWeightMaximum = 0;
                TraitTable.Clear();
                // Sets the weight ranges of the selected items.
                for (int i = 0; i < PossibleTraits.Length; i++)
                {
                    EquipmentTrait trait = PossibleTraits[i];
                    int rangeFrom = currentProbabilityWeightMaximum;
                    if (trait == null)
                    {
                        Debug.LogWarning("Trait is null in traitdrop " + this.name.ToUpper() + " at index " + i, this);
                        continue;
                    }
                    currentProbabilityWeightMaximum += trait.GetWeight();
                    int rangeTo = currentProbabilityWeightMaximum;
                    TraitRange lootrange = new TraitRange(rangeFrom, rangeTo, trait);
                    TraitTable.Add(lootrange);
                }

                for (int i = 0; i < TraitTable.Count; i++)
                {
                    float percent = (float)(TraitTable[i].Trait.GetWeight() / (float)currentProbabilityWeightMaximum) * (float)Formulas.Hundred;
                    TraitTable[i].RelativePercent = percent;
                    TraitTable[i].Description = TraitTable[i].Trait.GetTraitName() + "_" + percent.ToString();
                }

                TraitTable.Sort((p1, p2) => p1.RelativePercent.CompareTo(p2.RelativePercent));
            }
            else
            {
                TraitTable = new List<TraitRange>();
                currentProbabilityWeightMaximum = 0;
            }
        }
        public EquipmentTrait RandomRoll()
        {
            EquipmentTrait dropped = null;
            int roll = Random.Range(0, currentProbabilityWeightMaximum);
            for (int i = 0; i < TraitTable.Count; i++)
            {
                if (roll > TraitTable[i].RangeFrom && roll <= TraitTable[i].RangeTo)
                {
                    dropped = TraitTable[i].Trait;
                    return dropped;
                }
            }
            int maxCount = 100;
            while (dropped == null)
            {
                roll = Random.Range(0, currentProbabilityWeightMaximum);
                for (int i = 0; i < TraitTable.Count; i++)
                {
                    if (roll > TraitTable[i].RangeFrom && roll <= TraitTable[i].RangeTo)
                    {
                        dropped = TraitTable[i].Trait;
                        return dropped;
                    }
                    if (i > maxCount)
                    {
                        break;
                    }
                }
            }

            ARPGDebugger.DebugMessage("Did not find valid trait on the trait table " + this.name, this);
            return dropped;
        }
    }

    [System.Serializable]
    public class TraitRange
    {
        public string Description;
        [Tooltip("For the designer to see % chance to drop.")]
        public float RelativePercent;
        public int RangeFrom;
        public int RangeTo;
        public EquipmentTrait Trait;
        public TraitRange(int rangeFrom, int rangeTo, EquipmentTrait _trait)
        {
            RangeFrom = rangeFrom;
            RangeTo = rangeTo;
            Trait = _trait;
        }
    }
}