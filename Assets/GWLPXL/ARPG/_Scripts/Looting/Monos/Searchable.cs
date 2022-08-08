
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Leveling.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Looting.com
{
    [System.Serializable]
    public class SearchableVars
    {
        public Transform ParentofOriginalMesh = null;
        public Transform ParentofDropLocation = null;
        public float Delay = .25f;
        public float SearchRange = 3;

        public SearchableVars(Transform parentoriginal, Transform parentdrop, float searchdistance)
        {
            ParentofOriginalMesh = parentoriginal;
            ParentofDropLocation = parentdrop;
            SearchRange = searchdistance;
        }
    }

    public class Searchable : MonoBehaviour, IInteract
    {
        public ActorSearchEvents SearchEvents => searchEvents;
        [SerializeField]
        ActorSearchEvents searchEvents = new ActorSearchEvents();
        [SerializeField]
        SearchableVars vars;

        bool searched = false;
        IDropLoot[] droploot = new IDropLoot[0];
        ILootFX[] lootFX = new ILootFX[0];

        public void SetVars(SearchableVars newvars)
        {
            vars = newvars;
        }
        private void Awake()
        {
            droploot = GetComponents<IDropLoot>();
            lootFX = GetComponents<ILootFX>();
           
            
        }
        private void Start()
        {
            for (int i = 0; i < lootFX.Length; i++)
            {
                lootFX[i].EnableFX();
            }
        }
        public bool DoInteraction(GameObject interactor)
        {
            if (searched) return false;
            searched = true;
            for (int i = 0; i < droploot.Length; i++)
            {
                droploot[i].DropLoot();
            }
            for (int i = 0; i < lootFX.Length; i++)
            {
                lootFX[i].DisableFX();
            }
            vars.ParentofOriginalMesh.gameObject.SetActive(true);
            searchEvents.SceneEvents.OnSearched.Invoke(this);
            return searched;
        }

       

        public bool IsInRange(GameObject interactor)
        {
            Vector3 dir = interactor.transform.position - this.transform.position;
            float sqrdMag = dir.sqrMagnitude;
            if (sqrdMag <= (vars.SearchRange * vars.SearchRange))
            {
                return true;
            }
            return false;
        }

      
    }
}