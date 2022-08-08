
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.GameEvents.com
{
    public struct EquipmentEventVars
    {
        public ActorInventory Inventory { get; set; }
        public ActorAttributes Stats { get; set; }
        public Equipment Equipment { get; set; }
        public IAttributeUser StatUser { get; set; }
        public IInventoryUser InvUser { get; set; }
        public EquipmentSlotsType[] Slots { get; set; }
        public bool UnEquip { get; set; }
        public EquipmentEventVars(ActorInventory _inv, ActorAttributes _stats, Equipment _equipment, IAttributeUser _statUser, IInventoryUser _invUser, EquipmentSlotsType[] _slots, bool _unequip)
        {
            Inventory = _inv;
            Stats = _stats;
            Equipment = _equipment;
            StatUser = _statUser;
            InvUser = _invUser;
            Slots = _slots;
            UnEquip = _unequip;
        }
    }
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/GameEvents/NEW_EquipmentChange")]
    public class EquipmentChangeEvent : GameEvent
    {
        public EquipmentEventVars EventVars;

    }
}
