using GWLPXL.ARPGCore.Types.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GWLPXL.ARPGCore.StatusEffects.com
{
    [System.Serializable]
    public class UnityReceiveModifyResource : UnityEvent<int, ResourceType, ElementType> { }

    [System.Serializable]
    public class UnityOnSOTApplyEvent : UnityEvent<IRecieveStatusChanges> { }

    [System.Serializable]
    public class UnityOnStatusEffectApplied : UnityEvent<StatusEffectVars> { }
    [System.Serializable]
    public class UnityOnStatusEffectRemove : UnityEvent<StatusEffectVars> { }

    [System.Serializable]
    public class UnityStatusEffectsReceiverEvents
    {
        [Header("Resources")]
        public UnityReceiveModifyResource OnRegenResource;
        public UnityReceiveModifyResource OnReduceResource;
        public UnityOnStatusEffectApplied OnStatusEffectApplied;
        public UnityOnStatusEffectRemove OnStatusEffectRemoved;


    }

    [System.Serializable]
    public class UnitySoTEvents
    {
        public UnityOnSOTApplyEvent OnSoTApply;
    }

    [System.Serializable]
    public class ActorReceiverStatusEffects
    {
        public UnityStatusEffectsReceiverEvents SceneEvent;
    }
    [System.Serializable]
    public class PlayerReceiverStatusEffects
    {
        public UnityStatusEffectsReceiverEvents SceneEvent;
    }
    [System.Serializable]
    public class EnemyReceiverStatusEffects
    {
        public UnityStatusEffectsReceiverEvents SceneEvent;
    }

   [System.Serializable]
   public class EnvironmentSotEvents
    {
        public UnitySoTEvents SceneEvents;
    }
    [System.Serializable]
    public class DamageSourceSOTEvents
    {
        public UnitySoTEvents SceneEvents;
    }


}
