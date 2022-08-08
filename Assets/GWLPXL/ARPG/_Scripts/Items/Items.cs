
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Types.com;

using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using GWLPXL.ARPGCore.CanvasUI.com;

namespace GWLPXL.ARPGCore.Items.com
{



    public abstract class Item : ScriptableObject, ISaveJsonConfig
    {
        #region inventory grid interface

        public PatternHolder UIPattern = null;

        public int[] EquipmentIdentifier { get => GetEquipUIID(); set => uiEquipID = value; }

        protected int[] uiEquipID = new int[0];
        #endregion


        protected virtual int[] GetEquipUIID()
        {
            return uiEquipID;
        }

       
       

        [SerializeField]
        TextAsset config = null;
        [Header("Item Info")]
        [SerializeField]
        protected ItemID itemID;
        [SerializeField]
        protected bool autoName = true;
        [SerializeField]
        protected Rarity rarity;
        [SerializeField]
        [Tooltip("Used to visually represent the item on the floor, i.e. as loot. An item can have a visual representation of itself as loot and also not be part of the wearable system.")]
        protected GameObject meshPrefab;
        [SerializeField]
        [FormerlySerializedAs("Sprite")]
        protected Sprite sprite;
        [SerializeField]
        protected int purchaseCost = 10;
        [SerializeField]
        protected int sellCost = 5;
        string savedName = string.Empty;
        protected bool canEnchant = false;



        public abstract string GetBaseItemName();
        public abstract string GetGeneratedItemName();
        public abstract void SetGeneratedItemName(string newName);
        public abstract ItemType GetItemType();
        public abstract bool IsStacking();
        public abstract int GetStackingAmount();
        public abstract string GetUserDescription();
        public virtual int GetPurchaseCost() => purchaseCost;
        public virtual int GetSellCost() => sellCost;
        public virtual bool CanEnchant() => canEnchant;
        public virtual void SetCanEnchant(bool canEnchant) => this.canEnchant = canEnchant;
        public virtual void SetID(ItemID newID)
        {
            itemID = newID;
        }
        public virtual ItemID GetID()
        {
            if (itemID == null) return new ItemID(GetGeneratedItemName(), 0, this);
            return itemID;
        }
        public virtual void SetRarity(Rarity newRarity)
        {
            rarity = newRarity;
        }

        public virtual void RemoveItemFromInventory(Item itemToRemove, ActorInventory fromInventory, int inventorySlotID)
        {
            if (fromInventory == null) return;
            if (itemToRemove == null) return;
            fromInventory.RemoveItemFromInventory(inventorySlotID);
        }
        public virtual GameObject CreateMeshInstance(Transform parent)
        {
            if (meshPrefab == null) return null;
            GameObject newObj = Instantiate(meshPrefab, parent);
            return newObj;
        }

        public virtual TMP_FontAsset GetItemTextFont()
        {
            return rarity.GetTMFont();
        }
        public virtual GameObject GetItemTextPrefab()
        {
            return rarity.GetLootTextPrefab();
        }
        public virtual Rarity GetRarity()
        {
            return rarity;
        }

        public virtual Color GetRarityColor()
        {
            if (rarity == null) return Color.white;
            return rarity.GetRarityColor();
        }
        public virtual Sprite GetSprite()
        {
            return sprite;
        }

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
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (autoName == false) return;
            string name = GetGeneratedItemName();
            if (string.IsNullOrEmpty(name) == false && rarity != null)//not empty and we have rarity
            {
                string rarity = GetRarity().GetItemRarity().ToString();
                string path = UnityEditor.AssetDatabase.GetAssetPath(this);

                if (this is Equipment)
                {
    
                    Equipment equip = this as Equipment;
                    EquipmentType type = equip.GetEquipmentType();
                    string equipmentMat = equip.GetMaterialDescription();
                    string equipmentSuffix = "";
                    if (string.IsNullOrEmpty(equipmentMat) == false)
                    {
                        equipmentSuffix = "_" + equipmentMat;
                    }
                    string equipmentPrefix = Formulas.ConvertToInt(equip.GetStats().GetBaseStat()).ToString() + "_"+equip.GetStats().GetBaseType().ToString();
                    savedName = equipmentPrefix + "_" + name + "_" + rarity + equipmentSuffix;
                    if (this.name != savedName)
                    {
                        UnityEditor.AssetDatabase.RenameAsset(path, savedName);
                    }

                }
                else
                {
                    savedName = name + "_" + rarity;
                    if (this.name != savedName)
                    {
                        UnityEditor.AssetDatabase.RenameAsset(path, savedName);
                    }

                }


            }
        }

       

#endif
    }
}