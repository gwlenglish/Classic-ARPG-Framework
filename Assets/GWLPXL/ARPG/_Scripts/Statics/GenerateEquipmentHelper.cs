using GWLPXL.ARPGCore.CanvasUI.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Traits.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Statics.com
{


    public static class GenerateEquipmentHelper 
    {

        public static System.Action<EquipmentGenerateClass> OnResultsUpdated;

        public static System.Action<EquipmentGenerateClass, EquipmentTrait> OnNativeTraitAdded;
        public static System.Action<EquipmentGenerateClass, EquipmentTrait> OnLastRandoTraitRemoved;
        public static System.Action<EquipmentGenerateClass, EquipmentTrait> OnLastNativeTraitRemoved;
        public static System.Action<EquipmentGenerateClass, EquipmentTrait> OnRandomTraitAdded;

        public static System.Action<EquipmentGenerateClass> OnClearAllRando;
        public static System.Action<EquipmentGenerateClass> OnClearAllNative;

        public static System.Action<EquipmentGenerateClass, TraitDrops> OnTableUpdate;

        public static void FreezeDungeon(bool shouldFreeze, GameObject mainPanel)
        {
            if (shouldFreeze && mainPanel.activeInHierarchy)
            {
                DungeonMaster.Instance.GetDungeonUISceneRef().SetDungeonState(0);
            }
            else if (shouldFreeze && !mainPanel.activeInHierarchy)
            {
                DungeonMaster.Instance.GetDungeonUISceneRef().SetDungeonState(1);

            }
        }

       

        static void TraitWeightUpdate(EquipmentGenerateClass vars, EquipmentTrait trait, int newwight)
        {
            bool found = false;
            for (int i = 0; i < vars.RandomtraitTemplates.Count; i++)
            {
                if (trait == vars.RandomtraitTemplates[i])
                {
                    vars.RandomtraitTemplates[i].SetWeight(newwight);
                    found = true;
                    Debug.Log("Found");
                    break;
                }
            }
            if (found)
            {
                UpdateTable(vars);
            }
        }

       



        public static void SetTemplate(EquipmentGenerateClass vars, string template)
        {
            vars.SetEquipmentTemplate(template);
        }
        public static void SetILevel(EquipmentGenerateClass vars, string newLevel)
        {
            vars.SetILevel(newLevel);

        }
       
        public static void RemoveLastRando(EquipmentGenerateClass vars)
        {
            vars.RemoveLastRando();

        }
        public static void RemoveLastNative(EquipmentGenerateClass vars)
        {
            vars.RemoveLastNative();

        }
        public static void ClearAllRando(EquipmentGenerateClass vars)
        {
            vars.ClearAllRandom();


        }
        public static void ClearAllNative(EquipmentGenerateClass vars)
        {
            vars.ClearAllNatives();

        }

        public static void AddAsRandomTrait(EquipmentGenerateClass vars)
        {
            vars.AddRandomTrait();


        }
        public static void AddAsNativeTrait(EquipmentGenerateClass vars)
        {
            vars.AddNativeTrait();

        }

        public static void UpdateResults(EquipmentGenerateClass vars)
        {
            vars.UpdateRuntime();

            OnResultsUpdated?.Invoke(vars);

           
        }

        private static void UpdateTable(EquipmentGenerateClass vars)
        {
            if (vars.RunTimeGenerated == null)
            {
                Debug.LogWarning("Trying to make a trait table but no equipment selected");
                return;
            }
            for (int i = 0; i < vars.RunTimeGenerated.GetTraitTier().Length; i++)
            {
                vars.RunTimeGenerated.GetTraitTier()[i].PossibleTierDrops.CreateLootTable();
                OnTableUpdate?.Invoke(vars, vars.RunTimeGenerated.GetTraitTier()[i].PossibleTierDrops);
            }

          

            
        }
    }
}