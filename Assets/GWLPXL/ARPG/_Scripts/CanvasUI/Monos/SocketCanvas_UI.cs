using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;

using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public interface ISocketSmithCanvas
    {
        void Open(IUseSocketSmithCanvas user);
        void Close();
        void Toggle();
        void SetStation(SocketStation station);
        bool GetCanvasEnabled();
        bool GetFreezeMover();
    }



    /// <summary>
    /// handles canvas movement and swapping around sockets
    /// </summary>
    public class SocketCanvas_UI : MonoBehaviour, ISocketSmithCanvas
    {
        #region states
        public enum DraggingState
        {
            None = 0,
            SocketHolder = 1,
            SocketItem = 2,
            SocketInsert = 3
        }
        DraggingState state = DraggingState.None;
        #endregion
        public SocketStation Station => station;

        public SocketUIEvents Events = new SocketUIEvents();
        public GameObject MainPanel = default;
        public bool FreezeDungeon = true;
        [Header("Interaction")]
        public string InteractButton = "Fire1";
        [Header("Socketable")]
        public GameObject SocketablePrefab = default;
        public Transform SocketableContentParent = default;

        [Header("Sockets")]
        public GameObject SocketItemPrefab = default;
        public RectTransform SocketContentParent = default;
        public RectTransform SocketItemPanel = default;
        [Header("Preview")]
        public RectTransform SocketablePreviewInstance = default;
        protected ISocketHolderUIElement previewholder = null;

        [Header("Draggable")]
        public Transform SocketItemDraggableInstance = default;
        protected ISocketItemUIElement socketItemDraggable = null;
        public RectTransform SocketHolderDraggableInstance = default;
        protected ISocketHolderUIElement socketHolderDraggable = null;
        public Transform SocketItemInsertDraggableInstnace = default;
        protected ISocketItemUIElementInsert socketInsert = null;

        protected IUseSocketSmithCanvas user = null;
        protected SocketStation station = default;
        protected Vector3 homePosition;

        #region socketinserts

        Dictionary<GameObject, SocketInsertUI> socketinsertsDic = new Dictionary<GameObject, SocketInsertUI>();
        public class SocketInsertUI
        {
            public RectTransform RectTransform;
            public ISocketItemUIElementInsert Interface;
            public SocketInsertUI(GameObject obj)
            {
                RectTransform = obj.GetComponent<RectTransform>();
                Interface = obj.GetComponent<ISocketItemUIElementInsert>();

            }
        }
        #endregion
        #region socketholderui 
        protected Dictionary<Item, ISocketHolderUIElement> uidic = new Dictionary<Item, ISocketHolderUIElement>();
        Dictionary<GameObject, SocketHolderUI> socketholdersdic = new Dictionary<GameObject, SocketHolderUI>();
        [System.Serializable]
        public class SocketHolderUI
        {
            public Item ItemStack;
            public GameObject UIObject;
            public RectTransform RectTransform;
            public ISocketHolderUIElement Interface;
            public SocketHolderUI(Item stack, GameObject ob)
            {
                ItemStack = stack;
                UIObject = ob;
                RectTransform = ob.GetComponent<RectTransform>();
                Interface = ob.GetComponent<ISocketHolderUIElement>();
            }
        }
        #endregion
        #region socketitem class
        //socket items.
        Dictionary<int, GameObject> slotPerUIDic = new Dictionary<int, GameObject>();
        Dictionary<GameObject, SocketItemUI> socketitemsdic = new Dictionary<GameObject, SocketItemUI>();
        [System.Serializable]
        public class SocketItemUI
        {
            public ItemStack ItemStack;
            public GameObject UIObject;
            public RectTransform RectTransform;
            public ISocketItemUIElement Interface;
            public SocketItemUI(ItemStack stack, GameObject ob)
            {
                ItemStack = stack;
                UIObject = ob;
                RectTransform = ob.GetComponent<RectTransform>();
                Interface = ob.GetComponent<ISocketItemUIElement>();
            }



        }
        #endregion

        #region unity calls
        protected virtual void Awake()
        {
            socketInsert = SocketItemInsertDraggableInstnace.GetComponent<ISocketItemUIElementInsert>();
            previewholder = SocketablePreviewInstance.GetComponent<ISocketHolderUIElement>();
            socketItemDraggable = SocketItemDraggableInstance.GetComponent<ISocketItemUIElement>();
            socketHolderDraggable = SocketHolderDraggableInstance.GetComponent<ISocketHolderUIElement>();
            homePosition = SocketHolderDraggableInstance.position;
        }
        protected virtual void OnEnable()
        {
            SocketHolderUIElement uielem = SocketablePreviewInstance.GetComponent<SocketHolderUIElement>();
            if (uielem != null)
            {
                uielem.OnInsertablesCreated += SocketInserts;
            }


        }

        protected virtual void OnDisable()
        {
            SocketHolderUIElement uielem = SocketablePreviewInstance.GetComponent<SocketHolderUIElement>();
            if (uielem != null)
            {
                uielem.OnInsertablesCreated -= SocketInserts;
            }
        }
        protected virtual void LateUpdate()
        {
            DraggingBehavior();

        }
        #endregion

        #region public interface
        public void Close()
        {
            ShutDown();
        }

        public bool GetCanvasEnabled()
        {
            return MainPanel.activeInHierarchy;
        }

        public bool GetFreezeMover()
        {
            return FreezeDungeon;
        }

        public void Open(IUseSocketSmithCanvas user)
        {
            Setup(user);
        }
        public void SetStation(SocketStation station)
        {
            this.station = station;
        }

        public void Toggle()
        {
            if (MainPanel.activeInHierarchy)
            {
                Close();
            }
            else
            {
                Open(user);
            }
        }

        #endregion

        #region protected virtual
        protected virtual void DraggingBehavior()
        {
            if (GetCanvasEnabled() == false) return;

            switch (state)
            {
                case DraggingState.None:
                    CheckDraggingStartDraggingHolder();
                    CheckDraggingStartDraggingSocketItem();
                    CheckDraggingSlotInserts();
                    break;
                case DraggingState.SocketHolder:
                    DoDraggingSocketHolder();
                    CheckStopDraggingHolder();
                    break;
                case DraggingState.SocketInsert:
                    DoDraggingSocketInsert();
                    CheckStopDraggingItemInsert();
                    break;
                case DraggingState.SocketItem:
                    DoDraggingSocketItem();
                    CheckStopDraggingSocketItem();
                    break;

            }
        }

        #region socket inserts
        protected virtual void SocketInserts(List<GameObject> inserts)
        {
            socketinsertsDic.Clear();
            for (int i = 0; i < inserts.Count; i++)
            {
                socketinsertsDic[inserts[i]] = new SocketInsertUI(inserts[i]);
            }

        }

        protected virtual void CheckDraggingSlotInserts()
        {
            if (socketinsertsDic.Count <= 0) return;//no inserts, no dragging
            if (state != DraggingState.None) return;

            foreach (var kvp in socketinsertsDic)
            {

                Vector2 localMousePos = kvp.Value.RectTransform.InverseTransformPoint(Input.mousePosition);
                if (kvp.Value.RectTransform.rect.Contains(localMousePos))
                {
                    if (Input.GetButtonDown(InteractButton))
                    {
                        if (kvp.Value.Interface.GetSocket().SocketedThing == null) continue;//dont add a null one...

                        socketInsert.SetIndex(kvp.Value.Interface.GetIndex());
                        socketInsert.SetSocket(kvp.Value.Interface.GetSocket(), kvp.Value.Interface.GetHolder());
                        Debug.Log("Drag Start Success");
                        state = DraggingState.SocketInsert;
                        Events.OnStartDragSocketInsert?.Invoke(kvp.Value.Interface.GetSocket(), kvp.Value.Interface.GetHolder());
                        Events.SceneEvents.OnStartDragSocketInsert?.Invoke(kvp.Value.Interface.GetSocket(), kvp.Value.Interface.GetHolder());
                        break;


                    }
                }
            }
        }
        protected virtual void CheckStopDraggingItemInsert()
        {
            if (Input.GetButtonUp(InteractButton) == false) return;

            ISocketItemUIElementInsert insert = socketInsert;
            foreach (var kvp in socketinsertsDic)
            {
                //check if we put it back. 

                RectTransform socketpanel = SocketItemPanel;
                Vector2 localmouse = socketpanel.InverseTransformPoint(Input.mousePosition);

                if (socketpanel.rect.Contains(localmouse))
                {
                    //add back to inventory.
                    if (insert.GetSocket().SocketedThing == null) continue;

                    bool canAdd = station.Inventory.CanWeAddItem(insert.GetSocket().SocketedThing);
                    if (canAdd)
                    {
                        ReturnedToInventory(insert);
                    }
                    else
                    {
                        //reset
                        Events.OnSocketInsertMoveFail?.Invoke(insert.GetSocket(), insert.GetHolder());
                        Events.SceneEvents.OnSocketInsertMoveFail?.Invoke(insert.GetSocket(), insert.GetHolder()); ;
                        Debug.Log("Return to Insert");
                    }
                    break;
                }

                RectTransform rectitem = kvp.Value.RectTransform;//this doesn't work, 
                localmouse = rectitem.InverseTransformPoint(Input.mousePosition);
                if (rectitem.rect.Contains(localmouse))
                {
                    //swap them.
                    bool swap = station.SwapSocketItem(insert.GetHolder() as Equipment, insert.GetIndex(), kvp.Value.Interface.GetIndex());
                    if (swap)
                    {
                        Swapped(insert);
                        break;
                    }

                }


            }



            SocketItemInsertDraggableInstnace.position = homePosition;
            state = DraggingState.None;


        }
        protected virtual void DoDraggingSocketInsert()
        {
            SocketItemInsertDraggableInstnace.position = Input.mousePosition;

        }

        #endregion

        #region socket items

        protected virtual void DoDraggingSocketItem()
        {
            SocketItemDraggableInstance.position = Input.mousePosition;

        }
        protected virtual void CheckStopDraggingSocketItem()
        {
            if (Input.GetButtonUp(InteractButton) == false) return;

            SocketItem socketitem = socketItemDraggable.GetSocketItem().Item as SocketItem;

            foreach (var kvp in socketinsertsDic)
            {
                RectTransform recT = kvp.Value.RectTransform;
                Rect rect = recT.rect;
                Vector2 localMousePos = recT.InverseTransformPoint(Input.mousePosition);
                if (rect.Contains(localMousePos))
                {
                    ISocketItemUIElementInsert socketinsert = kvp.Value.Interface;
                    Debug.Log(socketinsert);
                    Equipment eq = previewholder.GetSocketHolder();
                    int index = socketinsert.GetIndex();

                    if (station.CanAdd(eq, index, socketitem))
                    {
                        //check if socketable exists, and if so, check if we can add to inventory.
                        EquipmentSocketable eqsock = station.SocketItemExists(eq, index);
                        if (eqsock != null)
                        {
                            bool canAddToInventory = station.Inventory.CanWeAddItem(eqsock);
                            bool removefrominventory = station.Inventory.RemoveItemFromSlot(socketitem, socketItemDraggable.GetSocketItem().SlotID);
                            if (canAddToInventory && removefrominventory)
                            {
                                station.Inventory.AddItemsToInventory(eqsock, 1);
                                bool added = station.AddSocketable(eq, socketitem, index, station.Rename);
                                if (added)
                                {
                                    Events.OnSocketInsertSuccess?.Invoke(socketInsert.GetSocket(), socketinsert.GetHolder());
                                    Events.SceneEvents.OnSocketInsertSuccess?.Invoke(socketInsert.GetSocket(), socketinsert.GetHolder());
                                    socketItemDraggable.UpdateItem();
                                    socketinsert.UpdateSocket();
                                    previewholder.SetSockets(eq);
                   
                                    break;
                                }
                            }
                        }
                        else
                        {
                            ///try remove from inventory
                            bool removefrominventory = station.Inventory.RemoveItemFromSlot(socketitem, socketItemDraggable.GetSocketItem().SlotID);
                            if (removefrominventory)
                            {
                                //if removed okay, then add as socketable
                                bool added = station.AddSocketable(eq, socketitem, index, station.Rename);
                                if (added)
                                {
                                    Events.OnSocketInsertSuccess?.Invoke(socketinsert.GetSocket(), socketinsert.GetHolder());
                                    Events.SceneEvents.OnSocketInsertSuccess?.Invoke(socketinsert.GetSocket(), socketinsert.GetHolder());
                                    socketItemDraggable.UpdateItem();
                                    socketinsert.UpdateSocket();
                                    previewholder.SetSockets(eq);
                      
                                    break;
                                }

                   
                            }
                        }
                        
                    }
                    else
                    {
                        Events.OnSocketInsertMoveFail?.Invoke(socketinsert.GetSocket(), socketinsert.GetHolder());
                        Events.SceneEvents.OnSocketInsertMoveFail?.Invoke(socketinsert.GetSocket(), socketinsert.GetHolder());
                    }


                }


            }

            Events.OnStopDragSocketItem?.Invoke(socketItemDraggable.GetSocketItem());
            Events.SceneEvents.OnStopDragSocketItem?.Invoke(socketItemDraggable.GetSocketItem());
            SocketItemDraggableInstance.position = homePosition;
            state = DraggingState.None;



        }
        protected virtual void CheckDraggingStartDraggingSocketItem()
        {

            if (state != DraggingState.None) return;

            foreach (var kvp in socketitemsdic)
            {
                Vector2 localMousePos = kvp.Value.RectTransform.InverseTransformPoint(Input.mousePosition);
                if (kvp.Value.RectTransform.rect.Contains(localMousePos))
                {
                    if (Input.GetButtonDown(InteractButton))
                    {
                        ItemStack stack = kvp.Value.Interface.GetSocketItem();
                        socketItemDraggable.SetSocketItem(stack.SlotID, station.Inventory);
                        Events.OnStartDragSocketItem?.Invoke(stack);
                        Events.SceneEvents.OnStartDragSocketItem?.Invoke(stack);
                        state = DraggingState.SocketItem;
                        break;
                    }
                }
            }


        }

        #endregion


        #region socket holder
        protected virtual void DoDraggingSocketHolder()
        {
            SocketHolderDraggableInstance.position = Input.mousePosition;

        }
        protected virtual void CheckStopDraggingHolder()
        {
            if (Input.GetButtonUp(InteractButton) == false) return;

            if (SocketablePreviewInstance.rect.Overlaps(SocketHolderDraggableInstance.rect))
            {
                //placed
                previewholder.SetSockets(socketHolderDraggable.GetSocketHolder());
                Events.OnPreviewSetHolder?.Invoke(socketHolderDraggable.GetSocketHolder());
                Events.SceneEvents.OnPreviewSetHolder?.Invoke(socketHolderDraggable.GetSocketHolder());
            }

            Events.OnStopDragSocketHolder?.Invoke(socketHolderDraggable.GetSocketHolder());
            Events.SceneEvents.OnStopDragSocketHolder?.Invoke(socketHolderDraggable.GetSocketHolder());

            SocketHolderDraggableInstance.position = homePosition;
            state = DraggingState.None;

        }
        protected virtual void CheckDraggingStartDraggingHolder()
        {

            if (state != DraggingState.None) return;

            foreach (var kvp in socketholdersdic)
            {
                Vector2 localMousePos = kvp.Value.RectTransform.InverseTransformPoint(Input.mousePosition);
                if (kvp.Value.RectTransform.rect.Contains(localMousePos))
                {
                    if (Input.GetButtonDown(InteractButton))
                    {
                        if (kvp.Value.Interface.GetSocketHolder() == null) continue;
                        Item item = kvp.Value.Interface.GetSocketHolder();
                        socketHolderDraggable.SetSockets(item as Equipment);
                        Events.OnStartDragSocketHolder?.Invoke(item as Equipment);
                        Events.SceneEvents.OnStartDragSocketHolder?.Invoke(item as Equipment);
                        state = DraggingState.SocketHolder;
                        break;

                    }
                }
            }


        }
        #endregion

        #region misc ui
        protected virtual void ShutDown()
        {
            if (station != null)
            {
                station.OnSmithClosed?.Invoke();
                station.Inventory.OnSlotChanged -= UpdateUI;
                station.OnAddSocketable -= UpdateEquipmentUI;
                station.OnRemoveSocketable -= UpdateEquipmentUI;
            }

            uidic.Clear();
            MainPanel.gameObject.SetActive(false);

            foreach (var kvp in socketholdersdic)
            {
                Destroy(kvp.Key);
            }


            foreach (var kvp in socketitemsdic)
            {
                station.Inventory.OnSlotChange -= kvp.Value.Interface.UpdateItem;
                slotPerUIDic.Remove(kvp.Value.ItemStack.SlotID);
                Destroy(kvp.Key);
            }


            socketitemsdic.Clear();
            slotPerUIDic.Clear();
            socketholdersdic.Clear();

            Events.OnClose?.Invoke(user);
            Events.SceneEvents.OnClose?.Invoke(user);
            station = null;
            user = null;

            
            if (FreezeDungeon)
            {
                DungeonMaster.Instance.GetDungeonUISceneRef().SetDungeonState(1);
            }
        }
        protected virtual void UpdateUI(int slot, ItemStack stack)
        {

            if (slotPerUIDic.ContainsKey(slot))
            {
                //update
                GameObject obj = slotPerUIDic[slot];
                if (stack.Item == null || stack.Item is EquipmentSocketable == false || stack.CurrentStackSize <= 0)
                {

                    RemoveSocketItemUI(obj);


                }
                else
                {
                    socketitemsdic[obj].Interface.UpdateItem();
                }



            }
            else
            {
                CreateUISocketElement(stack);
            }

        }
        protected virtual void Setup(IUseSocketSmithCanvas user)
        {

            this.user = user;
            this.user.SetCanvasSmithCanvas(this);
            MainPanel.gameObject.SetActive(true);
            station.OnRemoveSocketable += UpdateEquipmentUI;
            station.OnAddSocketable += UpdateEquipmentUI;
            station.Inventory.OnSlotChanged += UpdateUI;

            List<Equipment> equipped = station.GetEquippedEquipmentWithSockets();
            for (int i = 0; i < equipped.Count; i++)
            {
                Item item = equipped[i];
                CreateUISocketableElement(item);
            }

            List<Equipment> inventory = station.GetEquipmentInInventoryWithSockets();
            for (int i = 0; i < inventory.Count; i++)
            {
                CreateUISocketableElement(inventory[i]);
            }

            List<ItemStack> socketableItems = station.Inventory.GetAllUniqueStacks();
            for (int i = 0; i < socketableItems.Count; i++)//rethink...
            {
                CreateUISocketElement(socketableItems[i]);
            }

            if (FreezeDungeon)
            {
                DungeonMaster.Instance.GetDungeonUISceneRef().SetDungeonState(0);
            }
            station.OnSmithOpen?.Invoke();

            Events.SceneEvents.OnOpen?.Invoke(user);
            Events.OnOpen?.Invoke(user);
        }

        protected virtual void CreateUISocketElement(ItemStack itemStack)
        {
            if (slotPerUIDic.ContainsKey(itemStack.SlotID) == false)
            {
                GameObject instance = Instantiate(SocketItemPrefab, SocketContentParent);
                ISocketItemUIElement socketitem = instance.GetComponent<ISocketItemUIElement>();
                station.Inventory.OnSlotChange += socketitem.UpdateItem;
                socketitem.SetSocketItem(itemStack.SlotID, station.Inventory);
                SocketItemUI newui = new SocketItemUI(itemStack, instance);
                socketitemsdic[instance] = newui;
                slotPerUIDic[itemStack.SlotID] = instance;
            }


        }

        protected virtual void RemoveSocketItemUI(GameObject key)
        {
            if (socketitemsdic.ContainsKey(key))
            {
                //do the removal
                station.Inventory.OnSlotChange -= socketitemsdic[key].Interface.UpdateItem;
                slotPerUIDic.Remove(socketitemsdic[key].ItemStack.SlotID);
                socketitemsdic.Remove(key);

                Destroy(key);


            }

        }
        protected virtual void CreateUISocketableElement(Item item)
        {
            GameObject instance = Instantiate(SocketablePrefab, SocketableContentParent);
            ISocketHolderUIElement element = instance.GetComponent<ISocketHolderUIElement>();
            element.SetSockets(item as Equipment);

            uidic[item] = element;

            SocketHolderUI newui = new SocketHolderUI(item, instance);
            socketholdersdic[instance] = newui;
        }
        protected virtual void UpdateEquipmentUI(Item item)
        {
            if (uidic.ContainsKey(item))
            {
                uidic[item].SetSockets(item as Equipment);
            }
        }
        protected virtual void Swapped(ISocketItemUIElementInsert insert)
        {
            insert.UpdateSocket();
            socketHolderDraggable.RefreshSockets();//also need to refresh the thing in the item panel...
            uidic[insert.GetHolder()].RefreshSockets();//doesn't seem to work...
            previewholder.SetSockets(previewholder.GetSocketHolder());
            socketInsert.UpdateSocket();
            Debug.Log("Swap");
        }
        protected virtual void ReturnedToInventory(ISocketItemUIElementInsert insert)
        {
            station.Inventory.AddItemsToInventory(insert.GetSocket().SocketedThing, 1);
            station.RemoveSocketable(insert.GetHolder() as Equipment, insert.GetIndex(), station.Rename);
            //need to update visuals now. 
            insert.UpdateSocket();
            socketHolderDraggable.RefreshSockets();
            uidic[insert.GetHolder()].RefreshSockets();
            previewholder.SetSockets(previewholder.GetSocketHolder());
            //needs to create its own ui element or add to existing...
            Debug.Log("Return to Inventory");
            Events.OnSocketInsertReturnedToInventory?.Invoke(socketInsert.GetSocket(), socketInsert.GetHolder());
            Events.SceneEvents.OnSocketInsertReturnedToInventory?.Invoke(socketInsert.GetSocket(), socketInsert.GetHolder());
        }
        #endregion

        #endregion

    }
}