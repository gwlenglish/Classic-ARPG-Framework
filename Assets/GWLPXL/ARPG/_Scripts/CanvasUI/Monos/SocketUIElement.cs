using GWLPXL.ARPGCore.Items.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
namespace GWLPXL.ARPGCore.CanvasUI.com
{

    public interface ISocketUIElement
    {
        void SetSocket(int index, Item holder);
        Socket GetSocket();
        void Refresh();
    }
    public class SocketUIElement : MonoBehaviour, ISocketUIElement
    {
        public Image Image = default;
        public Sprite EmptySprite = null;
        Socket socket = null;
        int index;
        Item holder;

        public Socket GetSocket()
        {
            return socket;
        }

        public void Refresh()
        {
            Debug.Log("REfreshed");
            if (holder is Equipment)
            {
                Equipment eq = holder as Equipment;
                socket = eq.GetStats().GetSocket(index);
                SetSocket(index, holder);
            }
        }

        public void SetSocket(int index, Item holder)
        {
            this.holder = holder;
            this.index = index;
            if (this.holder is Equipment)
            {
                Equipment eq = this.holder as Equipment;
                this.socket = eq.GetStats().GetSocket(index);
            }
        
            Setup(socket);
        }

        protected virtual void Setup(Socket socket)
        {
            if (this.socket == null)
            {
                Image.sprite = EmptySprite;

            }
            else
            {
                if (socket.SocketedThing is EquipmentSocketable)
                {
                    EquipmentSocketable sock = socket.SocketedThing as EquipmentSocketable;
                    Image.sprite = sock.GetSprite();
                }

            }
        }
    }
}