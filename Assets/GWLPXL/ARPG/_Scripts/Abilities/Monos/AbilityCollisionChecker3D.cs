
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.com
{

 
    /// <summary>
    /// Checks OnCollisionEnter
    /// </summary>
    public class AbilityCollisionChecker3D : MonoBehaviour, IInterruptAbilityChecker
    {
        public Ability ToInterrupt;
        public System.Action<Ability> OnInterrupt;
        public LayerMask[] CollisionLayers = new LayerMask[0];
        bool interrupted = false;

        protected virtual void Start()
        {
            OnInterrupt += Interrupted;
        }

        protected virtual bool ContainsLayer(LayerMask mask, int layer)
        {
            return ((mask & (1 << layer)) != 0);
        }

        protected virtual void Interrupted(Ability ability)
        {
            interrupted = true;
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            if (interrupted) return;
            if (CollisionLayers.Length == 0) return;
            for (int i = 0; i < CollisionLayers.Length; i++)
            {
                if (ContainsLayer(CollisionLayers[i], collision.gameObject.layer))
                {
                    //raise interrupt
                    OnInterrupt.Invoke(ToInterrupt);
                    break;
                }
            }
        }
       

        public virtual void Remove()
        {
            OnInterrupt -= Interrupted;
            Destroy(this);
        }
    }
}