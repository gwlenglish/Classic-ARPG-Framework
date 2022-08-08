using GWLPXL.ARPGCore.Types.com;
using UnityEngine;
using UnityEngine.Events;

namespace GWLPXL.ARPGCore.Auras.com
{
    //casters 
    [System.Serializable]
    public class UnityAuraEvent : UnityEvent<Aura> { }


    [System.Serializable]
    public class UnityAuraEvents
    {
        public UnityAuraEvent OnAuraEquipped;
        public UnityAuraEvent OnAuraLearned;
        public UnityAuraEvent OnAuraForgot;

    }
   
    [System.Serializable]
    public class PlayerAuraEvents
    {
        public UnityAuraEvents SceneEvents = new UnityAuraEvents();
    }
    [System.Serializable]
    public class EnemyAuraEvents
    {
        public UnityAuraEvents SceneEvents = new UnityAuraEvents();
    }

    //receivers
    [System.Serializable]
    public class UnityAuraReceiveResourceEvent : UnityEvent<ResourceType, int> { }
    [System.Serializable]
    public class UnityAuraReceiveStatEvent : UnityEvent<StatType, int> { }
    [System.Serializable]
    public class UnityAuraReceiveEvents
    {
        [Header("Resources")]
        public UnityAuraReceiveResourceEvent OnReceiveResourceCurrent;
        public UnityAuraReceiveResourceEvent OnReceiveResourceAuraMax;
        [Header("Stats")]
        public UnityAuraReceiveStatEvent OnReceiveStatAura;

    }
    [System.Serializable]
    public class EnemyAuraReceiveEvents
    {
        public UnityAuraReceiveEvents SceneEvents = new UnityAuraReceiveEvents();
    }

    [System.Serializable]
    public class PlayerAuraReceiveEvents
    {
        public UnityAuraReceiveEvents SceneEvents = new UnityAuraReceiveEvents();
    }
}