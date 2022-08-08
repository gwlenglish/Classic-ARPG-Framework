using GWLPXL.ARPGCore.PlayerInput.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.com
{

    [RequireComponent(typeof(IInteractPlayerInput))]
    public class InteractMouse3D : MonoBehaviour, ITick
    {
        public void SetLayerMask(LayerMask mask) => interactLayer = mask;
        [SerializeField]
        LayerMask interactLayer;
        [SerializeField]
        float rayDistance = 100;
        IInteractPlayerInput input;

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


        public void AddTicker() => TickManager.Instance.AddTicker(this);
      
        public void DoTick()
        {
            if (input.GetInteracted())
            {
                bool HitInteract = TryInteraction(UnityEngine.Input.mousePosition);


            }
        }

        bool TryInteraction(Vector3 atPosition)
        {
            RaycastHit interactableHit;
            bool hitInteractable = Physics.Raycast(DungeonMaster.Instance.GetMainCamera().ScreenPointToRay(atPosition), out interactableHit, rayDistance, interactLayer);
            if (hitInteractable)
            {

                IInteract _interactable = interactableHit.collider.GetComponent<IInteract>();
                if (_interactable == null) return false;

                if (_interactable.IsInRange(this.gameObject))
                {
                    _interactable.DoInteraction(this.gameObject);
                }

            }
            return hitInteractable;
        }
        public float GetTickDuration() => Time.deltaTime;


        public void RemoveTicker() => TickManager.Instance.RemoveTicker(this);
     

    

    }
}