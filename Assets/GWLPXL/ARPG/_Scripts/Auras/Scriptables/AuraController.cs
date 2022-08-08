
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Statics.com;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GWLPXL.ARPGCore.Auras.com
{
   
    /// <summary>
    /// Controls the application of Auras, so only one Aura per unique category can be active at a time. 
    /// Anything player controlled should call auras through an aura receiver. 
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Auras/NEW_AuraController")]

    public class AuraController : ScriptableObject, ISaveJsonConfig
    {
        /// <summary>
        /// Aura learned and slot
        /// </summary>
        public System.Action<Aura, int> OnLearnedAura;
        /// <summary>
        /// Aura equipped and slot
        /// </summary>
        public System.Action<Aura, int> OnEquippedAura;
        /// <summary>
        /// Aura unequipped and slot
        /// </summary>
        public System.Action<Aura, int> OnUNEquippedAura;

        [SerializeField]
        protected TextAsset config;

        public AuraControllerData AuraControllerData;

        [SerializeField]
        protected Aura[] startingAuras = new Aura[0];
        [SerializeField]
        [Range(1, 100)]
        protected int equippedcap = 2;
        [SerializeField]
        [Range(1, 100)]
        protected int learnedCap = 100;

        protected Aura[] equipped = new Aura[0];
        protected Aura[] learned = new Aura[0];

        [System.NonSerialized]
        protected Dictionary<int, Aura> appliedPassiveBuffsByCategory = new Dictionary<int, Aura>();
        [System.NonSerialized]
        protected Dictionary<Aura, bool> appliedAuras = new Dictionary<Aura, bool>();

        protected Aura[] persistentscene = new Aura[0];
        public virtual AuraControllerData GetID() => AuraControllerData;
        public virtual void SetSceneAuras(Aura[] auras)
        {
            persistentscene = auras;
        }
        public virtual Aura[] GetSceneAuras()
        {
            return persistentscene;
        }
        public virtual Aura GetEquippedAuraAtSlot(int slot)
        {
            if (slot < 0 || slot > equipped.Length - 1) return null;
            return equipped[slot];
        }
        public virtual Aura[] GetEquippedAuras() => equipped;//read only
       
        public virtual Aura[] GetEquippedAndAppliedAuras()
        {
            List<Aura> temp = new List<Aura>();
            for (int i = 0; i < equipped.Length; i++)
            {
                if (equipped[i] == null) continue;
                appliedAuras.TryGetValue(equipped[i], out bool value);
                if (value)
                {
                    temp.Add(equipped[i]);
                }
            }
            return temp.ToArray();
        }
        public virtual Aura[] GetCopyAllKnownAuras()
        {
            List<Aura> temp = new List<Aura>();
            for (int i = 0; i < learned.Length; i++)
            {
                if (learned[i] == null) continue;
                temp.Add(learned[i]);
            }
            return temp.ToArray();
        }

       
        /// <summary>
        /// 
        /// </summary>
        public virtual void TryInitialize()
        {
            if (learned.Length > 0 && equipped.Length > 0) return;
            appliedPassiveBuffsByCategory.Clear();
            appliedAuras.Clear();
            learned = new Aura[learnedCap];
            equipped = new Aura[equippedcap];
            for (int i = 0; i < startingAuras.Length; i++)
            {
                LearnAura(startingAuras[i]);
            }
            for (int i = 0; i < startingAuras.Length; i++)
            {
                EquipAura(startingAuras[i], i);
            }
        }

        public virtual void EquipAura(Aura anyAura, int atSlot, bool autoLearn = false)
        {
            if (learned.Contains(anyAura) == false)
            {
                if (autoLearn)
                {
                    bool learned = LearnAura(anyAura);
                    if (learned == false)
                    {
                        ARPGDebugger.DebugMessage("Can't equip ability, need to learn it first and can't learn amymore.", this);
                        return;
                    }
                }
                ARPGDebugger.DebugMessage("Can't equip ability, need to learn it first.", this);
                return;
            }

            if (atSlot > equipped.Length - 1)
            {
                ARPGDebugger.DebugMessage("Can't equip ability, slot doesn't exist. Increase the equipped slot size.", this);
                return;
            }

            if (equipped[atSlot] != null)
            {
                LearnAura(equipped[atSlot]);
                OnUNEquippedAura?.Invoke(equipped[atSlot], atSlot);
            }

            equipped[atSlot] = anyAura;
            OnEquippedAura?.Invoke(anyAura, atSlot);
        }

        public virtual bool LearnAura(Aura anyAura)
        {
            if (anyAura == null) return false;

            int emptySlot = -1;
            bool known = false;
            for (int i = 0; i < learned.Length; i++)
            {
                if (anyAura != null && anyAura == learned[i])
                {
                    known = true;
                    break;
                }
            }

            if (known == true)
            {
                ARPGDebugger.DebugMessage("Can't learn aura, already know it", this);
                return false;
            }

            for (int i = 0; i < learned.Length; i++)
            {
                if (learned[i] == null)
                {
                    emptySlot = i;
                    break;
                }
            }

            if (emptySlot == -1)
            {
                ARPGDebugger.DebugMessage("Can't learn ability, no empty slots", this);
                return false;
            }

            //learned it
            learned[emptySlot] = anyAura;
            
            OnLearnedAura?.Invoke(learned[emptySlot], emptySlot);
            return true;

        }
      
     

        public virtual void ToggleEquippedAura(int atSlot, ITakeAura onUser)
        {
            if (atSlot > equipped.Length)
            {
                ARPGDebugger.DebugMessage("Can't toggle aura, slot is not valid.", this);
                return;
            }

            ToggleEquippedAura(equipped[atSlot], onUser);

        }
        //
        /// <summary>
        /// Will toggle the aura active/inactive. Will replace the currently active one with the new one passed as a param if they are the same category.
        /// </summary>
        /// <param name="anyAura"></param>
        /// <param name="onUser"></param>
        public virtual void ToggleEquippedAura(Aura anyAura, ITakeAura onUser)
        {
            if (anyAura == null)
            {
                ARPGDebugger.DebugMessage("Can't toggle aura, doesn't exist.", this);
                return;
            }

            if (equipped.Contains(anyAura) == false)
            {
                ARPGDebugger.DebugMessage("Can't toggle aura, need to equip it first.", this);
                return;
            }
            appliedAuras.TryGetValue(anyAura, out bool value);
            //we toggle, default will be false, so the opposite is true, which means we should try applying
            if (value == false)
            {
                ApplyAura(anyAura, onUser);
            }
            else if (value == true)
            {
                RemoveAura(anyAura, onUser);
            }
            
        }

        public virtual Dictionary<Aura, bool> GetAppliedAuras()
        {
            return appliedAuras;
        }
        /// <summary>
        /// Toggles off and then on the current auras in use.
        /// </summary>
        /// <param name="onUser"></param>
        public virtual void RefreshAuras(ITakeAura onUser)
        {
            Debug.Log("Refresh called");
            List<Aura> reapply = new List<Aura>();
            List<Aura> reremove = new List<Aura>();

            for (int i = 0; i < equipped.Length; i++)
            {
                if (equipped == null) continue;
                GetAppliedAuras().TryGetValue(equipped[i], out bool applied);
                if (applied)
                {
                    ToggleEquippedAura(equipped[i], onUser);//off
                    ToggleEquippedAura(equipped[i], onUser);//on
                }
              
            }


        }

       
        protected virtual void ApplyAura(Aura toApply, ITakeAura onUser)
        {
            appliedPassiveBuffsByCategory.TryGetValue(toApply.GetCategory(), out Aura oldValue);

            if (oldValue != null)
            {
                RemoveAura(oldValue, onUser);
            }

            appliedPassiveBuffsByCategory[toApply.GetCategory()] = toApply;
            appliedAuras[toApply] = true;
            toApply.Apply(onUser);

        }

        protected virtual void RemoveAura(Aura buff, ITakeAura fromUser)
        {
            appliedPassiveBuffsByCategory.TryGetValue(buff.GetCategory(), out Aura value);
            if (value != null)
            {
                value.Remove(fromUser);
                appliedPassiveBuffsByCategory[buff.GetCategory()] = null;
                appliedAuras[buff] = false;
            }
        }
        #region interface

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
        protected virtual void OnValidate()
        {
            if (AuraControllerData.AutoName && string.IsNullOrEmpty(AuraControllerData.Name) == false && this.name != AuraControllerData.Name)
            {
                string path = UnityEditor.AssetDatabase.GetAssetPath(this);
                UnityEditor.AssetDatabase.RenameAsset(path, "AuraController_" + AuraControllerData.Name);
            }

            if (AuraControllerData.AutoAssignUniqueID && AuraControllerData.ID != this.GetInstanceID())
            {
                AuraControllerData.ID = this.GetInstanceID();
            }
        }

    
#endif
    }
}