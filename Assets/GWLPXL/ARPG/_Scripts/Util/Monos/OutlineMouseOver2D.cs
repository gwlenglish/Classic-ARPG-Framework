
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.PlayerInput.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Looting.com
{

    /// <summary>
    /// simple class to provide highlights meant for dungeon loot. 
    /// maybe extend later if needed, but probably don't need to. 
    /// </summary>
    ///
    [RequireComponent(typeof(Collider2D))]
    public class OutlineMouseOver2D : MonoBehaviour, ITick, ILootFX
    {
        [SerializeField]
        PlayerMouseEvents mouseoverEvents = new PlayerMouseEvents();
        [SerializeField]
        OutlineMouseVars vars;
        #region fields
        Camera main = null;
        Collider2D coll = null;
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
         
            coll = GetComponent<Collider2D>();
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

            Vector3 worldpoint = main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            Vector2 worldPoint2D = new Vector2(worldpoint.x, worldpoint.y);
            if (coll.bounds.Contains(worldPoint2D))
            {
                Highlight();

            }
            else
            {
                ResetRend();

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