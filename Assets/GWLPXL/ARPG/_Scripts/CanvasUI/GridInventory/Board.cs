using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using System.Collections.Generic;
using UnityEngine;


namespace GWLPXL.ARPGCore.CanvasUI.com
{


    /// <summary>
    /// manager for the inventory board/grid
    /// </summary>
    [System.Serializable]
    public class Board
    {
    
        /// <summary>
        /// piece and slots occupied
        /// </summary>
        public System.Action<IInventoryPiece, List<IBoardSlot>> OnNewPiecePlaced;

        public System.Action<IInventoryPiece, IInventoryPiece> OnPieceSwapped;
        /// <summary>
        /// piece and slots removed
        /// </summary>
        public System.Action<IInventoryPiece, List<IBoardSlot>> OnPieceRemoved;
        /// <summary>
        /// new slot created, sub for your own creation
        /// </summary>
        public System.Action<IBoardSlot> OnBoardSlotCreated;
        /// <summary>
        /// instance and slot id 
        /// </summary>
        public Dictionary<GameObject, Vector2Int> IDS => ui;
        /// <summary>
        /// preview highlight, preview intance and preview status
        /// </summary>
        /// 
        public BoardPreview Preview => preview;
        public List<BoardSlot> Slots => slots;

        public Dictionary<IInventoryPiece, List<IBoardSlot>> SlotsOccupied => slotsOccupied;
        public Dictionary<GameObject, IInventoryPiece> PieceInSlot => pieceinslot;
        protected Dictionary<GameObject, Vector2Int> ui = new Dictionary<GameObject, Vector2Int>();
        protected Dictionary<IInventoryPiece, List<IBoardSlot>> slotsOccupied = new Dictionary<IInventoryPiece, List<IBoardSlot>>();
        [SerializeField]
        [Tooltip("Runtime slots")]
        protected List<BoardSlot> slots = new List<BoardSlot>();

        protected BoardPreview preview = new BoardPreview();
        [SerializeField]
        protected int Rows = 6;
        [SerializeField]
        protected int Columns = 9;

        protected readonly List<IBoardSlot> empty = new List<IBoardSlot>();
        protected Dictionary<GameObject, IInventoryPiece> pieceinslot = new Dictionary<GameObject, IInventoryPiece>();
        /// <summary>
        /// remove piece from board
        /// </summary>
        /// <param name="origin"></param>
        public virtual void RemovePiece(IInventoryPiece origin)
        {

            if (slotsOccupied.ContainsKey(origin))
            {
                List<IBoardSlot> slots = slotsOccupied[origin];
                RemovePiece(slots);
            }


        }
        /// <summary>
        /// remove piece placed at coordinates
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public virtual IInventoryPiece RemovePiece(Vector2Int origin)
        {
            IBoardSlot cell = slots.Find(x => x.Cell.Cell == origin);
            IInventoryPiece piece = cell.PieceOnBoard;
            if (piece != null)
            {
                if (slotsOccupied.ContainsKey(piece))
                {
                    List<IBoardSlot> slots = slotsOccupied[piece];
                    return RemovePiece(slots);

                }
            }


            return cell.PieceOnBoard;

        }

        /// <summary>
        /// used for preview
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public virtual List<IBoardSlot> TryPlace(IInventoryPiece piece, Vector2Int origin)
        {
            IBoardSlot cell = slots.Find(x => x.Cell.Cell == origin);
            List<IBoardSlot> placed = new List<IBoardSlot>();
            List<Vector2Int> pattern = piece.Pattern.GetCurrentPattern();
            placed.Add(cell);
            for (int i = 0; i < pattern.Count; i++)
            {
                Vector2Int local = origin + pattern[i];
                IBoardSlot newcell = slots.Find(x => x.Cell.Cell == local);
                placed.Add(newcell);

            }
            return placed;
        }

        /// <summary>
        /// check to see if we can swap piece, if so swap it
        /// returns count > 0 if successful, count == 0 if not successful
        /// </summary>
        /// <param name="currentOnBoard"></param>
        /// <param name="newpiece"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public virtual List<IBoardSlot> TrySwapWithBoard(IInventoryPiece currentOnBoard, IInventoryPiece newpiece, Vector2Int origin)
        {
            IBoardSlot cell = slots.Find(x => x.Cell.Cell == origin);
            List<IBoardSlot> placed = new List<IBoardSlot>();
            placed.Add(cell);
            List<Vector2Int> pattern = newpiece.Pattern.GetCurrentPattern();
            for (int i = 0; i < pattern.Count; i++)
            {
                Vector2Int local = origin + pattern[i];
                IBoardSlot newcell = slots.Find(x => x.Cell.Cell == local);

                if (newcell == null || newcell.Cell.Occupied && newcell.PieceOnBoard != currentOnBoard)
                {
                    return new List<IBoardSlot>();//fail condition
                }
                else
                {
                    placed.Add(newcell);
                }
            }

            //this is success
            RemovePiece(currentOnBoard);
            PlaceOnBoard(newpiece, origin);

            OnPieceSwapped?.Invoke(currentOnBoard, newpiece);
            return placed;
        }
        /// <summary>
        /// place on board at origin, returns board slots occupied if successful and list returns count 0 if not successful
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        /// 

