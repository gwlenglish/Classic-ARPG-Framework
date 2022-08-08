
using System.Collections.Generic;
using UnityEngine;
namespace GWLPXL.ARPGCore.com
{
    //maintains anything that ticks, enemy movement, dots, anything with controlled time

    public class TickManager : MonoBehaviour, ITickManager
    {
        public static TickManager Instance => instance;
        public bool Paused { get; set; }
        static TickManager instance;
        List<ITick> backingList = new List<ITick>();
        [Tooltip("Scene elements that are dependant on time. Useful when wanting to pause the scene. Also plugs into the DoT system.")]
        List<TickCounter> tickers = new List<TickCounter>();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }

        }

        public void AddTicker(ITick newTick)
        {
            if (backingList.Contains(newTick) == false)
            {
                TickCounter newTickCounter = new TickCounter(newTick);
                tickers.Add(newTickCounter);
                backingList.Add(newTick);
            }
        }

        public void DoTicks()
        {
            if (Paused) return;
            float _currentTime = 0;
            for (int i = 0; i < tickers.Count; i++)
            {
                //order is important here
                _currentTime = tickers[i].ThisTImer;
                _currentTime += Time.deltaTime;
                tickers[i].ThisTImer = _currentTime;
                if (_currentTime >= tickers[i].ThisTick.GetTickDuration())
                {
                    //we must set all the time before we tick, because the tick mgiht result in a destroy
                    tickers[i].ThisTImer -= tickers[i].ThisTick.GetTickDuration();
                    //tickers[i].ThisTImer = 0;
                    tickers[i].ThisTick.DoTick();
                }
                
            }



        }

        public void RemoveTicker(ITick remove)
        {
            if (backingList.Contains(remove))
            {
                for (int i = 0; i < tickers.Count; i++)
                {
                    if (tickers[i].ThisTick == remove)
                    {
                        tickers.RemoveAt(i);
                    }
                }

                backingList.Remove(remove);
            }
        }

        public void ClearTickers()
        {
            for (int i = 0; i < backingList.Count; i++)
            {
                if (backingList[i] == null)
                {
                    backingList.RemoveAt(i);
                }
            }
            for (int i = 0; i < tickers.Count; i++)
            {
                if (tickers[i].ThisTick == null)
                {
                    tickers.RemoveAt(i);
                }
            }
        }
    }

    #region helper classes

    [System.Serializable]
    public class TickCounter
    {
        public ITick ThisTick { get; set; }
        public float ThisTImer { get; set; }
        public TickCounter(ITick _newTicker)
        {
            ThisTick = _newTicker;
            ThisTImer = 0;

        }
    }
    #endregion
}