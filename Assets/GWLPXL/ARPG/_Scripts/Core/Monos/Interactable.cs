using GWLPXL.ARPGCore.Looting.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.com
{
    [System.Serializable]
    public class InteractVars
    {
        public float InteractRange = 3;
    }

    public class Interactable : MonoBehaviour, IInteract
    {
        public ActorInteractEvents InteractEvents => interactEvents;
        [SerializeField]
        ActorInteractEvents interactEvents;
        [SerializeField]
        InteractVars interactVars = new InteractVars();

        ILootFX[] lootFX = new ILootFX[0];//required for mouse over, maybe rename to InteractFX

        private void Awake()
        {

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
            interactEvents.SceneEvents.OnInteract.Invoke();
            interactEvents.SceneEvents.OnActorInteract.Invoke(interactor);
            return true;
        }

        public bool IsInRange(GameObject interactor)
        {
            Vector3 dir = interactor.transform.position - this.transform.position;
            float sqrdMag = dir.sqrMagnitude;
            if (sqrdMag <= (interactVars.InteractRange * interactVars.InteractRange))
            {
                return true;
            }
            return false;
        }

       
    }
}