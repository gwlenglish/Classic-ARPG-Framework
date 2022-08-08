using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.Traits.com;
using System.Text;
using GWLPXL.ARPGCore.Statics.com;
using System.Linq;

namespace GWLPXL.ARPGCore.Items.com
{
    
    /// <summary>
    /// class that performs the enchanting. Adds a trait to equipment.
    /// </summary>
    public class EnchantingStation
    {
        public System.Action<Equipment> OnEnchanted;
        public System.Action<EnchantingStation> OnStationSetup;
        public System.Action<EnchantingStation> OnStationClosed;
        public ActorInventory UserInventory => userInventory;
        public AffixReaderSO AffixReaderSO = default;
        public bool RenameItem = true;
        public RenameType RenameType = RenameType.Suffix;
        List<EquipmentEnchant> enchants = new List<EquipmentEnchant>();
        ActorInventory userInventory;

        #region ini cycle
        /// <summary>
        /// shutdown of the enchanting station
        /// </summary>
        public virtual void CloseStation()
        {
            this.enchants.Clear();
            this.userInventory = null;
            OnStationClosed?.Invoke(this);

        }
        /// <summary>
        /// initialization of the enchanting station
        /// </summary>
        /// <param name="userInventory"></param>
        /// <param name="enchants"></param>
        /// <param name="affixreader"></param>
        /// <param name="rename"></param>
        /// <param name="type"></param>
        public virtual void SetupStation(ActorInventory userInventory, List<EquipmentEnchant> enchants, AffixReaderSO affixreader = null, bool rename = true, RenameType type = RenameType.Suffix)
        {
            this.enchants = enchants;
            this.userInventory = userInventory;
            this.RenameType = type;
            this.RenameItem = rename;
            this.AffixReaderSO = affixreader;
           
            OnStationSetup?.Invoke(this);
        }
        #endregion

        #region getters
        /// <summary>
        /// Returns all enchants
        /// </summary>
        /// <returns></returns>
        public virtual List<EquipmentEnchant> GetAllEnchants()
        {
            return enchants;
        }
        /// <summary>
        /// Returns a list of names
        /// </summary>
        /// <returns></returns>
        public virtual List<string> GetAllEnchantsNames()
        {
            List<string> names = new List<string>();
            for (int i = 0; i < enchants.Count; i++)
            {
                names.Add(enchants[i].EnchantName);
                

            }
            return names;
        }
       
        #endregion

        #region enchant overrides
       
       

        /// <summary>
        /// takes a single enchant
        /// </summary>
        /// <param name="equipment"></param>
        /// <param name="enchant"></param>
        /// <param name="isNative"></param>
        /// <param name="isPreview"></param>
        public virtual void Enchant(Equipment equipment, EquipmentEnchant enchant, bool isNative = true)
        {
            List<EquipmentTrait> traits = enchant.EnchantTraits;
            for (int j = 0; j < traits.Count; j++)
            {
                Enchant(equipment, traits[j], enchants[j].EnchantLevel, isNative);//make true to bypass the event to raise
            }

            if (RenameItem)
            {
                EquipmentDescription.RenameItemWithEnchant(equipment, enchant, AffixReaderSO, RenameType);
            }
           
        }
        /// <summary>
        /// takes a list of enchants
        /// </summary>
        /// <param name="equipment"></param>
        /// <param name="enchants"></param>
        /// <param name="isNative"></param>
        /// <param name="isPreview"></param>
        protected virtual void Enchant(Equipment equipment, List<EquipmentEnchant> enchants, bool isNative = true, bool isPreview = false)
        {
            for (int i = 0; i < enchants.Count; i++)
            {
                List<EquipmentTrait> traits = enchants[i].EnchantTraits;
                for (int j = 0; j < traits.Count; j++)
                {
                    Enchant(equipment, traits[j], enchants[i].EnchantLevel, isNative);//make true to bypass the event to raise
                }
            }

        }
        /// <summary>
        /// renames and raises the event if not a preview enchant.
        /// </summary>
        /// <param name="equipment"></param>
        /// <param name="isPreview"></param>
        /// <summary>
        /// Modifies an existing item and adds a trait to it at the ilevel. Can overload to put in the native or random slots. Preview will not raise the Enchant event.
        /// </summary>
        /// <param name="equipment"></param>
        /// <param name="newEnchantTrait"></param>
        /// <param name="traitIlevel"></param>
        /// <param name="isNative"></param>
        protected virtual void Enchant(Equipment equipment, EquipmentTrait newEnchantTrait, int traitIlevel, bool isNative = true)
        {
            if (equipment == null)
            {
                DebugHelpers.com.ARPGDebugger.DebugMessage("Trying to enchant equipment which isn't enchantable ", null);
                return;
            }
            if (equipment.CanEnchant() == false)
            {
                DebugHelpers.com.ARPGDebugger.DebugMessage("Trying to enchant equipment which isn't enchantable " + equipment.GetUserDescription(), equipment);
                return;
            }
            EquipmentTrait[] existing = new EquipmentTrait[0];
            if (isNative)
            {
                existing = equipment.GetStats().GetNativeTraits();

            }
            else
            {
                existing = equipment.GetStats().GetRandomTraits();
            }

            EquipmentTrait copy = ScriptableObject.Instantiate(newEnchantTrait);
            copy.SetRandomValue(traitIlevel);
            System.Array.Resize(ref existing, existing.Length + 1);
            existing[existing.Length - 1] = copy;

            if (isNative)
            {
                equipment.GetStats().SetNativeTraits(existing);
            }
            else
            {
                equipment.GetStats().SetRandomTraits(existing);
            }



        }


     
        #endregion
    }
}
