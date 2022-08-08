
using UnityEngine;


namespace GWLPXL.ARPGCore.Auras.com
{
    /// <summary>
    /// lifetime timer for the AOE auras.
    /// </summary>
    public class AuraTimerArea : MonoBehaviour
    {
        public Aura AuraData;
        public ITakeAura MyUser;
        float timer = 0;
        // Update is called once per frame
        void Update()
        {
            TickTimer();
        }

        private void TickTimer()
        {
            timer += Time.deltaTime;
            if (timer >= AuraData.AuraData.CheckAreaRate)
            {
              AuraData.TryDoAOE(MyUser);
              timer = 0;
            }
        }
    }
}
