using UnityEngine.Events;
using UnityEngine;
using GWLPXL.ARPGCore.CanvasUI.com;

namespace GWLPXL.ARPGCore.Items.com
{

    [System.Serializable]
    public class UnitySocketingUIEvent : UnityEvent<IUseSocketSmithCanvas> { }
    [System.Serializable]
    public class UnitySocketingEvent : UnityEvent<SocketStation> { }
    [System.Serializable]
    public class UnityEquipmentSocketEvent : UnityEvent<Equipment> { }

    [System.Serializable]
    public class UnitySocketInsertEvent : UnityEvent<Socket, Item> { }
    [System.Serializable]
    public class UnitySocketHolderEvent : UnityEvent<Equipment> { }
    [System.Serializable]
    public class UnitySocketItemEvent : UnityEvent<ItemStack> { }
    [System.Serializable]
    public class UnitySocketEvents
    {
        public UnitySocketingEvent OnStationSetup;
        public UnitySocketingEvent OnStationClosed;
        public UnityEquipmentSocketEvent OnEquipmentSocketed;
    }
    [System.Serializable]
    public class UnitySocketSmithEvents
    {
    

        public UnitySocketEvents SceneEvents = new UnitySocketEvents();
    }

    [System.Serializable]
    public class UnitySocketUIEvents
    {
        [Header("Canvas")]
        public UnitySocketingUIEvent OnOpen;
        public UnitySocketingUIEvent OnClose;
        [Header("Socket Holders")]
        public UnitySocketHolderEvent OnStartDragSocketHolder;
        public UnitySocketHolderEvent OnStopDragSocketHolder;
        [Header("Socket Items")]
        public UnitySocketItemEvent OnStartDragSocketItem;
        public UnitySocketItemEvent OnStopDragSocketItem;
        [Header("Socket Inserts")]
        public UnitySocketInsertEvent OnStartDragSocketInsert;
        public UnitySocketInsertEvent OnSocketInsertSuccess;
        public UnitySocketInsertEvent OnSocketInsertReturnedToInventory;
        public UnitySocketInsertEvent OnSocketInsertMoveFail;
        [Header("Preview")]
        public UnitySocketHolderEvent OnPreviewSetHolder;
        public UnityEngine.Events.UnityEvent OnPreviewSetSocketItem;
 
    }
    [System.Serializable]
    public class SocketUIEvents
    {
        public System.Action<IUseSocketSmithCanvas> OnOpen;
        public System.Action<IUseSocketSmithCanvas> OnClose;
        public System.Action<Socket, Item> OnStartDragSocketInsert;
        public System.Action<Socket, Item> OnSocketInsertSuccess;
        public System.Action<Socket, Item> OnSocketInsertReturnedToInventory;
        public System.Action<Socket, Item> OnSocketInsertMoveFail;
        public System.Action<ItemStack> OnStartDragSocketItem;
        public System.Action<ItemStack> OnStopDragSocketItem;
        public System.Action<Equipment> OnStartDragSocketHolder;
        public System.Action<Equipment> OnStopDragSocketHolder;
        public System.Action<Equipment> OnPreviewSetHolder;
        public UnitySocketUIEvents SceneEvents = new UnitySocketUIEvents();

    }

}