        public virtual List<IBoardSlot> PlaceOnBoard(IInventoryPiece piece, Vector2Int origin, bool allowSwap = false)
        {
            IBoardSlot cell = slots.Find(x => x.Cell.Cell == origin);
            List<IBoardSlot> placed = new List<IBoardSlot>();
            if (cell.Cell.Occupied)
            {
                return empty;
            }

    
            List<Vector2Int> pattern = piece.Pattern.GetCurrentPattern();
            for (int i = 0; i < pattern.Count; i++)
            {
                Vector2Int local = origin + pattern[i];//doesn't seem to be working?
                IBoardSlot newcell = slots.Find(x => x.Cell.Cell == local);
                if (newcell == null)
                {
                    return empty;
                }

                if (newcell.Cell.Occupied)
                {
                    if (allowSwap)
                    {
                        return TrySwapWithBoard(newcell.PieceOnBoard, piece, origin);
                    }
                    else
                    {
                        return empty;
                    }
                }
                else
                {
                    placed.Add(newcell);
                }
            }

            //placed.Add(cell);//the origin

            for (int i = 0; i < placed.Count; i++)
            {
                pieceinslot[placed[i].Instance] = piece;
                placed[i].Cell.Occupied = true;
                placed[i].PieceOnBoard = piece;
            }


     
            slotsOccupied[piece] = placed;
            OnNewPiecePlaced?.Invoke(piece, placed);

            return placed;
            // Debug.Log("Success");

        }



        /// <summary>
        /// create the grid, also calls the OnBoardSlotCreated
        /// </summary>
        /// <param name="GridTransform"></param>
        /// <param name="CellPrefab"></param>
        /// <param name="PanelGrid"></param>
        public virtual void CreateGrid()
        {

            for (int i = 0; i < Rows; i++)
            {

                for (int j = 0; j < Columns; j++)
                {
                    Vector2Int newID = new Vector2Int(j, i);
                    VirtualInventoryCell cell = new VirtualInventoryCell(newID, false);
                    BoardSlot newslot = new BoardSlot(null, cell, null);
                    preview.CellDictionary.Add(newslot, null);
                    slots.Add(newslot);
                    OnBoardSlotCreated?.Invoke(newslot);

                }

            }

        }

        public virtual List<IBoardSlot> PlaceInFirstAvailable(IInventoryPiece piece)
        {
            foreach (var kvp in slots)
            {
                IVirtualInventoryCell slot = kvp.Cell;
                List<IBoardSlot> placed = PlaceOnBoard(piece, slot.Cell);
                if (placed.Count > 0)
                {
                   
                    //got it
                    return placed;
                }
            }
            return new List<IBoardSlot>();
        }
        /// <summary>
        /// helper to remove piece based on board slots
        /// </summary>
        /// <param name="removed"></param>
        /// <returns></returns>
        protected IInventoryPiece RemovePiece(List<IBoardSlot> removed)
        {
            if (removed.Count == 0) return null;

          
            IInventoryPiece obj = removed[0].PieceOnBoard;//should be the same for all and not null...

            for (int i = 0; i < removed.Count; i++)
            {
                removed[i].Cell.Occupied = false;
                removed[i].PieceOnBoard = null;
                if (pieceinslot.ContainsKey(removed[i].Instance))
                {
                    pieceinslot.Remove(removed[i].Instance);
                }
     
            }
            slotsOccupied[obj] = new List<IBoardSlot>();
            OnPieceRemoved?.Invoke(obj, removed);
            return obj;

        }

    }

    public interface IBoardSlot
    {
        GameObject Instance { get; set; }
        IInventoryPiece PieceOnBoard { get; set; }
        IVirtualInventoryCell Cell { get; set; }

    }
    /// <summary>
    /// defines a board/grid slot
    /// </summary>
    [System.Serializable]
    public class BoardSlot : IBoardSlot
    {
        public GameObject Instance { get => instance; set => instance = value; }
        public IInventoryPiece PieceOnBoard { get => pieceOnBoard; set => pieceOnBoard = value; }
        public IVirtualInventoryCell Cell { get => cell; set =>cell = value; }

        [Tooltip("the Slot itself")]
        public GameObject instance = default;
        [Tooltip("the thing the slot holds")]
        public IInventoryPiece pieceOnBoard = default;
        [Tooltip("the data")]
        public IVirtualInventoryCell cell = default;

        public BoardSlot(GameObject instance, IVirtualInventoryCell cell, IInventoryPiece piece)
        {
            this.cell = cell;
            this.instance = instance;
            this.pieceOnBoard = piece;
        }


    }

    /// <summary>
    /// helper for preview
    /// </summary>
    [System.Serializable]
    public class BoardPreview
    {
        public List<IBoardSlot> PreviewList = new List<IBoardSlot>();
        public Dictionary<IBoardSlot, GameObject> CellDictionary = new Dictionary<IBoardSlot, GameObject>();
    }

    /// <summary>
    /// the board/grid data
    /// </summary>
    [System.Serializable]
    public class InventoryGrid
    {
        public List<VirtualInventoryCell> Cells = new List<VirtualInventoryCell>();
        public int Rows = 6;
        public int Columns = 9;

    }

}