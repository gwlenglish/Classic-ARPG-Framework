
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.com
{
    public class GenericTimer : ITick
    {
        public System.Action OnComplete;
        float duration;
        public GenericTimer(float duration, System.Action onComplete = null)
        {
            if (onComplete != null)
            {
                OnComplete += onComplete;
            }
      
            this.duration = duration;
            AddTicker();
        }

        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            OnComplete?.Invoke();
            RemoveTicker();
        }

        public float GetTickDuration() => duration;
       

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }
    }
}