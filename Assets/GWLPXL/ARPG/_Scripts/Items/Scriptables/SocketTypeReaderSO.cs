using GWLPXL.ARPGCore.Types.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Items.com
{

    [System.Serializable]
    public class SocketTypeReader
    {
        public List<SocketType> TypeReaders = new List<SocketType>();
    }
    [System.Serializable]
    public class SocketType
    {
        public string Name = default;
        public GWLPXL.ARPGCore.Types.com.SocketTypes Type = default;
        public Sprite Sprite = default;

        public string Description = default;

    }
    /// <summary>
    /// used to map common information to the socket type, not used at the moment.
    /// </summary>
    [CreateAssetMenu(menuName ="GWLPXL/ARPG/Socketables/SocketReader")]
    public class SocketTypeReaderSO : ScriptableObject
    {
        public SocketTypeReader Reader;
        [System.NonSerialized]
        Dictionary<SocketTypes, SocketType> socketTypesdic = new Dictionary<SocketTypes, SocketType>();

        public virtual void Preload()
        {
            for (int i = 0; i < Reader.TypeReaders.Count; i++)
            {
                socketTypesdic[Reader.TypeReaders[i].Type] = Reader.TypeReaders[i];
            }
        }
        public virtual SocketType GetSocketType(SocketTypes type)
        {
            if (socketTypesdic.Count == 0)
            {
                Preload();
            }
            return socketTypesdic[type];
        }
    }
}
