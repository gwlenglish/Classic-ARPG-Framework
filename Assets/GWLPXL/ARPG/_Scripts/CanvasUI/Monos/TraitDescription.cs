using GWLPXL.ARPGCore.Traits.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GWLPXL.ARPGCore.Items.com;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public interface IDisplayTrait
    {
        void SetUI(IGenerateEquipment _ui);
        void DisplayTrait(EquipmentTrait _trait, bool isRandom);
       
    }


    public class TraitDescription : MonoBehaviour, IDisplayTrait
    {
        public System.Action<EquipmentGenerateClass, EquipmentTrait, int> OnTraitWeightUpdate;
        [SerializeField]
        TextMeshProUGUI description = null;
        [SerializeField]
        TextMeshProUGUI percent = null;
        [SerializeField]
        TMP_InputField inputField = null;
    
        IGenerateEquipment ui = null;
        EquipmentTrait trait;
        public void DisplayTrait(EquipmentTrait _trait, bool isRandom)
        {
            description.SetText(_trait.GetTraitUIDescription());
            inputField.gameObject.SetActive(isRandom);
            percent.gameObject.transform.parent.gameObject.SetActive(isRandom);
            if (isRandom)
            {
                inputField.text = _trait.GetWeight().ToString();
            }
          

            trait = _trait;
        }

        public void ModifyWeight(string newWeight)
        {
            int.TryParse(newWeight, out int result);
            OnTraitWeightUpdate?.Invoke(ui.GenerateClass, trait, result);
            

        }

        public void OnTableChanged(TraitDrops drops)
        {
            for (int i = 0; i < drops.TraitTable.Count; i++)
            {
                if (drops.TraitTable[i].Trait == trait)
                {
                    SetPercent(drops.TraitTable[i].RelativePercent);
                    break;
                }
            }
            
        }
        void SetPercent(float newPercent)
        {
            percent.SetText(newPercent.ToString());
        }

        public void SetUI(IGenerateEquipment _ui) => ui = _ui;
       
    }
}