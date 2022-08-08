
using UnityEngine;
using UnityEngine.Events;

namespace GWLPXL.ARPGCore.com
{

    [RequireComponent(typeof(Collider))]
    public class OnInteract3D : MonoBehaviour, IInteract
    {
        public UnityEvent OnInteractEvent;
        public float InteractRange = 1;
        public bool DoInteraction(GameObject interactor)
        {
            OnInteractEvent.Invoke();
            return true;
        }

        public bool IsInRange(GameObject interactor)
        {
            Vector3 dir = interactor.transform.position - this.transform.position;
            float sqrdMag = dir.sqrMagnitude;
            return (sqrdMag <= (InteractRange * InteractRange));
        }

    }
}