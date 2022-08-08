
using GWLPXL.ARPGCore.Statics.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Items.com
{
    #region helpers
    [System.Serializable]
    public class EquipmentList
    {
        public string EditorDescription;
        public List<Equipment> Equipment;
        public void AddPiece(Equipment newPiece)
        {
            if (Equipment.Contains(newPiece) == false)
            {
                Equipment.Add(newPiece);
            }
        }
        public void RemovePiece(Equipment remove)
        {
            if (Equipment.Contains(remove))
            {
                Equipment.Remove(remove);
            }
        }
        public EquipmentList(List<Equipment> equipment)
        {
            Equipment = equipment;
        }
        public int GetBaseStatsAdded()
        {
            int baseStat = 0;
            for (int i = 0; i < Equipment.Count; i++)
            {
                float baseint = Equipment[i].GetStats().GetBaseStat() * Formulas.Hundred;
                int converted = Mathf.RoundToInt(baseint);
                baseStat += converted;
            }
            return baseStat;
        }
    }
    #endregion
}