using System.Collections.Generic;
using UnityEngine;

using GWLPXL.ARPGCore.Traits.com;
/// <summary>
/// main class that handles runtime generation of equipment with traits
/// </summary>

namespace GWLPXL.ARPGCore.Items.com
{
    [System.Serializable]
    public class EquipmentGenerateClass
    {
        public System.Action<Equipment> OnEquipmentUpdated;
        //natives
        public System.Action<EquipmentTrait> OnNativeTraitAdded;
        public System.Action OnLastNativeTraitRemoved;
        public System.Action OnNativeCleared;
        //randos
        public System.Action<EquipmentTrait> OnRandomTraitAdded;
        public System.Action OnLastRandomTraitRemoved;
        public System.Action OnRandomCleared;

        //[HideInInspector]
        [Tooltip("Generated at runtime, for viewing purposes only")]
        public Equipment RunTimeGenerated = null;
        [HideInInspector]
        public string PrefixString = string.Empty;
        [HideInInspector]
        public string SuffixString = string.Empty;

      
        public EquipmentGenerate EquipmentGen => equipmentGen;
        public Equipment CurrentTemplate => currentTemplate;
        public List<EquipmentTrait> NativetraitTemplates => nativetraitTemplates;
        public List<EquipmentTrait> RandomtraitTemplates => randomtraitTemplates;
        public int ILevel { get { return ilevel; } set { ilevel = value; } }
        public List<string> ItemKeys => itemKeys;

        public TraitGenerate TraitGen => traitGen;
        public List<string> TraitKeys => traitKeys;
        public EquipmentTrait TemplateTrait { get { return templateTrait; } set { templateTrait = value; } }
        
        [Header("Equipment")]
        [SerializeField]
        [Tooltip("The item database to use for generation")]
        ItemDatabase items = null;
        [SerializeField]
        EquipmentGenerate equipmentGen = null;
        List<string> itemKeys = new List<string>();

        [Header("Traits")]
        [SerializeField]
        [Tooltip("The trait database to use for generation")]
        EquipmentTraitDatabase traits = null;
        TraitGenerate traitGen = null;
        List<string> traitKeys = new List<string>();

        int ilevel = 1;
        Equipment currentTemplate = null;
        EquipmentTrait templateTrait = null;
        List<EquipmentTrait> nativetraitTemplates = new List<EquipmentTrait>();//copies
        List<EquipmentTrait> randomtraitTemplates = new List<EquipmentTrait>();//copies
        /// <summary>
        /// call to initialize the generator
        /// </summary>
        public void Ini()
        {
            SetItemDatabase(items);
            SetTraitDatabase(traits);
        }
        /// <summary>
        /// sets the trait database to use for generation
        /// </summary>
        /// <param name="db"></param>
        public void SetTraitDatabase(EquipmentTraitDatabase db)
        {
            if (db == null)
            {
                Debug.LogWarning("Trait Database is null, can't generate traits");
                return;
            }
            traitGen = new TraitGenerate(db);
            traitKeys = traitGen.GetKeysToList();
            SetTraitTemplate(0);
            
        }
        /// <summary>
        /// sets the item database to use for generation
        /// </summary>
        /// <param name="db"></param>
        public void SetItemDatabase(ItemDatabase db)
        {
            if (db == null)
            {
                Debug.LogWarning("Item Database is null, can't generate traits");
                return;
            }
            equipmentGen = new EquipmentGenerate(db);
            itemKeys = equipmentGen.GetKeysToList();
            SetEquipmentTemplate(0);
        }
      /// <summary>
      /// used to set and consume prefix/suffixes
      /// </summary>
      /// <returns></returns>
        EquipmentTrait SetupCopy()
        {

            EquipmentTrait copy = ScriptableObject.Instantiate(TraitGen.TemplateEmpty[TemplateTrait]);
            copy.name = copy.GetTraitName() + " runtime copy";
            if (string.IsNullOrEmpty(PrefixString) == false)
            {
                string[] prefixsplit = PrefixString.Split(',');
                copy.SetPrefixes(prefixsplit);
            }
            if (string.IsNullOrEmpty(SuffixString) == false)
            {
                string[] suffixsplit = SuffixString.Split(',');
                copy.SetSuffixes(suffixsplit);
            }

            //consume them
            SetPrefixes(string.Empty);
            SetSuffixes(string.Empty);

            //any other values that we need to transfer

            return copy;

        }
        /// <summary>
        /// clears all generated native traits
        /// </summary>
        public void ClearAllNatives()
        {
            nativetraitTemplates.Clear();
            if (RunTimeGenerated != null)
            {
                RunTimeGenerated.GetStats().SetNativeTraits(nativetraitTemplates.ToArray());
            }
            UpdateRuntime();
            OnNativeCleared?.Invoke();
        }
        /// <summary>
        /// sets the equipment template to use for generation.
        /// </summary>
        /// <param name="index"></param>
        public void SetEquipmentTemplate(int index)
        {
            if (index >= 0 && index < itemKeys.Count - 1)
            {
                SetEquipmentTemplate(itemKeys[index]);
       
            }
            else
            {
                SetTraitTemplate(string.Empty);
            }
           
        }
        /// <summary>
        /// disabled the generator
        /// </summary>
        public void Disabled()
        {
            ClearRuntime();
        }
        /// <summary>
        /// enables the generator and sets the defaults
        /// </summary>
        public void Enabled()
        {
            SetEquipmentTemplate(equipmentGen.Default);
            SetTraitTemplate(traitGen.Default);
        }
        /// <summary>
        /// sets equipment template for generation
        /// </summary>
        /// <param name="template"></param>
        public void SetEquipmentTemplate(string template)
        {
            if (string.IsNullOrEmpty(template))
            {
                RunTimeGenerated = null;
                return;
            }
            if (equipmentGen.EquipmentDic.ContainsKey(template) == false)
            {
                Debug.LogWarning("template " + template + " is not found in the dictionary.");
                return;
            }
            currentTemplate = equipmentGen.EquipmentDic[template];
            RunTimeGenerated = ScriptableObject.Instantiate(equipmentGen.EquipmentDic[template]);
            UpdateRuntime();

        }
        /// <summary>
        /// sets trait template for generation
        /// </summary>
        /// <param name="index"></param>
        public void SetTraitTemplate(int index)
        {
            if (index >= 0 && index < traitKeys.Count - 1)
            {
                SetTraitTemplate(traitKeys[index]);
       

            }
            else
            {
                SetTraitTemplate(string.Empty);
            }
      
        }
        /// <summary>
        /// sets trait template for generation
        /// </summary>
        /// <param name="template"></param>
        public void SetTraitTemplate(string template)
        {
            if (string.IsNullOrEmpty(template))
            {
                templateTrait = null;
                return;
            }
            templateTrait = traitGen.Traitsdic[template];
        }

