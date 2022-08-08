
using UnityEngine;
using UnityEngine.Events;
namespace GWLPXL.ARPGCore.GameEvents.com
{


    public class GameEventListener : MonoBehaviour
    {
        public GameEvent Event;
        public UnityEvent Response;

        private void OnEnable()
        { 
            Event.RegisterListener(this);
        }

        private void OnDisable()
        { 
            Event.UnregisterListener(this);
        }

        public void OnEventRaised()
        { 
            Response.Invoke();
        }
    }
}