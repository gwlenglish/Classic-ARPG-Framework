using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace GWLPXL.ARPGCore.com
{

    /// <summary>
    /// used for generic timing events
    /// </summary>
    public class TimerEvent : MonoBehaviour, ITick
    {
        public float Duration = 1;
        public bool Loop = false;
        public bool StartImmediately = false;
        public UnityEvent OnTimerStart;
        public UnityEvent OnTimerEnd;
        bool started = false;
        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }
        private void Start()
        {
            AddTicker();
            if (StartImmediately)
            {
                StartTimer();
            }
        }
        private void OnDestroy()
        {
            RemoveTicker();
        }
        public void DoTick()
        {
            if (started == false) return;
            EndTimer();
        }

        public float GetTickDuration()
        {
            return Duration;
        }

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }

        public void StartTimer()
        {
            started = true;
           
            OnTimerStart?.Invoke();
        }

        public void EndTimer()
        {
            OnTimerEnd?.Invoke();
            if (Loop)
            {
                StartTimer();
            }
            else
            {
                started = false;
            }
        }
    }
}