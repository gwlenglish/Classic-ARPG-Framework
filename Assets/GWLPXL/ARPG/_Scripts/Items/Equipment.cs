
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Traits.com;
using GWLPXL.ARPGCore.Types.com;
using GWLPXL.ARPGCore.Wearables.com;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;
namespace GWLPXL.ARPGCore.Items.com
{


    public abstract class Equipment : Item
    {
        [Header("Equipment Info")]
        [SerializeField]
        protected ClassType classRestriction = ClassType.None;
        [SerializeField]
        protected EquipmentSlotsType[] slots = new EquipmentSlotsType[0];
        [SerializeField]
        protected TraitTier[] traitTiers = new TraitTier[0];
        [Header("Equipment Unique")]
        [SerializeField]
        protected EquipmentStats stats;
        [Header("Wearable Prefab")]
        [SerializeField]
        [Tooltip("If the item can be worned by the actor")]
        protected Wearable wearablePefab;
        protected bool stacking = false;
        protected int stackingAmount = 1;
        [SerializeField]
        protected bool enchantable = true;

        protected int[] uiSlotsID = new int[0];
        protected override int[] GetEquipUIID()
        {
            uiSlotsID = new int[slots.Length];
            for (int i = 0; i < uiSlotsID.Length; i++)
            {
                uiSlotsID[i] = (int)slots[i];
            }
            return uiSlotsID;
        }
        public abstract string GetMaterialDescription();
        public abstract EquipmentType GetEquipmentType();
        public override bool CanEnchant()
        {
            return enchantable;
        }
        public override void SetCanEnchant(bool canEnchant)
        {
            enchantable = canEnchant;
        }
        public override string GetBaseItemName()
        {
            return GetStats().GetBaseName();
        }
        public override void SetGeneratedItemName(string newName)
        {
            GetStats().SetGeneratedName(newName);
        }
        public override bool IsStacking()
        {
            return stacking;
        }
        public override int GetStackingAmount()
        {
            return stackingAmount;
        }
        public virtual void AssignEquipmentTraits(int ofLevel)
        {
           
            GetStats().SetRandomNativeStats(ofLevel);
            EquipmentTrait[] randoTraits = DetermineRandomTraits(ofLevel);
            GetStats().SetRandomTraits(randoTraits);
            EquipmentTrait[] alltraits = GetStats().GetAllTraits();
            int combinedequipmentilevel = 0;
            for (int i = 0; i < alltraits.Length; i++)
            {
                combinedequipmentilevel += Mathf.FloorToInt((ofLevel * alltraits[i].GetMyLevelMulti()));//add the trait multiplers to the ilevel, so they factor into it

            }
            ofLevel = combinedequipmentilevel;
            GetStats().SetiLevel(ofLevel);//sets the level
            string generatedName = EquipmentDescription.GenerateNewNameForItem(this);
            ARPGDebugger.DebugMessage(generatedName + " generated", null);
            GetStats().SetGeneratedName(generatedName);
        }

        protected virtual EquipmentTrait[] DetermineRandomTraits(int iLevel)
        {
            EquipmentTrait[] _traits = new EquipmentTrait[0];
            if (GetTraitTier() == null) return _traits;

            int traitLimit = GetRarity().GetNumberOfTraits();

            TraitTier[] tiers = GetTraitTier();
            List<EquipmentTrait> _temp = new List<EquipmentTrait>();

            for (int i = 0; i < tiers.Length; i++)
            {
                int maxNumber = tiers[i].MaxNumberOfTraitsPerTier;
                float multi = tiers[i].ILevelMulti;
                TraitDrops possibleTraits = tiers[i].PossibleTierDrops;
                EquipmentTrait[] traits = possibleTraits.PossibleTraits;
                if (traits.Length == 0) continue;

                if (traitLimit <= 0)//if we reached the limit, leave
                {
                    break;
                }

                for (int j = 0; j < maxNumber; j++)
                {
                    EquipmentTrait template = possibleTraits.RandomRoll();
                    EquipmentTrait trait = Instantiate(template);
                    float ilevelMulti = iLevel * trait.GetMyLevelMulti();
                    int rounded = Mathf.FloorToInt(ilevelMulti * multi);

                    //ARPGDebugger.DebugMessage("iLevel: " + iLevel.ToString());
                    //ARPGDebugger.DebugMessage("trait multi: " + trait.GetMyLevelMulti().ToString());
                    //ARPGDebugger.DebugMessage("rounded value: " + rounded.ToString());

                    trait.SetRandomValue(rounded);//here is where we can control if we throw away traits with '0' on them
                    if (trait.GetLeveledValue() <= 0)
                    {
                        ARPGDebugger.DebugMessage("Didn't add trait because it's 0 or less", this);
                        continue;//don't add it
                    }
                    traitLimit--;
                    _temp.Add(trait);
                    if (traitLimit <= 0)
                    {
                        break;
                    }

                }
            }

            return _temp.ToArray();



        }
        public virtual ClassType GetClassRestriction()
        {
            return classRestriction;
        }
        public override string GetGeneratedItemName()
        {
            return stats.GetGeneratedName();
        }
        public virtual void SetTraitTiers(TraitTier[] newTiers) => traitTiers = newTiers;
        public virtual TraitTier[] GetTraitTier()
        {
            return traitTiers;
        }

