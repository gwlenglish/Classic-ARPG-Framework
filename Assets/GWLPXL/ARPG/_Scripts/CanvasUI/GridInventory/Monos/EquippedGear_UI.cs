using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GWLPXL.ARPGCore.CanvasUI.com
{


    /// <summary>
    /// example UI class that uses the EquippedGear class and subs to GridInventory_UI
    /// </summary>
    public class EquippedGear_UI : MonoBehaviour
    {

        public GridEquipmentEvents Events;
        public EquippedGearARPG Gear => gear;
        public GridInventory_UI InventoryUI;
        [Header("Gear Slots")]
        [SerializeField]
        protected EquippedGearARPG gear = new EquippedGearARPG();
        #region slot colors
        [Header("Colors")]
        [SerializeField]
        protected Color transparent = new Color(255, 255, 255, 0);
        [SerializeField]
        protected Color neutral = new Color(255, 255, 255, 1);
        [SerializeField]
        protected Color valid = Color.green;
        [SerializeField]
        protected Color invalid = Color.red;
        #endregion
        protected IActorHub user;

        #region unity virtual calls
        protected virtual void OnEnable()
        {
            InventoryUI.Events.OnTryPlace += TryPlace;
            InventoryUI.Events.OnTryRemove += TryRemove;
            InventoryUI.Events.OnStartDraggingPiece += CheckDragging;
            InventoryUI.Events.OnStopDragging += StopCheckDragging;
            InventoryUI.Events.ONTryHighlight += TryHighlight;


        }

        protected virtual void OnDisable()
        {
            InventoryUI.Events.OnTryPlace -= TryPlace;
            InventoryUI.Events.OnTryRemove -= TryRemove;
            InventoryUI.Events.OnStartDraggingPiece -= CheckDragging;
            InventoryUI.Events.OnStopDragging -= StopCheckDragging;
            InventoryUI.Events.ONTryHighlight -= TryHighlight;


        }



        #endregion

        #region public virtual
        /// <summary>
        /// sub to inventory and assign gear slots
        /// </summary>
        /// <param name="user"></param>
        public virtual void CreateGear(IActorHub user)
        {
            if (this.user != null)
            {
                CloseDown();
            }

            this.user = user;
            gear.Setup();

            ActorInventory inv = user.MyInventory.GetInventoryRuntime();
            Dictionary<EquipmentSlotsType, EquipmentSlot> slots = inv.GetEquippedEquipment();
            foreach (var kvp in slots)
            {
                UpdateEquip(kvp.Value);
            }

            inv.OnEquipmentSlotChanged += UpdateEquip;


        }
        /// <summary>
        /// unsub from inventory
        /// </summary>
        public virtual void CloseDown()
        {
            user.MyInventory.GetInventoryRuntime().OnEquipmentSlotChanged -= UpdateEquip;

        }

        #endregion








        #region protected virtual
        /// <summary>
        /// update visual to match equipment slot
        /// </summary>
        /// <param name="ment"></param>
        protected virtual void UpdateEquip(EquipmentSlot ment)
        {
            int key = (int)ment.slot;
            if (gear.GearSlotIDDic.ContainsKey(key))
            {
                IGearSlot slot = gear.GearSlotIDDic[key];
                slot.Equipment = ment.EquipmentInSlots;

                if (ment.EquipmentInSlots == null)
                {
                    slot.ItemInSlotImage.sprite = null;
                    slot.ItemInSlotImage.color = transparent;

                }
                else
                {
                    slot.ItemInSlotImage.sprite = ment.EquipmentInSlots.GetSprite();
                    slot.ItemInSlotImage.color = neutral;

                }

            }





        }

        /// <summary>
        /// gear equipped, clean up carried inventory piece and tell inventory
        /// </summary>
        /// <param name="piece"></param>
        protected virtual void GearEquipped(IInventoryPiece piece)
        {
            Events.SceneEvents.OnEquipped?.Invoke(piece);
            Events.EquippedPiece?.Invoke(piece);
            InventoryUI.NoPieces();
        }
        /// <summary>
        /// gear unequipped, set state to no pieces
        /// </summary>
        protected virtual void GearUnEquipped()
        {
            Events.SceneEvents.OnUnequip?.Invoke();
            InventoryUI.NoPieces();

        }

        /// <summary>
        /// update visuals to reflect valid and invalid slots
        /// </summary>
        /// <param name="dragging"></param>
        protected virtual void CheckDragging(IInventoryPiece dragging)
        {
            if (dragging == null)
            {
                StopCheckDragging();
                return;
            }
            foreach (var kvp in gear.GearSlotIDDic)
            {
                kvp.Value.SlotImage.color = invalid;

            }


            for (int i = 0; i < dragging.EquipmentIdentifier.Length; i++)
            {
                if (gear.GearSlotIDDic.ContainsKey(dragging.EquipmentIdentifier[i]))
                {
                    IGearSlot slot = gear.GearSlotIDDic[dragging.EquipmentIdentifier[i]];
                    slot.SlotImage.color = valid;
                }
            }
        }

        /// <summary>
        /// return all the slot images back to neutral
        /// </summary>
        protected virtual void StopCheckDragging()
        {
            foreach (var kvp in gear.GearSlotIDDic)
            {
                IGearSlot gearslot = kvp.Value;
                gearslot.SlotImage.color = neutral;

            }

        }

        protected virtual void TryHighlight(List<RaycastResult> results)
        {
            foreach (RaycastResult result in results)
            {
                GameObject keyinstance = result.gameObject;
                if (gear.GearSlotDic.ContainsKey(keyinstance) == false) continue;

                IGearSlot slot = gear.GearSlotDic[keyinstance];
                Equipment piece = slot.Equipment;
                Events.OnEquipmentHighlighted?.Invoke(piece);
                return;

            }


        }
        /// <summary>
        /// check to see if results hit a gear slot, if so unequip
        /// </summary>
        /// <param name="results"></param>
        protected virtual void TryRemove(List<RaycastResult> results)
        {
            foreach (RaycastResult result in results)
            {
                if (gear.GearSlotDic.ContainsKey(result.gameObject) == false) continue;
                Debug.Log("Hit Gear " + result.gameObject.name, result.gameObject);
                IGearSlot slot = gear.GearSlotDic[result.gameObject];
                Equipment eq = user.MyInventory.GetInventoryRuntime().GetEquipmentInSlot((EquipmentSlotsType)slot.Identifier);
                if (eq == null) continue;

                if (slot.Equipment != null)
                {
                    user.MyInventory.GetInventoryRuntime().UnEquip(eq);
                    GearUnEquipped();
                 
                    break;
                
                }
    


            }
        }

        /// <summary>
        /// try to place piece into gear slot, if so equip and if already filled, unequip first
        /// </summary>
        /// <param name="results"></param>
        /// <param name="piece"></param>
        protected virtual void TryPlace(List<RaycastResult> results, IInventoryPiece piece)
        {

            foreach (RaycastResult result in results)
            {

                if (gear.GearSlotDic.ContainsKey(result.gameObject) == false) continue;

                IGearSlot slot = gear.GearSlotDic[result.gameObject];


                bool allowed = false;
                for (int i = 0; i < piece.EquipmentIdentifier.Length; i++)
                {
                    if (piece.EquipmentIdentifier[i] == slot.Identifier)
                    {
                        //allow
                        allowed = true;
                        break;
                    }
                }
                if (allowed)
                {
                    if (slot.Equipment != null)
                    {
                        user.MyInventory.GetInventoryRuntime().UnEquip(slot.Equipment);
                        GearUnEquipped();
                    }


                    user.MyInventory.GetInventoryRuntime().Equip(piece.Item as Equipment);
                    GearEquipped(piece);

                }


            }

            
        }


        #endregion


    }
}