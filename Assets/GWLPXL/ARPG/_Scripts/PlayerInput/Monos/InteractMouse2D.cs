using GWLPXL.ARPGCore.PlayerInput.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.com
{


    public class InteractMouse2D : MonoBehaviour, ITick
    {
        public GameObject ActorHub = null;
        public void SetLayerMask(LayerMask mask) => interactLayer = mask;
        [SerializeField]
        LayerMask interactLayer;
        //[SerializeField]
        //float rayDistance = 100;
        IInteractPlayerInput input;

        public void AddTicker() => TickManager.Instance.AddTicker(this);
      
        public void DoTick()
        {
            if (input.GetInteracted())
            {
                Vector2 mousePosition = UnityEngine.Input.mousePosition;

                Vector2 v = Camera.main.ScreenToWorldPoint(mousePosition);

                Collider2D[] col = Physics2D.OverlapPointAll(v, interactLayer);

                if (col.Length == 0) return;

                for (int i = 0; i < col.Length; i++)
                {
                    IInteract interactable = col[i].GetComponent<IInteract>();
                    if (interactable == null) continue;
                    if (interactable.IsInRange(ActorHub))
                    {
                        interactable.DoInteraction(ActorHub);
                        break;//only one interactable at a time
                    }
                }
               

            }
        }

        public float GetTickDuration() => Time.deltaTime;


        public void RemoveTicker() => TickManager.Instance.RemoveTicker(this);
     

        private void Awake()
        {
            input = GetComponent<IInteractPlayerInput>();
        }

        private void Start()
        {
            AddTicker();
        }
        private void OnDestroy()
        {
            RemoveTicker();
        }
    }
}