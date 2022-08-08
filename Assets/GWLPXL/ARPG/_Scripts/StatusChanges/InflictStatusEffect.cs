

using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.StatusEffects.com
{
  
    /// <summary>
    /// used to quickly test status effects by attaching the mono to the root of the actor.
    /// </summary>
    public class InflictStatusEffect : MonoBehaviour
    {
        public StatusEffectSO Effect;
        
        void Start()
        {
            IActorHub statusr = GetComponent<IActorHub>();
            if (statusr != null)
            {
                StatusEffectHelper.ApplyEffect(statusr, Effect.Vars);
            }

        }

       
    }
}