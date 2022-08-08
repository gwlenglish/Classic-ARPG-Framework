
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.com
{
    /// <summary>
    /// OnCollisionEnter interrupt ability
    /// </summary>
    public class AbilityCollisionChecker2D : MonoBehaviour, IInterruptAbilityChecker
    {
        public Ability ToInterrupt;
        public System.Action<Ability> OnInterrupt;
        public LayerMask[] CollisionLayers = new LayerMask[0];
        bool interrupted = false;

        private void Start()
        {
            OnInterrupt += Interrupted;
        }
      
        bool ContainsLayer(LayerMask mask, int layer)
        {
            return ((mask & (1 << layer)) != 0);
        }

        void Interrupted(Ability ability)
        {
            interrupted = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
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

        public void Remove()
        {
            OnInterrupt -= Interrupted;
            Destroy(this);
        }
    }
}