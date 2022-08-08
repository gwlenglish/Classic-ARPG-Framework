
using UnityEngine;

namespace GWLPXL.ARPGCore.com
{


    public class AliveTime : MonoBehaviour, ITick
    {
        [SerializeField]
        float Duration = 10;

        private void Start()
        {
            AddTicker();
        }
        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        private void OnDestroy()
        {
            RemoveTicker();
        }
        public void DoTick()
        {
            Destroy(this.gameObject);
        }

        public float GetTickDuration()
        {
            return Duration;
        }

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }

       
    }
}