        public virtual EquipmentStats GetStats()
        {
            if (stats == null) stats = new EquipmentStats();
            return stats;
        }
        public virtual void SetStats(EquipmentStats newStats)
        {
            stats = newStats;
        }
        public virtual Wearable GetWearable()
        {
            return wearablePefab;
        }

        public virtual void CreateWearableInstance(IWearClothing clothingComponent)
        {
            if (clothingComponent == null) return;

            clothingComponent.WearClothing(this);


        }

        public virtual string[] GetAllRandomTraitNames()
        {
            string[] names = new string[GetStats().GetRandomTraits().Length];
            for (int i = 0; i < GetStats().GetRandomTraits().Length; i++)
            {
                names[i] = GetStats().GetRandomTraits()[i].GetTraitName();
            }
            return names;
        }
        public virtual string[] GetAllNativeTraitNames()
        {
            string[] names = new string[GetStats().GetNativeTraits().Length];
            for (int i = 0; i < GetStats().GetNativeTraits().Length; i++)
            {
                names[i] = GetStats().GetNativeTraits()[i].GetTraitName();
            }
            return names;
        }
        public virtual string[] GetAllTraitNames()
        {
            string[] names = new string[GetStats().GetAllTraits().Length];
            for (int i = 0; i < GetStats().GetAllTraits().Length; i++)
            {
                names[i] = GetStats().GetAllTraits()[i].GetTraitName();
            }
            return names;
        }
        public virtual string GetRandomTraitsDescription()
        {
            StringBuilder sb = new StringBuilder("");
            EquipmentTrait[] traits = GetStats().GetRandomTraits();
            for (int i = 0; i < traits.Length; i++)
            {
                sb.Append("\n" + traits[i].GetTraitUIDescription());
            }
            return sb.ToString();
        }
        public virtual string GetSocketTraitsDescription()
        {
            StringBuilder sb = new StringBuilder("");
            List<Socket> sockets = GetStats().GetSockets();
            for (int i = 0; i < sockets.Count; i++)
            {
                if (sockets[i].SocketedThing == null)
                {
                    sb.Append("\n" + sockets[i].SocketType.ToString() + " EMPTY");
                }
                else
                {
                    if (sockets[i].SocketedThing is EquipmentSocketable)
                    {
                        EquipmentSocketable eqsock = sockets[i].SocketedThing as EquipmentSocketable;
                        sb.Append("\n" + eqsock.GetUserDescription());
                    }

                }
        
            }
            return sb.ToString();
        }

        public virtual string GetNativeTraitDescription()
        {
            StringBuilder sb = new StringBuilder("");
            EquipmentTrait[] traits = GetStats().GetNativeTraits();
            for (int i = 0; i < traits.Length; i++)
            {
                sb.Append("\n" + traits[i].GetTraitUIDescription());
            }
            return sb.ToString();
        }

        public virtual string GetRarityDescription()
        {
            return GetRarity().GetItemRarity().ToString();
        }
        public virtual string GetBaseTypeDescription()
        {

            return GetStats().GetBaseType().ToString() + ": " + GetStats().GetBaseStateConverted().ToString();
        }
        /// <summary>
        /// eventually move to a static naming class
        /// </summary>
        /// <returns></returns>
        public override string GetUserDescription()
        {
            StringBuilder sb = new StringBuilder("");

            //base name
      
            sb.Append(GetGeneratedItemName() + "\n" + GetBaseTypeDescription()  + "\n" + GetRarityDescription());

            sb.Append(GetNativeTraitDescription());

            sb.Append(GetRandomTraitsDescription());

            sb.Append(GetSocketTraitsDescription());

            return sb.ToString();
        }
        public virtual EquipmentSlotsType[] GetEquipmentSlot()
        {
            if (slots == null || slots.Length == 0)
            {
                ARPGDebugger.DebugMessage("Equipmet has not been set up with slots " + GetGeneratedItemName(), this);
                slots = new EquipmentSlotsType[0];
            }
            return slots;
        }
        public virtual void SetEquipmentSlots(EquipmentSlotsType[] newSlots)
        {
            slots = newSlots;
        }



    }

