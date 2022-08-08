using UnityEngine.Events;


namespace GWLPXL.ARPGCore.Items.com
{


    [System.Serializable]
    public class UnityEnchantingEvent : UnityEvent<EnchantingStation> { }
    [System.Serializable]
    public class UnityEquipmentEnchantedEvent : UnityEvent<Equipment> { }
    [System.Serializable]
    public class UnityEquipmentEnchantEvent : UnityEvent<EquipmentEnchant> { }
    [System.Serializable]
    public class UnityEnchantableEvent : UnityEvent<ItemStack> { }

    [System.Serializable]
    public class UnityEnchantEvents
    {
        public UnityEnchantingEvent OnStationSetup;
        public UnityEnchantingEvent OnStationClosed;
        public UnityEquipmentEnchantedEvent OnEquipmentEnchanted;
    }
    [System.Serializable]
    public class UnityEnchanterEvents
    {
 
        public UnityEnchantEvents SceneEvents = new UnityEnchantEvents();
    }

    [System.Serializable]
    public class EnchantUISceneEvents
    {
        public UnityEnchantableEvent OnStartDragEnchantable;
        public UnityEquipmentEnchantEvent OnStartDragEnchant;
        public UnityEnchantableEvent OnPreviewSetEnchantable;
        public UnityEquipmentEnchantEvent OnPreviewSetEnchant;
        public UnityEquipmentEnchantedEvent OnEnchantSuccess;
    }
    
    [System.Serializable]
    public class EnchantUIEvents
    {
        public System.Action<ItemStack> OnStartDragEnchantable;
        public System.Action<EquipmentEnchant> OnStartDragEnchant;
        public System.Action<ItemStack> OnPreviewSetEnchantable;
        public System.Action<EquipmentEnchant> OnPreviewSetEnchant;
        public System.Action<Equipment> OnEnchantSuccess;
        public EnchantUISceneEvents SceneEvents = new EnchantUISceneEvents();

    }

}