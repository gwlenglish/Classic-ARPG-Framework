using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GWLPXL.ARPGCore.PlayerInput.com
{

    [System.Serializable]
    public class UnityMouseHighlightEvent : UnityEvent<GameObject> { };

    [System.Serializable]
    public class MouseEvents
    {
        public UnityMouseHighlightEvent OnMouseOverEnter = new UnityMouseHighlightEvent();
        public UnityMouseHighlightEvent OnMouseOverExit = new UnityMouseHighlightEvent();
    }
    [System.Serializable]
    public class PlayerMouseEvents
    {
        public MouseEvents SceneEvents = new MouseEvents();
    }

    

}