using GWLPXL.ARPGCore.PlayerInput.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GWLPXL.ARPGCore.com
{


    public class InteractFacing2D : MonoBehaviour, ITick
    {
        public GameObject ActorHub = null;
        public void SetLayerMask(LayerMask mask) => interactLayer = mask;
        [SerializeField]
        protected LayerMask interactLayer;
        [SerializeField]
        protected float range = 3;
        //[SerializeField]
        //float rayDistance = 100;
        protected IInteractPlayerInput input;
        protected IActorHub hub;
        protected virtual void Awake()
        {
            hub = ActorHub.GetComponent<IActorHub>();
            input = GetComponent<IInteractPlayerInput>();
        }

        protected virtual void Start()
        {
            AddTicker();
        }
        protected virtual void OnDestroy()
        {
            RemoveTicker();
        }
        public void AddTicker() => TickManager.Instance.AddTicker(this);

        public void DoTick()
        {
            CheckInteraction();
        }

        protected virtual void CheckInteraction()
        {
            if (input.GetInteracted())
            {
                Vector3 facing = hub.MyStateMachine.Get2D().GetFacingVector();
                Vector2 v = hub.MyTransform.position + (facing * range);


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


    }
}
