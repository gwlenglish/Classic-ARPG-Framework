
using UnityEngine;
using UnityEngine.Events;
namespace GWLPXL.ARPGCore.com
{

    [RequireComponent(typeof(Collider2D))]
    public class OnInteract2D : MonoBehaviour, IInteract
    {
        public UnityEvent OnInteract;
        public float InteractRange = 1;
        public bool DoInteraction(GameObject interactor)
        {
            OnInteract.Invoke();
            return true;
        }

        public bool IsInRange(GameObject interactor)
        {
            Vector2 dir = (Vector2)interactor.transform.position - (Vector2)this.transform.position;
            float sqrdMag = dir.sqrMagnitude;
            return (sqrdMag <= (InteractRange * InteractRange));
        }

       
    }
}