
using GWLPXL.ARPGCore.com;
using UnityEngine.Events;

namespace GWLPXL.ARPGCore.Looting.com
{
    [System.Serializable]
    public class UnityDropLootEvent : UnityEvent<ILoot> { }


    [System.Serializable]
    public class UnityLootDroppedEvent : UnityEvent<Loot> { };
    [System.Serializable]
    public class UnitySearchedEvent : UnityEvent<Searchable> { };


    [System.Serializable]
    public class UnityBreakableEvent : UnityEvent<Breakable> { };

    [System.Serializable]
    public class UnitySearchedEvents
    {
        public UnitySearchedEvent OnSearched = new UnitySearchedEvent();
    }
    [System.Serializable]
    public class UnityBreakableEvents
    {
        public UnityBreakableEvent OnBroken = new UnityBreakableEvent();
        public UnityDamageEvent OnDamaged = new UnityDamageEvent();
    }

    [System.Serializable]
    public class UnityLootDropEvents
    {
        public UnityDropLootEvent OnLootDropped = new UnityDropLootEvent();
        public UnityDropLootEvent OnLootPickedUp = new UnityDropLootEvent();

        public UnityEvent OnLootActive = new UnityEvent();
    }

    [System.Serializable]
    public class UnityLootInstanceEvents
    {
        public UnityLootDropEvents SceneEvents = new UnityLootDropEvents();
    }

    [System.Serializable]
    public class UnityLootEvents
    {
        public UnityDropLootEvent OnLootPickedUp = new UnityDropLootEvent();

    }
    [System.Serializable]
    public class ActorDropLootEvents
    {
        public UnityLootDropEvents SceneEvents = new UnityLootDropEvents();
    }


    [System.Serializable]
    public class EnemyDropLootEvents
    {
        public UnityLootDropEvents SceneEvents = new UnityLootDropEvents();
    }

    [System.Serializable]
    public class ActorSearchEvents
    {
        public UnitySearchedEvents SceneEvents = new UnitySearchedEvents();
    }

    [System.Serializable]
    public class PlayerBreakableEvents
    {
        public UnityBreakableEvents SceneEvents = new UnityBreakableEvents();
    }
   
}