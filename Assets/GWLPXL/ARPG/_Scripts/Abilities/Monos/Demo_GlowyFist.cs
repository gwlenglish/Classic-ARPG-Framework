
using GWLPXL.ARPGCore.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.com
{

    /// <summary>
    /// only an example.
    /// </summary>
    public class Demo_GlowyFist : MonoBehaviour, ITick
    {
        public float LifeTime = 1;
        float timer = 0;
        bool ini;

        private void Start()
        {
            ini = true;
        }

        private void UpdateMethod()
        {
            if (ini == false) return;

            timer += Time.deltaTime;
            if (timer >= LifeTime)
            {
                Destroy(this.gameObject);
                ini = false;
            }
        }

      

        private void OnEnable()
        {
            AddTicker();
        }

        private void OnDisable()
        {
            RemoveTicker();
        }


        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            UpdateMethod();
        }

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }

        public float GetTickDuration()
        {
            return Time.deltaTime;
        }
    }
}