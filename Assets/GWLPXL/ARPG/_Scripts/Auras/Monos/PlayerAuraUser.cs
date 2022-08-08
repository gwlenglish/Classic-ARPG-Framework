

using UnityEngine;
using GWLPXL.ARPGCore.com;
namespace GWLPXL.ARPGCore.Auras.com
{
    public interface IScenePersist
    {
        void Load();
        void Save();
    }

    
 


    public class PlayerAuraUser : MonoBehaviour, IUseAura, IScenePersist, ISubscribeEvents
    {

        [SerializeField]
        protected PlayerAuraEvents auraEvents = new PlayerAuraEvents();
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


        protected virtual void ResetSceneAuras()
        {
            Aura[] auras = GetAuraControllerRuntime().GetSceneAuras();
            for (int i = 0; i < auras.Length; i++)
            {
                ToggleAura(auras[i]);
            }
        }

        protected virtual void DisableAurasAndSaveThem()
        {
            Aura[] auras = GetAuraControllerRuntime().GetEquippedAndAppliedAuras();
            for (int i = 0; i < auras.Length; i++)
            {
                GetAuraControllerRuntime().ToggleEquippedAura(auras[i], hub.MyAuraTaker);
            }
            GetAuraControllerRuntime().SetSceneAuras(auras);
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
            if (runtime != null)
            {
                UnSubscribeEvents();
            }

            runtime = controller;
            runtime.TryInitialize();

            if (runtime != null)
            {
                SubscribeEvents();
            }    

        }

        public virtual void Load()
        {
            ResetSceneAuras();
        }

        public virtual void Save()
        {
            DisableAurasAndSaveThem();
        }

        public virtual void SetActorHub(IActorHub newHub)
        {
            hub = newHub;
        }
    }
}