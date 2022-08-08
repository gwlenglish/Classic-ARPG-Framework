using UnityEngine;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public interface IVirtualInventoryCell
    {
        Vector2Int Cell { get; set; }
        bool Occupied { get; set; }
        bool Preview { get; set; }
    }

    /// <summary>
    /// defines an inventory cell/slot
    /// </summary>
    [System.Serializable]
    public class VirtualInventoryCell : IVirtualInventoryCell
    {
        public Vector2Int Cell { get => cell; set => cell = value; }
        public bool Occupied { get => occupied; set => occupied = value; }
        public bool Preview { get =>preview; set => preview = value; }

        public Vector2Int cell;
        public bool occupied;
        public bool preview;
        public VirtualInventoryCell(Vector2Int cell, bool occupied)
        {
            this.cell = cell;
            this.occupied = occupied;
        }


    }
}