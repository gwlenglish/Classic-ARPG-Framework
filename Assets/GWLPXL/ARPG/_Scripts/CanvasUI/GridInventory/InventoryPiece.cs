using GWLPXL.ARPGCore.Items.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public interface IInventoryPiece
    {
        GameObject Instance { get; set; }
        GameObject PreviewInstance { get; set; }
        Item Item { get; set; }
        IPattern Pattern { get; set; }
        int[] EquipmentIdentifier { get; set; }
        int ItemStackID { get; set; }
        void CleanUP();
    }

    /// <summary>
    /// defines an inventory piece
    /// </summary>
    [System.Serializable]
    public class InventoryPiece : IInventoryPiece
    {
        public GameObject Instance { get =>instance; set => instance = value; }
        public GameObject PreviewInstance { get => previewInstance; set => previewInstance = value; }
        public IPattern Pattern { get => pattern; set => pattern = value; }
        public int[] EquipmentIdentifier { get => equipmentIdentifier; set =>equipmentIdentifier = value; }
        public Item Item { get => item; set => item = value; }
        public int ItemStackID { get => itemStackID; set => itemStackID = value; }

        [SerializeField]
        protected GameObject instance;
        [SerializeField]
        protected  GameObject previewInstance;
        [SerializeField]
        protected IPattern pattern;
        [SerializeField]
        protected int[] equipmentIdentifier = new int[0];
        [SerializeField]
        protected Item item;
        [SerializeField]
        protected int itemStackID;
        public InventoryPiece(GameObject instance, GameObject previewInstance, Item item, int itemstackid)
        {
            this.itemStackID = itemstackid;
            this.item = item;
            equipmentIdentifier = item.EquipmentIdentifier;
            this.pattern = item.UIPattern.Pattern;
            this.instance = instance;
            this.previewInstance = previewInstance;
            previewInstance.SetActive(false);
        }


        public virtual void CleanUP()
        {
            GameObject.Destroy(instance);
            GameObject.Destroy(previewInstance);
            pattern = null;
            item = null;
            equipmentIdentifier = new int[0];
        }


    }
}