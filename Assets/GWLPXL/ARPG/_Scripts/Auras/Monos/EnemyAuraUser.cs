
using GWLPXL.ARPGCore.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Auras.com
{


    public class EnemyAuraUser : MonoBehaviour, IUseAura
    {
        [SerializeField]
        protected EnemyAuraEvents auraEvents = new EnemyAuraEvents();
        [SerializeField]
        protected AuraController AuraControllerTemplate = null;

        protected AuraController runtime = null;

        protected IActorHub hub = null;
        protected virtual void Awake()
        {
            SetRuntimeAuraController(Instantiate(AuraControllerTemplate));


        }
        public virtual void SetTemplate(AuraController newTemplate)
        {
            AuraControllerTemplate = newTemplate;
        }
        public virtual void SubscribeEvents()
        {
            GetAuraControllerRuntime().OnEquippedAura += OnEquipped;
            GetAuraControllerRuntime().OnEquippedAura += OnLearned;
            GetAuraControllerRuntime().OnEquippedAura += OnForgot;

        }

        protected virtual void OnEquipped(Aura aura, int slot)
        {
            auraEvents.SceneEvents.OnAuraEquipped.Invoke(aura);
        }
        protected virtual void OnLearned(Aura aura, int slot)
        {
            auraEvents.SceneEvents.OnAuraLearned.Invoke(aura);
        }
        protected virtual void OnForgot(Aura aura, int slot)
        {
            auraEvents.SceneEvents.OnAuraForgot.Invoke(aura);
        }
        public virtual void UnSubscribeEvents()
        {
            if (GetAuraControllerRuntime() == null) return;
            GetAuraControllerRuntime().OnEquippedAura -= OnEquipped;
            GetAuraControllerRuntime().OnEquippedAura -= OnLearned;
            GetAuraControllerRuntime().OnEquippedAura -= OnForgot;
        }


        public virtual AuraController GetAuraControllerRuntime()
        {
            return runtime;
        }

        public virtual void ToggleAura(Aura aura)
        {
            GetAuraControllerRuntime().ToggleEquippedAura(aura, hub.MyAuraTaker);
        }
        public virtual void ToggleAura(int atEquippedSlot)
        {
            GetAuraControllerRuntime().ToggleEquippedAura(atEquippedSlot, hub.MyAuraTaker);
        }

        public virtual AuraController GetAuraControllerTemplate()
        {
            return AuraControllerTemplate;
        }

        public virtual void SetRuntimeAuraController(AuraController controller)
        {
            runtime = controller;
            runtime.TryInitialize();

        }

        public virtual void SetActorHub(IActorHub newHub)
        {
            hub = newHub;
        }
    }
}