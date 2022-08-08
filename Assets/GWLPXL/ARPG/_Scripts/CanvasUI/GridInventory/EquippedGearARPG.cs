using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GWLPXL.ARPGCore.CanvasUI.com
{


    /// <summary>
    /// come in and gut this
    /// </summary>
    [System.Serializable]
    public class EquippedGearARPG
    {

        public System.Action<IGearSlot> OnSlotCreated;
        public System.Action<IGearSlot> OnSlotRemoved;


        public Dictionary<GameObject, IGearSlot> GearSlotDic => gearSlotDic;
        public Dictionary<int, IGearSlot> GearSlotIDDic => gearSlotIDDic;
        /// <summary>
        /// design time, set in the editor
        /// </summary>
        [Tooltip("Set in the editor")]
        public List<ARPGGearSlot> Slots = new List<ARPGGearSlot>();

        /// <summary>
        /// values used at runtime
        /// </summary>
        [SerializeField]
        [Tooltip("Runtime values")]
        protected List<IGearSlot> registeredSlots = new List<IGearSlot>();

        protected Dictionary<GameObject, IGearSlot> gearSlotDic = new Dictionary<GameObject, IGearSlot>();
        protected Dictionary<int, IGearSlot> gearSlotIDDic = new Dictionary<int, IGearSlot>();


        #region public virtual
        /// <summary>
        /// call to initialize the equipment manager
        /// </summary>
        public virtual void Setup()
        {

            registeredSlots.Clear();
            gearSlotDic.Clear();
            gearSlotIDDic.Clear();
            for (int i = 0; i < Slots.Count; i++)
            {
                AddEquippedGearSlot(Slots[i].SlotInstance, Slots[i].Identifier);
            }



        }

 

        /// <summary>
        /// remove an existing slot completely
        /// </summary>
        /// <param name="instance"></param>
        public virtual void RemoveEquippedGearSlot(GameObject instance)
        {
            if (gearSlotDic.ContainsKey(instance))
            {
                int id = gearSlotDic[instance].Identifier;
                if (gearSlotIDDic.ContainsKey(id))
                {
                    gearSlotIDDic.Remove(id);
                }
                IGearSlot slot = gearSlotDic[instance];
                gearSlotDic.Remove(instance);
                registeredSlots.Remove(slot);
                OnSlotRemoved?.Invoke(slot);

            }
        }

        /// <summary>
        /// add a new gear slot
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="identifier"></param>
        public virtual void AddEquippedGearSlot(GameObject instance, int identifier)
        {
            if (gearSlotDic.ContainsKey(instance) == false)
            {
                ARPGGearSlot slot = new ARPGGearSlot(instance, null, identifier);
                gearSlotDic.Add(instance, slot);
                gearSlotIDDic.Add(slot.Identifier, slot);
              
                registeredSlots.Add(slot);
                OnSlotCreated?.Invoke(slot);
            }


        }

        #endregion

        #region protected virtual






        #endregion
    }
}