
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Traits.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.com;


namespace GWLPXL.ARPGCore.CanvasUI.com
{

    /// <summary>
    /// an example unity UI interface for equipment generation
    /// </summary>
    public class EquipmentGenerator_UI : MonoBehaviour, IGenerateEquipment
    {
        [Header("Generator")]
        public EquipmentGenerateClass Generator;
        [Header("Misc")]
        public GameObject Player;
        [SerializeField]
        KeyCode toggleKey = KeyCode.F3;
        [Header("UI")]
        [SerializeField]
        GameObject mainPanel = null;
        [SerializeField]
        bool freezeDungeon = true;
        [SerializeField]
        GameObject displayResults = null;

        [Header("Equipment")]
        [SerializeField]
        TMP_Dropdown equipmentTemplateDropdown = null;
        [SerializeField]
        TMP_InputField equipmentNameInputField = null;

        [Header("Traits")]
        [SerializeField]
        TMP_Dropdown traitDropDown = null;
        [SerializeField]
        Transform nativePanel = null;
        [SerializeField]
        Transform randoPanel = null;
        [SerializeField]
        GameObject temporaryPrefab = null;

        IDisplayEquipmentGenerate display = null;
        List<GameObject> nativeObjs = new List<GameObject>();
        List<GameObject> randoObjs = new List<GameObject>();

        public EquipmentGenerateClass GenerateClass => Generator;


        #region initialization
        
       
        #endregion
        #region unity calls
        private void OnEnable()
        {
            Generator.Ini();
            Generator.OnEquipmentUpdated += DisplayResults;

            Generator.OnNativeTraitAdded += NativeTraitAdded;
            Generator.OnRandomTraitAdded += RandomTraitAdded;

            Generator.OnNativeCleared += ClearAllNativeObjects;
            Generator.OnRandomCleared += ClearAllRandoObjects;

            Generator.OnLastNativeTraitRemoved += TryRemoveLastNative;
            Generator.OnLastRandomTraitRemoved += TryRemoveLastRando;
        }
        private void OnDisable()
        {
            Generator.OnEquipmentUpdated -= DisplayResults;

            Generator.OnNativeTraitAdded -= NativeTraitAdded;
            Generator.OnRandomTraitAdded -= RandomTraitAdded;

            Generator.OnNativeCleared -= ClearAllNativeObjects;
            Generator.OnRandomCleared -= ClearAllRandoObjects;

            Generator.OnLastNativeTraitRemoved -= TryRemoveLastNative;
            Generator.OnLastRandomTraitRemoved -= TryRemoveLastRando;

            Generator.ClearAllNatives();
            Generator.ClearAllRandom();
            Generator.Disabled();
        }
        private void Start()
        {
            UISetup(Generator);
        }
        /// <summary>
        /// for testing at the moment
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                if (mainPanel.activeInHierarchy)
                {
                    DisableMaker();
                }
                else
                {
                    EnableMaker();
                }
            }
        }
        #endregion

        #region ui lifetime and subs
        /// <summary>
        /// generate the ui elements according to the generator
        /// </summary>
        /// <param name="generator"></param>
        private void UISetup(EquipmentGenerateClass generator)
        {
            equipmentTemplateDropdown.ClearOptions();
            equipmentTemplateDropdown.AddOptions(generator.ItemKeys);

            traitDropDown.ClearOptions();
            traitDropDown.AddOptions(generator.TraitKeys);

        }
        public void EnableMaker()
        {
            mainPanel.SetActive(true);
            PlayerHelper.DisableGameplayInputs();
            GenerateEquipmentHelper.FreezeDungeon(freezeDungeon, mainPanel);
            Generator.Enabled();
        }
        public void DisableMaker()
        {
            mainPanel.SetActive(false);
            PlayerHelper.EnableGameplayInputs();
            GenerateEquipmentHelper.FreezeDungeon(freezeDungeon, mainPanel);
            Generator.Disabled();
        }
        void ClearAllNativeObjects()
        {
            for (int i = 0; i < nativeObjs.Count; i++)
            {
                Destroy(nativeObjs[i]);
            }
            nativeObjs.Clear();
        }
        void ClearAllRandoObjects()
        {
            for (int i = 0; i < randoObjs.Count; i++)
            {
                Destroy(randoObjs[i]);
            }
            randoObjs.Clear();
        }
        void NativeTraitAdded(EquipmentTrait addedtrait)
        {

            GameObject obj = Instantiate(temporaryPrefab, nativePanel);
            IDisplayTrait dtrait = obj.GetComponent<IDisplayTrait>();
            dtrait.SetUI(this);
            dtrait.DisplayTrait(addedtrait, false);

            nativeObjs.Add(obj);
        }
        void RandomTraitAdded(EquipmentTrait addedtrait)
        {

            GameObject obj = Instantiate(temporaryPrefab, randoPanel);
            IDisplayTrait dtrait = obj.GetComponent<IDisplayTrait>();
            dtrait.SetUI(this);
            dtrait.DisplayTrait(addedtrait, false);

            randoObjs.Add(obj);
        }

        void TryRemoveLastNative()
        {
            if (nativeObjs.Count > 0)
            {
                Destroy(nativeObjs[nativeObjs.Count - 1]);
                nativeObjs.RemoveAt(nativeObjs.Count - 1);
            }
        }
        void TryRemoveLastRando()
        {
            if (randoObjs.Count > 0)
            {
                Destroy(randoObjs[randoObjs.Count - 1]);
                randoObjs.RemoveAt(randoObjs.Count - 1);
            }
        }
        void DisplayResults(Equipment vars)
        {
            if (display == null)
            {
                display = displayResults.GetComponent<IDisplayEquipmentGenerate>();
            }
            display.DisplayEquipment(vars);
        }
      
        
        #endregion

        #region unity button/dropdown/input calls
        public void ClearAllNative()
        {
            Generator.ClearAllNatives();

        }
        public void ClearAllRando()
        {
            Generator.ClearAllRandom();

        }
        public void RemoveLastNative()
        {
            Generator.RemoveLastNative();

        }
        public void RemoveLastRando()
        {
            Generator.RemoveLastRando();
        }
        public void AddAsNativeTrait()
        {
            Generator.AddNativeTrait();
        }
        public void AddAsRandomTrait()
        {
            Generator.AddRandomTrait();

        }
        public void SetSuffixes(string suffixes)
        {
            Generator.SetSuffixes(suffixes);
        }
        public void  SetPrefixes(string prefixes)
        {
            Generator.SetPrefixes(prefixes);
        }
        public void SetTraitTemplate(string template)
        {
            Generator.SetTraitTemplate(template);
        }
        public void SetILevel(string newLevel)
        {
            Generator.SetILevel(newLevel);

        }
        public void SetTraitCopy(int index)
        {
            Generator.SetTraitTemplate(index);
        }
        public void SetEquipmentCopy(int index)
        {
            Generator.SetEquipmentTemplate(index);
        }
        public void AddToInventory()
        {
            IInventoryUser inv = Player.GetComponent<IActorHub>().MyInventory;
            if (inv == null) return;
            inv.GetInventoryRuntime().AddItemToInventory(Generator.RunTimeGenerated);
        }
        public void ReRoll()
        {
            Generator.UpdateRuntime();
        }

        #endregion

        
       
    }
}
