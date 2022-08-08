
using GWLPXL.ARPGCore.Attributes.com;

using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

namespace GWLPXL.ARPGCore.Traits.com
{


    public abstract class EquipmentTrait : ScriptableObject, ISaveJsonConfig
    {
     

        [SerializeField]
        protected TextAsset config = null;
        [SerializeField]
        protected bool AutoName = true;
        protected readonly string positive = "+";
        protected readonly string negative = "-";
        protected readonly string percent = "%";
        [SerializeField]
        protected EquipmentTraitID traitID = null;
        [Tooltip("Prefixes go before the equipment name.")]
        [SerializeField]
        protected string traitName = string.Empty;
        [SerializeField]
        protected string[] TraitPrefixes = new string[0];
        [Tooltip("Suffixes go after the equipment name.")]
        [SerializeField]
        protected string[] TraitSuffixes = new string[0];
        [SerializeField]
        protected string[] TraitNouns = new string[0];
        //multiplied by level to get stats
        [SerializeField]
        [Tooltip("Base Power. Multiplied by myLevelMulti to get final result.")]
        protected int myValue = 1;
        [SerializeField]
        [Tooltip("An item's power is determined by its ILevel. This is a multipler for the ILevel. So if an item has an Ilevel of 100, a multi of 50 would return a value of 50 of this trait (100 * 50) = 50." +
            "Think of it as one point of Ilevel equals one point in the trait at 100%, and adjust accordingly. So a mylevelmulti of 1 would mean the trait receives 1% of the ilevel, so for 100 it would receive a value of 1." +
            "These traits are floored to int. ")]
        [Range(1, 100)]
        protected int myLevelMulti = 1;
        [SerializeField]
        protected int weight = 1;
        [SerializeField]
        protected Sprite sprite;
        #region public virtual
        public virtual string[] GetPrefixes() => TraitPrefixes;
        public virtual string[] GetSuffixes() => TraitSuffixes;
        public virtual string[] GetAllNouns() => TraitNouns;
        public virtual List<string> GetAllAffixes()
        {
            List<string> _temp = new List<string>();
            for (int i = 0; i < GetPrefixes().Length; i++)
            {
                _temp.Add(GetPrefixes()[i]);
            }
            for (int i = 0; i < GetSuffixes().Length; i++)
            {
                _temp.Add(GetSuffixes()[i]);
            }
            return _temp;
        }
        public virtual void SetPrefixes(string[] newPrefixes) => TraitPrefixes = newPrefixes;
        public virtual void SetSuffixes(string[] newSuffixes) => TraitSuffixes = newSuffixes;
        public virtual int GetMyLevelMultRaw() => myLevelMulti;
        public virtual void SetSprite(Sprite newsprite)
        {
            sprite = newsprite;
        }

        public virtual Sprite GetSprite()
        {
            return sprite;
        }
        public virtual void SetTraitName(string newName)
        {
            traitName = newName;
        }
        public virtual int SetWeight(int newWeight) => weight = newWeight;
        public virtual int GetWeight()
        {
            return weight;
        }
        public virtual float GetMyLevelMulti()
        {
            float percent = (float)myLevelMulti / (float)Formulas.Hundred;
            return percent;
        }

        public void SetTraitID(EquipmentTraitID ID)
        {
            traitID = ID;
        }

        public string GetTraitName()
        {
            return traitName;
        }
        public EquipmentTraitID GetID()
        {
            return traitID;
        }
        public virtual int GetStaticValue()
        {
            return myValue;
        }
        public virtual int GetLeveledValue()
        {
            return Mathf.FloorToInt(GetStaticValue() * GetMyLevelMulti());
        }

        public virtual void SetMulti(int newMulti)
        {
            myLevelMulti = newMulti;
        }
        //applies its level when it drops depending on the level passed through, example is from monsters
        public virtual void SetRandomValue(int level)
        {
            myValue = Formulas.GetEquipmentTraitILevel(level);
        }
        public virtual void SetStaticValue(int value)
        {
            myValue = value;
        }
        public virtual string GetTraitUIDescription()
        {
            StringBuilder sb = new StringBuilder("");
            sb.Append(GetTraitName());
            if (GetLeveledValue() > 0)
            {
                sb.Append(positive);
            }
            else
            {
                sb.Append(negative);
            }
            sb.Append(GetLeveledValue().ToString());
            sb.Append(percent);

            return sb.ToString();

        }
        #endregion

        #region json interface
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
        #endregion

        #region public abstract

        public abstract TraitType GetTraitType();
        public abstract void ApplyTrait(IAttributeUser toActor);
        public abstract void RemoveTrait(IAttributeUser toActor);
        public virtual string GetEditorPrefix()
        {
            return this.GetType().Name;
        }
        public virtual string GetTraitPrefix()
        {
            string prefix = "";
            if (TraitPrefixes.Length > 0)
            {
                prefix = PlayerDescription.GetRandomTraitDescriptor(TraitPrefixes);
            }
            return prefix;
        }
        public virtual string GetTraitSuffix()
        {
            string suffix = "";
            if (TraitSuffixes.Length > 0)
            {
                suffix = PlayerDescription.GetRandomTraitDescriptor(TraitSuffixes);
            }
            return suffix;
        }

        #endregion


        //auto-naming assets
        #region
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (AutoName == false) return;
            string path = UnityEditor.AssetDatabase.GetAssetPath(this);
            string name = GetTraitName();
            string typePrefix = GetEditorPrefix();

            float percent = GetMyLevelMulti();
            percent *= Formulas.Hundred;
            string namevalue = percent.ToString();
            UnityEditor.AssetDatabase.RenameAsset(path, name + "_" + typePrefix + "_" + namevalue + "%");


        }

#endif
    }
    #endregion

}
