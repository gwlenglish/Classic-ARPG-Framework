using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace GWLPXL.ARPGCore.com
{


    [System.Serializable]
    public class UnityARPGActorIni : UnityEvent<IActorHub> { }


    [System.Serializable]
    public class UnityActorEvents
    {
        [Tooltip("Passes the actor's IActorHub after initialization.")]
        public UnityARPGActorIni OnActorInitialized;

    }


    [System.Serializable]
    public class ActorEvents
    {
        public UnityActorEvents SceneEvents = new UnityActorEvents();
    }

    [System.Serializable]
    public class EnemyActorEvents
    {
        public UnityActorEvents SceneEvents = new UnityActorEvents();
    }

    [System.Serializable]
    public class PlayerActorEvents
    {
        public UnityActorEvents SceneEvents = new UnityActorEvents();
    }

}

