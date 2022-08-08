using GWLPXL.ARPGCore.Items.com;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
    /// <summary>
    /// Interface for the socket inserts UI
    /// </summary>
    public interface ISocketItemUIElementInsert
    {
        Item GetHolder();
   
        void SetSocket(Socket socket, Item holder);
        Socket GetSocket();
        int GetIndex();
        void SetIndex(int index);
        void UpdateSocket();
    }
    /// <summary>
    /// example of the socket insert ui class, used to maintain references of any socket items placed within an equipment socket
    /// </summary>
    public class SocketItemInsertUIElement : MonoBehaviour, ISocketItemUIElementInsert
    {

        public Image ThingImage = default;
        public TextMeshProUGUI ThingDescriptionText = default;
        public Sprite EmptySprite = default;

        Socket socket;
        int index;
        Item holder;
        public Item GetHolder()
        {
            return holder;
        }

        public int GetIndex()
        {
            return index;
        }

        public Socket GetSocket()
        {
            return socket;
        }

        public void SetIndex(int index)
        {
            this.index = index;
        }

       

        public void SetSocket(Socket socket, Item holder)
        {
            this.socket = socket;
            this.holder = holder;
            Setup(socket);
        }

        public void UpdateSocket()
        {
            if (holder is Equipment)
            {
                Equipment eq = holder as Equipment;
                socket = eq.GetStats().GetSockets()[index];
                Setup(socket);
            }
          
        }

        /// <summary>
        /// override if you want a different setup
        /// </summary>
        /// <param name="socket"></param>
        protected virtual void Setup(Socket socket)
        {
            if (socket == null)
            {
                Debug.LogWarning("socket shouldn't be null and have an instance of it. Something went wrong");
                return;
            }

            SocketItem socketitem = socket.SocketedThing;
            if (socketitem != null)
            {
                ThingImage.sprite = socketitem.GetSprite();
                ThingDescriptionText.SetText(socketitem.GetUserDescription());
            }
            else
            {
                ThingImage.sprite = EmptySprite;
                ThingDescriptionText.SetText(socket.SocketType.ToString());

            }



        }
      
    }
}