    [System.Serializable]
    public class Socket
    {
        [Tooltip("Leave empty for an empty socket.")]
        public SocketItem SocketedThing = null;
        public SocketTypes SocketType = SocketTypes.Any;
      
        public Socket(SocketItem thing, SocketTypes type)
        {
            SocketedThing = thing;
            SocketType = type;
        }
    }
    #region helper classes
    [System.Serializable]
    public class EquipmentStats
    {
        [Header("Base")]
        [SerializeField]
        [Tooltip("Named used as the base for its description. E.g. 'Sword' would be Prefix + Sword + Suffix.")]
        protected string baseName = string.Empty;
        protected string generatedName = string.Empty;
        [SerializeField]
        [Tooltip("Used to determine if the stat is damage or armor.")]
        protected CombatStatType baseType;
        [SerializeField]
        protected int baseStatMinValue = 1;
        [SerializeField]
        [Range(0, 1f)]
        protected float baseStateILevelMulti;
        [Header("Traits")]
        [Tooltip("Traits you can set in the editor and will always be on this piece of equipment.")]
        [SerializeField]
        protected EquipmentTrait[] nativeTraits = new EquipmentTrait[0];
        [Tooltip("Randomly created at runtime, not guarenteed to be on the equipment always.")]
        protected EquipmentTrait[] randomTraits = new EquipmentTrait[0];
        [SerializeField]
        [Tooltip("defines the number of sockets the item has.")]
        protected Socket[] sockets = new Socket[0];
        protected string droppedName = string.Empty;
        public virtual string GetDroppedName() => droppedName;
        public virtual void SetDroppedName(string droppedName)
        {
            this.droppedName = droppedName;
        }
        protected int iLevel = 1;

        #region sockets

        public virtual List<string> GetAllTraitNouns()
        {
            List<string> _temp = new List<string>();
            EquipmentTrait[] traits = GetAllTraits();
            for (int i = 0; i < traits.Length; i++)
            {
                string[] nouns = traits[i].GetAllNouns();
                for (int j = 0; j < nouns.Length; j++)
                {
                    _temp.Add(nouns[j]);
                }
            }

            return _temp;
        }
        public virtual List<string> GetAllNounsSockets()
        {
            List<string> _temp = new List<string>();
            for (int i = 0; i < GetSockets().Count; i++)
            {
                if (GetSockets()[i].SocketedThing != null && GetSockets()[i].SocketedThing is EquipmentSocketable)
                {
                    EquipmentSocketable sock = GetSockets()[i].SocketedThing as EquipmentSocketable;
                    for (int k = 0; k < sock.EquipmentTraitSocketable.Count; k++)
                    {
                        EquipmentTrait trait = sock.EquipmentTraitSocketable[k];
                        for (int j = 0; j < trait.GetAllNouns().Length; j++)
                        {
                            _temp.Add(trait.GetAllNouns()[j]);
                        }
                    }
                    
                }
            }
            return _temp;
        }
        public virtual List<string> GetAllNounsTraits()
        {
            List<string> _temp = GetAllNounsNative();
            List<string> rando = GetAllNounsRandom();
            _temp.Concat(rando);
            return _temp;
        }
        public virtual List<string> GetAllNounsRandom()
        {
            List<string> _temp = new List<string>();
            for (int i = 0; i < GetNativeTraits().Length; i++)
            {
                for (int j = 0; j < GetRandomTraits()[i].GetAllNouns().Length; j++)
                {
                    _temp.Add(GetRandomTraits()[i].GetAllNouns()[j]);
                }
            }
            return _temp;
        }
        public virtual List<string> GetAllNounsNative()
        {
            List<string> _temp = new List<string>();
            for (int i = 0; i < GetNativeTraits().Length; i++)
            {
                for (int j = 0; j < GetNativeTraits()[i].GetAllNouns().Length; j++)
                {
                    _temp.Add(GetNativeTraits()[i].GetAllNouns()[j]);
                }
            }
            return _temp;
        }

