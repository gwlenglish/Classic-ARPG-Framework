using GWLPXL.ARPGCore.Items.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public interface ISocketHolderUIElement
    {
        void SetSockets(Equipment forEquipment);
        Equipment GetSocketHolder();
        List<ISocketUIElement> GetSockets();
        void RefreshSockets();
    }

    public class SocketHolderUIElement : MonoBehaviour, ISocketHolderUIElement
    {
        public System.Action<List<GameObject>> OnInsertablesCreated;
        [Header("Socket Holder")]
        public GameObject SocketPrefab = default;
        public Transform SocketParent = default;
        public Image Image;
        public TextMeshProUGUI DescriptionText;

        [Header("Socket Items")]
        public GameObject SocketItemPrefab = default;
        public Transform SocketItemParent = default;
        List<GameObject> instances = new List<GameObject>();
        Equipment equipment = null;
        List<ISocketUIElement> socketInstances = new List<ISocketUIElement>();
        List<GameObject> socketitemobjects = new List<GameObject>();
        List<ISocketItemUIElementInsert> inserts = new List<ISocketItemUIElementInsert>();
        public Equipment GetSocketHolder()
        {
            return equipment;
        }

        public List<ISocketUIElement> GetSockets()
        {
            return socketInstances;
        }

        public void SetSockets(Equipment forEquipment)
        {
            this.equipment = forEquipment;

            Setup(forEquipment);

        }

        public void RefreshSockets()
        {
            Setup(equipment);


        }

        protected virtual void Setup(Equipment forEquipment)
        {
            for (int i = 0; i < socketitemobjects.Count; i++)
            {
                Destroy(socketitemobjects[i]);//cldean up
            }
            for (int i = 0; i < instances.Count; i++)
            {
                Destroy(instances[i]);//remove old
            }
           
            socketInstances.Clear();
            inserts.Clear();
            socketitemobjects.Clear();
            instances.Clear();
            List<Socket> sockets = forEquipment.GetStats().GetSockets();

            for (int i = 0; i < sockets.Count; i++)
            {
                GameObject instance = Instantiate(SocketPrefab, SocketParent);
                ISocketUIElement sock = instance.GetComponent<ISocketUIElement>();
                sock.SetSocket(i, forEquipment);
                socketInstances.Add(sock);
                instances.Add(instance);

                if (SocketItemPrefab != null)//only preview displays the inserts
                {
                    GameObject socketitem = Instantiate(SocketItemPrefab, SocketItemParent);
                    socketitemobjects.Add(socketitem);
                    ISocketItemUIElementInsert sockeitemElement = socketitem.GetComponent<ISocketItemUIElementInsert>();
                    sockeitemElement.SetIndex(i);
                    sockeitemElement.SetSocket(sockets[i], equipment);

                    inserts.Add(sockeitemElement);
                }
       

            }
            Image.sprite = forEquipment.GetSprite();
            DescriptionText.SetText(forEquipment.GetUserDescription());

            OnInsertablesCreated?.Invoke(socketitemobjects);

        }

       
    }
}