        void ClearRuntime()
        {
            templateTrait = null;
            RunTimeGenerated = null;
            currentTemplate = null;
        }
        /// <summary>
        /// assigns the ilevel
        /// </summary>
        /// <param name="newLevel"></param>
        public void SetILevel(string newLevel)
        {
            int.TryParse(newLevel, out int result);
            SetILevel(result);

        }
        /// <summary>
        /// assigns the ilevel
        /// </summary>
        /// <param name="newLevel"></param>
        public void SetILevel(int newLevel)
        {
            ilevel = newLevel;
            UpdateRuntime();

        }
        /// <summary>
        /// removes the last random trait added
        /// </summary>
        public void RemoveLastRando()
        {
            if (randomtraitTemplates.Count == 0)
            {
                Debug.LogWarning("Trying to remove last random but there are no natives to remove", null);
                return;
            }
            EquipmentTrait removed = randomtraitTemplates[randomtraitTemplates.Count - 1];
            randomtraitTemplates.RemoveAt(randomtraitTemplates.Count - 1);
            UpdateRuntime();
            OnLastRandomTraitRemoved?.Invoke();

        }
        /// <summary>
        /// removes the last native trait added
        /// </summary>
        public void RemoveLastNative()
        {
            if (nativetraitTemplates.Count == 0)
            {
                Debug.LogWarning("Trying to remove last native but there are no natives to remove", null);
                return;
            }
            EquipmentTrait removed = nativetraitTemplates[nativetraitTemplates.Count - 1];
            nativetraitTemplates.RemoveAt(nativetraitTemplates.Count - 1);
            UpdateRuntime();
            OnLastNativeTraitRemoved?.Invoke();
        }
        /// <summary>
        /// clears all random trait templates
        /// </summary>
        public void ClearAllRandom()
        {
            randomtraitTemplates.Clear();
            if (RunTimeGenerated != null)
            {
                RunTimeGenerated.GetStats().SetNativeTraits(randomtraitTemplates.ToArray());
            }
            UpdateRuntime();
            OnNativeCleared?.Invoke();
        }
        /// <summary>
        /// adds a native trait template
        /// </summary>
        public void AddNativeTrait()
        {
            EquipmentTrait copy = SetupCopy();
            nativetraitTemplates.Add(copy);
            RunTimeGenerated.GetStats().SetNativeTraits(NativetraitTemplates.ToArray());
            UpdateRuntime();
            OnNativeTraitAdded?.Invoke(copy);
        }
        /// <summary>
        /// adds a random trait template
        /// </summary>
        public void AddRandomTrait()
        {
            EquipmentTrait copy = SetupCopy();
            randomtraitTemplates.Add(copy);
            RunTimeGenerated.GetTraitTier()[0].PossibleTierDrops.PossibleTraits = randomtraitTemplates.ToArray();
            UpdateRuntime();
            OnRandomTraitAdded?.Invoke(copy);
        }
        /// <summary>
        /// reassigns the equipment information, e.g. traits and name
        /// </summary>
        public void UpdateRuntime()
        {
            if (RunTimeGenerated == null)
            {
                Debug.LogWarning("Trying to update runtime but runtime is null");
                return;
            }

            RunTimeGenerated.AssignEquipmentTraits(ILevel);
            OnEquipmentUpdated?.Invoke(RunTimeGenerated);

        }
        /// <summary>
        /// set the suffixes
        /// </summary>
        /// <param name="suffixes"></param>
        public void SetSuffixes(string suffixes)
        {
            SuffixString = suffixes;
        }
        /// <summary>
        /// set the prefixes
        /// </summary>
        /// <param name="prefixes"></param>
        public void SetPrefixes(string prefixes)
        {
            PrefixString = prefixes;
        }


    }
}
