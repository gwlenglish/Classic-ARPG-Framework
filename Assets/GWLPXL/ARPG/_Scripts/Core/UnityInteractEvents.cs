
using UnityEngine;
using UnityEngine.Events;

namespace GWLPXL.ARPGCore.com
{

    [System.Serializable]
    public class UnityActorInteracted : UnityEvent<GameObject> { }

    [System.Serializable]
    public class UnityInteractEvents
    {
        public UnityEvent OnInteract;
        public UnityActorInteracted OnActorInteract;
    }
    [System.Serializable]
    public class ActorInteractEvents
    {
        public UnityInteractEvents SceneEvents = new UnityInteractEvents();
    }

}