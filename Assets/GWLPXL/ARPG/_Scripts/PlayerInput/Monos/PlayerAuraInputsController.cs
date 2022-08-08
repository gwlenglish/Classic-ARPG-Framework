using GWLPXL.ARPGCore.Auras.com;
using GWLPXL.ARPGCore.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.PlayerInput.com
{
    /// <summary>
    /// handles the aura input. 
    /// </summary>
    public class PlayerAuraInputsController : MonoBehaviour, ITick
    {
        IPlayerAuraInput auraInput = null;
        IUseAura auraUser = null;
        bool hasAuras;

        private void Awake()
        {
            auraInput = GetComponent<IPlayerAuraInput>();
            auraUser = GetComponent<IUseAura>();

            if (auraInput != null && auraUser != null)
            {
                hasAuras = true;
            }
          
        }


        private void Start()
        {
            AddTicker();
        }

        private void OnDestroy()
        {
            RemoveTicker();
        }
        public void AddTicker() => TickManager.Instance.AddTicker(this);


        public void DoTick()
        {
            if (hasAuras)
            {
                Aura aura = auraInput.GetFirstAuraToggle();
                if (aura != null)
                {
                    auraUser.ToggleAura(aura);
                }
            }

        }

        public float GetTickDuration() => Time.deltaTime;


        public void RemoveTicker() => TickManager.Instance.RemoveTicker(this);
    }
}