        /// <summary>
        /// get all prefixes and suffixes
        /// </summary>
        /// <returns></returns>
        public virtual List<string> GetAllTraitAffixes(bool nosockets = false)
        {

            List<string> _temp = new List<string>();
            List<string> affixes = new List<string>();
            EquipmentTrait[] traits;
            if (nosockets)
            {
                traits = GetAllTraitsNoSockets();
            }
            else
            {
                traits = GetAllTraits();
            }

            for (int i = 0; i < traits.Length; i++)
            {
                affixes.Clear();
                affixes = traits[i].GetAllAffixes();
                for (int j = 0; j < affixes.Count; j++)
                {
                    _temp.Add(affixes[j]);
                }
            }

            return _temp;
            
        }
        public virtual List<string> GetAllTraitPrefixes(bool nosckets = false)
        {
            List<string> _temp = new List<string>();

            EquipmentTrait[] traits;
            if (nosckets)
            {
                traits = GetAllTraitsNoSockets();
            }
            else
            {
                traits = GetAllTraits();
            }
            for (int i = 0; i < traits.Length; i++)
            {

                string[] affixes = traits[i].GetPrefixes();
                for (int j = 0; j < affixes.Length; j++)
                {
                    _temp.Add(affixes[j]);
                }
            }

            return _temp;

        }
        public virtual List<string> GetAllTraitSuffixes(bool nosockets = false)
        {
            List<string> _temp = new List<string>();
            EquipmentTrait[] traits;
            if (nosockets)
            {
                traits = GetAllTraitsNoSockets();
            }
            else
            {
                traits = GetAllTraits();
            }
     
           
            for (int i = 0; i < traits.Length; i++)
            {
                string[] affixes = traits[i].GetSuffixes();
                for (int j = 0; j < affixes.Length; j++)
                {
                    _temp.Add(affixes[j]);
                }
            }

            return _temp;

        }
        public virtual List<string> GetAllSocketAffixes()
        {
            List<string> _temp = new List<string>();
            for (int i = 0; i < sockets.Length; i++)
            {
                if (sockets[i].SocketedThing != null)
                {
                    if (sockets[i].SocketedThing is EquipmentSocketable)
                    {
                        EquipmentSocketable sock = sockets[i].SocketedThing as EquipmentSocketable;
                        for (int k = 0; k < sock.EquipmentTraitSocketable.Count; k++)
                        {
                            EquipmentTrait trait = sock.EquipmentTraitSocketable[k];
                            for (int j = 0; j < trait.GetAllAffixes().Count; j++)
                            {
                                _temp.Add(trait.GetAllAffixes()[j]);
                            }
                        }
          
                       
                    }
                }
            }
            return _temp;
        }
        public virtual List<string> GetAllSocketPrefixes()
        {
            List<string> _temp = new List<string>();
            for (int i = 0; i < sockets.Length; i++)
            {
                if (sockets[i].SocketedThing != null)
                {
                    if (sockets[i].SocketedThing is EquipmentSocketable)
                    {
                        EquipmentSocketable sock = sockets[i].SocketedThing as EquipmentSocketable;
                        for (int k = 0; k < sock.EquipmentTraitSocketable.Count; k++)
                        {
                            EquipmentTrait trait = sock.EquipmentTraitSocketable[k];
                            for (int j = 0; j < trait.GetPrefixes().Length; j++)
                            {
                                _temp.Add(trait.GetPrefixes()[j]);
                            }
                        }
                        
                    }
                }
            }
            return _temp;
        }
        public virtual List<string> GetAllSocketSuffixes()
        {
            List<string> _temp = new List<string>();
            for (int i = 0; i < sockets.Length; i++)
            {
                if (sockets[i].SocketedThing != null)
                {
                    if (sockets[i].SocketedThing is EquipmentSocketable)
                    {
                        EquipmentSocketable sock = sockets[i].SocketedThing as EquipmentSocketable;
                        for (int k = 0; k < sock.EquipmentTraitSocketable.Count; k++)
                        {
                            EquipmentTrait trait = sock.EquipmentTraitSocketable[k];
                            for (int j = 0; j < trait.GetSuffixes().Length; j++)
                            {
                                _temp.Add(trait.GetSuffixes()[j]);
                            }
                        }
           
                       
                    }
                }
            }
            return _temp;
        }

