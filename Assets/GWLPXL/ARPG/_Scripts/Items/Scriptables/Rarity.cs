
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Types.com;
using TMPro;
using UnityEngine;

namespace GWLPXL.ARPGCore.Items.com
{




    [CreateAssetMenu(menuName = "GWLPXL/ARPG/RarityType/NEW_Rarity")]

    public class Rarity : ScriptableObject, ISaveJsonConfig
    {
        [SerializeField]
        protected TextAsset config = null;
        [SerializeField]
        protected ItemRarityType type = ItemRarityType.Common;
        [SerializeField]
        protected int weight = 1;
        [SerializeField]
        protected IntRange possibleTraitsRange = null;
        [SerializeField]
        protected Color Color = Color.white;
        [SerializeField]
        protected GameObject _LootTextPrefab = null;
        [SerializeField]
        protected TMP_FontAsset _FontAsset = null;
        public virtual ItemRarityType GetItemRarity()
        {
            return type;
        }

        public virtual int GetWeight()
        {
            return weight;
        }
        public virtual int GetNumberOfTraits()
        {
            return Random.Range(possibleTraitsRange.Min, possibleTraitsRange.Max + 1);
        }
        public virtual Color GetRarityColor()
        {
            return Color;
        }
        public virtual TMP_FontAsset GetTMFont()
        {
            return _FontAsset;
        }
        public virtual GameObject GetLootTextPrefab()
        {
            return _LootTextPrefab;
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

#if UNITY_EDITOR
        void OnValidate()
        {
            string path = UnityEditor.AssetDatabase.GetAssetPath(this);
            string name = GetItemRarity().ToString();
            string weight = "_W" + GetWeight().ToString();
            UnityEditor.AssetDatabase.RenameAsset(path, name + weight);

            if (possibleTraitsRange.Min > possibleTraitsRange.Max)
            {
                possibleTraitsRange.Min = possibleTraitsRange.Max;
            }
            else if (possibleTraitsRange.Max < possibleTraitsRange.Min)
            {
                possibleTraitsRange.Max = possibleTraitsRange.Min;
            }
        }

       

#endif
    }

    #region helper classes
    [System.Serializable]
    public class IntRange
    {
        public int Max;
        public int Min;
    }
    #endregion
}