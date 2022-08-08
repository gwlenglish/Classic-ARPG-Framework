
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.PlayerInput.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Looting.com
{
    public interface ILootFX
    {
        void EnableFX();
        void DisableFX();
    }
    [System.Serializable]
    public class OutlineMouseVars
    {
        public LayerMask Layermask;
        public Transform ParentofOriginalMesh = null;
        public Transform ParentofHighlightMesh = null;
        public OutlineMouseVars(Transform parentoriginal, Transform parenthighlight)
        {
            ParentofOriginalMesh = parentoriginal;
            ParentofHighlightMesh = parenthighlight;
        }
    }
    /// <summary>
    /// simple class to provide highlights meant for dungeon loot. 
    /// maybe extend later if needed, but probably don't need to. 
    /// </summary>
    ///
    [RequireComponent(typeof(Collider))]
    public class OutlineMouseOver : MonoBehaviour, ITick, ILootFX
    {
        [SerializeField]
        PlayerMouseEvents mouseoverEvents = new PlayerMouseEvents();
        [SerializeField]
        OutlineMouseVars vars;
        #region fields
        Camera main = null;
        Collider coll = null;
        bool enableHighlight = false;
        float tickRate = .02f;
        bool hasHighlightversion = false;
        #endregion

        public void SetVars(OutlineMouseVars newvars)
        {
            vars = newvars;
        }
       
        #region unity calls
        private void Awake()
        {
         
            coll = GetComponent<Collider>();
            main = Camera.main;
            if (vars != null && vars.ParentofHighlightMesh != null && vars.ParentofOriginalMesh != null)
            {
                hasHighlightversion = true;
                vars.ParentofHighlightMesh.gameObject.SetActive(false);
                vars.ParentofOriginalMesh.gameObject.SetActive(true);
            }

          
        }

        private void Start()
        {
            AddTicker();
        }

        private void OnDestroy()
        {
            RemoveTicker();
        }


        #endregion

     

        void CheckHighlight()
        {
            if (hasHighlightversion == false) return;
            if (enableHighlight == false) return;
            Ray ray = main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, vars.Layermask))
            {
                if (coll.bounds.Contains(hit.point))
                {
                    Highlight();
                }
                else
                {
                    ResetRend();
                }

            }
        }

        void Highlight()
        {
            if (hasHighlightversion == false) return;
            if (vars.ParentofHighlightMesh.gameObject.activeInHierarchy) return;
            vars.ParentofHighlightMesh.gameObject.SetActive(true);
            vars.ParentofOriginalMesh.gameObject.SetActive(false);
            mouseoverEvents.SceneEvents.OnMouseOverEnter.Invoke(this.gameObject);
           // Debug.Log("Hit");
        }

        void ResetRend()
        {
            if (hasHighlightversion == false) return;
            if (!vars.ParentofHighlightMesh.gameObject.activeInHierarchy) return;
            vars.ParentofHighlightMesh.gameObject.SetActive(false);
            vars.ParentofOriginalMesh.gameObject.SetActive(true);
            mouseoverEvents.SceneEvents.OnMouseOverExit.Invoke(this.gameObject);
            //  Debug.Log("Miss");

        }

        #region interface

        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            CheckHighlight();
        }

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);

        }

        public float GetTickDuration()
        {
            return tickRate;
        }

        public void EnableFX()
        {
            if (hasHighlightversion == false) return;
            enableHighlight = true;
        }

        public void DisableFX()
        {
            if (hasHighlightversion == false) return;
            enableHighlight = false;
            vars.ParentofHighlightMesh.gameObject.SetActive(false);
        }
        #endregion
    }
}