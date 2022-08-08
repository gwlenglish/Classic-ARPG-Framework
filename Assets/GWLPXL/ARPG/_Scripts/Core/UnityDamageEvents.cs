using GWLPXL.ARPGCore.GameEvents.com;
using UnityEngine.Events;

namespace GWLPXL.ARPGCore.com
{
    [System.Serializable]
    public class UnityDamageEvent : UnityEvent<int> { }

    [System.Serializable]
    public class UnityHealthEvents
    {
        public UnityDamageEvent OnDamageTaken;
        public UnityEvent OnDie;
    }
    [System.Serializable]
    public class ActorHealthEvents
    {
        public UnityHealthEvents SceneEvents = new UnityHealthEvents();
    }
    [System.Serializable]
    public class PlayerHealthEvents
    {
        public PlayerHealthGameEvents GameEvents;
        public UnityHealthEvents SceneEvents = new UnityHealthEvents();
    }



}