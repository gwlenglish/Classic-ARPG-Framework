
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;
using GWLPXL.ARPGCore.Statics.com;
namespace GWLPXL.ARPGCore.Classes.com
{


    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Classes/NEW_")]

    public class ActorClass : ScriptableObject, ISaveJsonConfig
    {
        [SerializeField]
        TextAsset Config = null;
        [SerializeField]
        ClassID classID = null;
        [SerializeField]
        ClassAttributes classAttributes = null;
        [SerializeField]
        bool autoName = true;
        public ClassID GetID() => classID;
        public void SetID(ClassID newID) => classID = newID;
        public ClassAttributes GetClassAttributes()
        {
            return classAttributes;
        }
        public string GetClassName()
        {
            string name = "";
            if (classAttributes != null)
            {
                if (string.IsNullOrEmpty(classAttributes.ClassName) == false)
                {
                    name = classAttributes.ClassName;
                }
                else
                {
                    name = classAttributes.ClassType.ToString();
                }
            }
            else
            {
                name = "NULL";
            }

            return name;
        }
        public ClassID GetClassID()
        {
            return classID;
        }
        public void SetClassID(ClassID newID)
        {
            classID = newID;
        }
        /// <summary>
        /// Checks the types and if the conditions are met to equip. 
        /// </summary>
        /// <param name="equipment"></param>
        /// <returns></returns>
        public bool CanEquipment(Equipment equipment)
        {
            bool can = false;
            ClassType classRestriction = equipment.GetClassRestriction();
            can = CanEquip(classRestriction);
            //ARPGDebugger.DebugMessage("Class can equip: " + can, this);
            if (can == false) return false;

            EquipmentSlotsType[] slots = equipment.GetEquipmentSlot();
            for (int i = 0; i < slots.Length; i++)
            {
                can = CanEquip(slots[i]);
                //ARPGDebugger.DebugMessage("Slot can equip: " + can, this);
                if (can == false) return false;
            }

            if (equipment is Accessory)
            {
                Accessory accessory = equipment as Accessory;
                AccessoryType accessType = accessory.GetAccessoryType();
                can = CanEquip(accessType);
                //ARPGDebugger.DebugMessage("Armor can equip: " + can, this);
                if (can == false) return false;
            }

            if (equipment is Armor)
            {
                Armor armor = equipment as Armor;
                ArmorMaterial mat = armor.GetArmorMat();
                can = CanEquip(mat);
                //ARPGDebugger.DebugMessage("Armor can equip: " + can, this);
                if (can == false) return false;
            }

            if (equipment is Weapon)
            {
                Weapon weapon = equipment as Weapon;
                WeaponType type = weapon.GetWeaponType();
                can = CanEquip(type);
                //ARPGDebugger.DebugMessage("Weapon can equip: " + can, this);
                if (can == false) return false;
            }

            //ARPGDebugger.DebugMessage("can equip: " + can, this);

            return can;

        }

        bool CanEquip(ClassType classType)
        {
            if (classAttributes.ClassType == ClassType.None)
            {
                return true;
            }

            if (classType == ClassType.None)
            {
                return true;
            }

            if (classAttributes.ClassType == classType)
            {
                return true;
            }
            return false;
        }

        bool CanEquip(EquipmentSlotsType slot)
        {
            for (int i = 0; i < classAttributes.AllowedSlots.Length; i++)
            {
                if (slot == classAttributes.AllowedSlots[i])
                {
                    return true;
                }
            }
            return false;
        }
        bool CanEquip(ArmorMaterial armor)
        {
            for (int i = 0; i < classAttributes.AllowedArmors.Length; i++)
            {
                if (armor == classAttributes.AllowedArmors[i])
                {
                    return true;
                }
            }
            return false;
        }
        bool CanEquip(WeaponType weapon)
        {
            for (int i = 0; i < classAttributes.AllowedWeapons.Length; i++)
            {
                if (classAttributes.AllowedWeapons[i] == weapon)
                {
                    return true;
                }
            }
            return false;
        }

        bool CanEquip(AccessoryType accessory)
        {
            for (int i = 0; i < classAttributes.AllowedAccessories.Length; i++)
            {
                if (classAttributes.AllowedAccessories[i] == accessory)
                {
                    return true;
                }
            }
            return false;
        }

        #region interface json
        public void SetTextAsset(TextAsset textAsset)
        {
            Config = textAsset;
        }

        public TextAsset GetTextAsset()
        {
            return Config;
        }

        public Object GetObject()
        {
            return this;
        }
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (classAttributes != null && string.IsNullOrEmpty(classAttributes.ClassName) == false && autoName)
            {
                string suffix = "_class";
                string path = UnityEditor.AssetDatabase.GetAssetPath(this);
                UnityEditor.AssetDatabase.RenameAsset(path, classAttributes.ClassName + suffix);

            }
        }

      
#endif
    }
    /// <summary>
    /// Things that currently define a class.
    /// </summary>
    [System.Serializable]
    public class ClassAttributes
    {
        public string ClassName;
        public ClassType ClassType;
        public EquipmentSlotsType[] AllowedSlots;
        public WeaponType[] AllowedWeapons;
        public ArmorMaterial[] AllowedArmors;
        public AccessoryType[] AllowedAccessories;


    }
}