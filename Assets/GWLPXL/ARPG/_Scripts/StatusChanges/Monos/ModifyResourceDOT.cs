

using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.StatusEffects.com
{
    /// <summary>
    /// generic DOT, damages over time but can also heal with negatie numbers. Attach to actor root to give effect at start.
    /// </summary>
    public class ModifyResourceDOT : MonoBehaviour
    {
        public ModifyResourceVars Master;
        IActorHub target;


        private void Awake()
        {
            target = GetComponent<IActorHub>();
        }

        private void Start()
        {
            if (target != null && target.MyStatusEffects != null)
            {
                SoTHelper.AddDoT(target, Master);
                //target.MyStatusEffects.AddDoT(Master);
            }

        }

    }
}
