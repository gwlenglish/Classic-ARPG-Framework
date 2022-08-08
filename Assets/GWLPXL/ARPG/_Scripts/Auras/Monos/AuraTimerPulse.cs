
using UnityEngine;

namespace GWLPXL.ARPGCore.Auras.com
{
    /// <summary>
    /// lifetime timer for the aura pulses
    /// </summary>
    public class AuraTimerPulse : MonoBehaviour
    {
        public Aura AuraData;
        public ITakeAura Myuser;
        float timer = 0;
        // Update is called once per frame
        void Update()
        {
            TickTimer();
        }

        private void TickTimer()
        {
            timer += Time.deltaTime;
            if (timer >= AuraData.AuraData.PulseRate)
            {
                AuraData.TryDoPulse(Myuser);
                timer = 0;
            }
        }
    }
}