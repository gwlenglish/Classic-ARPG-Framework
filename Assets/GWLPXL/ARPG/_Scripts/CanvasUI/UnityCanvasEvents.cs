using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GWLPXL.ARPGCore.CanvasUI.com
{

    [System.Serializable]
    public class UnityGridItemUsed : UnityEvent<Item> { }
    [System.Serializable]
    public class UnityGridEquipmentEquipped : UnityEvent<IInventoryPiece> { }
    [System.Serializable]
    public class UnityInventoryPiecePlaced : UnityEvent<IInventoryPiece> { }

    [System.Serializable]
    public class UnityGridInventoryEvents
    {
        public UnityInventoryPiecePlaced OnPiecePlaced;
        public UnityInventoryPiecePlaced OnPieceRemoved;
        public UnityInventoryPiecePlaced OnStartDragging;
        public UnityEvent OnStopDragging;
        public UnityGridItemUsed OnItemUsed;
        public UnityGridItemUsed OnItemDropped;
    }
    [System.Serializable]
    public class UnityGridEquipmentEvents
    {
        public UnityEvent OnUnequip;
        public UnityGridEquipmentEquipped OnEquipped;
    }

    [System.Serializable]
    public class GridInventoryEvents
    {
        public UnityGridInventoryEvents SceneEvents = new UnityGridInventoryEvents();
        public System.Action<IInventoryPiece> OnPiecePlaced;
        public System.Action<IInventoryPiece> OnPieceRemoved;
        public System.Action<Item> OnItemUsed;
        public System.Action<List<RaycastResult>> OnTryRemove;
        public System.Action<List<RaycastResult>> ONTryHighlight;
        public System.Action<IInventoryPiece> OnStartDraggingPiece;
        public System.Action<Item> OnItemDropped;
        public System.Action OnStopDragging;
        public System.Action<List<RaycastResult>, IInventoryPiece> OnTryPlace;
    }

    [System.Serializable]
    public class GridEquipmentEvents
    {
        public UnityGridEquipmentEvents SceneEvents;
        public System.Action<Item> OnEquipmentHighlighted;
        public System.Action<IInventoryPiece> EquippedPiece;


    }

    [System.Serializable]
    public class UnityUpdateSlotEventUser : UnityEvent<int, IAbilityUser> { }
    [System.Serializable]
    public class UnityAbilityEvent : UnityEvent<Ability, IAbilityUser> { }

    [System.Serializable]
    public class DraggableEvents
    {
        public UnityEvent OnDraggableEnabled = new UnityEvent();
        public UnityEvent OnDraggableDisabled = new UnityEvent();
    }

    [System.Serializable]
    public class DraggableAbilityEvents
    {
        public DraggableEvents SceneEvents = new DraggableEvents();
    }

    [System.Serializable]
    public class SlotEvents
    {
        public UnityEvent OnUpdated = new UnityEvent();
        public UnityUpdateSlotEventUser OnEquippedSlotUpdated = new UnityUpdateSlotEventUser();


    }
    [System.Serializable]
   public class EquippedAbilityButtonEvents
    {
        public SlotEvents SceneEvents = new SlotEvents();
    }
}