        public virtual void SetSocket(int atIndex, Socket socket)
        {
            if (atIndex < 0 || atIndex > sockets.Length - 1)
            {
                Debug.LogWarning("Trying to set socket at index " + atIndex + " but index out of range.");
                return;
            }
            sockets[atIndex] = socket;

        }
        public virtual Socket GetSocket(int atIndex)
        {
            if (atIndex < 0 || atIndex > sockets.Length - 1)
            {
                Debug.LogWarning("Trying to set socket at index " + atIndex + " but index exception");
                return null;
            }
            return sockets[atIndex];
        }
        public virtual void SetSockets(Socket[] sockets)
        {
            this.sockets = sockets;
        }
        public virtual List<Socket> GetSockets()
        {
            List<Socket> _temp = new List<Socket>();
            for (int i = 0; i < sockets.Length; i++)
            {
                _temp.Add(sockets[i]);
            }
            return _temp;
        }

        #endregion
        public virtual CombatStatType GetBaseType()
        {
            return baseType;
        }
        public virtual int GetIlevel()
        {
            return iLevel;
        }
        public virtual string GetBaseName()
        {
            return baseName;
        }
        public virtual string GetGeneratedName() => generatedName;
        public virtual void SetGeneratedName(string nameAddition)
        {
            generatedName = nameAddition;
        }
        public virtual void SetiLevel(int level)
        {
            iLevel = level;

        }
        public virtual void SetRandomNativeStats(int oflevel)
        {
            for (int i = 0; i < nativeTraits.Length; i++)
            {
                nativeTraits[i].SetRandomValue(iLevel);
            }
        }
        /// <summary>
        /// used for descriptions
        /// </summary>
        /// <returns></returns>
        public virtual int GetBaseStateConverted()
        {
            return  Formulas.ConvertToInt(GetBaseStat());
        }
        /// <summary>
        /// used for scaling
        /// </summary>
        /// <returns></returns>
        public virtual float GetBaseStat()
        {
            float value = Formulas.RoundFloat(baseStatMinValue + (baseStatMinValue * baseStateILevelMulti * GetIlevel()));
            return value;
        }
        public virtual void SetRandomTraits(EquipmentTrait[] _new)
        {
            randomTraits = _new;
        }
        public virtual void SetNativeTraits(EquipmentTrait[] _new)
        {
            nativeTraits = _new;
        }
        public virtual EquipmentTrait[] GetNativeTraits()
        {
            return nativeTraits;
        }
        public virtual EquipmentTrait[] GetRandomTraits()
        {
            return randomTraits;
        }

        /// <summary>
        /// native, random, and sockets
        /// </summary>
        /// <returns></returns>
        public virtual EquipmentTrait[] GetAllTraits()
        {
            List<EquipmentTrait> _temp = new List<EquipmentTrait>();
            for (int i = 0; i < nativeTraits.Length; i++)
            {
                _temp.Add(nativeTraits[i]);
            }
            for (int i = 0; i < randomTraits.Length; i++)
            {
                _temp.Add(randomTraits[i]);
            }

            for (int i = 0; i < sockets.Length; i++)
            {
                SocketItem item = sockets[i].SocketedThing;
                if (item != null)
                {
                    if (item is EquipmentSocketable)
                    {
                        EquipmentSocketable eqsock = item as EquipmentSocketable;
                        for (int j = 0; j < eqsock.EquipmentTraitSocketable.Count; j++)
                        {
                            _temp.Add(eqsock.EquipmentTraitSocketable[j]);
                        }
                       
                    }
                }
            }

            return _temp.ToArray();
        }
        /// <summary>
        /// native, random
        /// </summary>
        /// <returns></returns>
        public virtual EquipmentTrait[] GetAllTraitsNoSockets()
        {
            List<EquipmentTrait> _temp = new List<EquipmentTrait>();
            for (int i = 0; i < nativeTraits.Length; i++)
            {
                _temp.Add(nativeTraits[i]);
            }
            for (int i = 0; i < randomTraits.Length; i++)
            {
                _temp.Add(randomTraits[i]);
            }

           

            return _temp.ToArray();
        }

    }

    [System.Serializable]
    public class TraitTier
    {
        [Range(0, 1)]
        public float ILevelMulti;
        public int MaxNumberOfTraitsPerTier;
        public TraitDrops PossibleTierDrops;
    }
    #endregion
}