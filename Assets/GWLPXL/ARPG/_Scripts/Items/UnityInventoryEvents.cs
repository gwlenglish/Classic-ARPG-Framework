using UnityEngine.Events;


namespace GWLPXL.ARPGCore.Items.com
{


    [System.Serializable]
    public class UnityInventoryItemAdded : UnityEvent<Item> { }
    [System.Serializable]
    public class UnityInventoryItemRemoved : UnityEvent<Item> { }

    [System.Serializable]
    public class UnityInventoryEquipEvent : UnityEvent<Equipment> { }
    [System.Serializable]
    public class UnityInventoryUnEquipEvent : UnityEvent<Equipment> { }

    [System.Serializable]
    public class UnityInventoryEvents
    {
        public UnityInventoryEquipEvent OnEquipped = new UnityInventoryEquipEvent();
        public UnityInventoryUnEquipEvent OnUnEquip = new UnityInventoryUnEquipEvent();
        public UnityInventoryItemAdded OnItemAdded = new UnityInventoryItemAdded();
        public UnityInventoryItemRemoved OnItemRemoved = new UnityInventoryItemRemoved();

    }
    [System.Serializable]
    public class PlayerInventoryEvents
    {
        public UnityInventoryEvents SceneEvents = new UnityInventoryEvents();
    }










}