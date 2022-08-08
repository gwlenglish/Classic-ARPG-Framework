
using GWLPXL.ARPGCore.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Auras.com
{

    /// <summary>
    /// Just a demo visualization, not really performant for a real game.
    /// </summary>
    public class Demo_AuraCircleVisualizer : MonoBehaviour, ITick
    {
        public Aura ForAura;
        public Material LineMat;

        [Tooltip("Will default to the pulse rate if aura has a pulse")]
        public float Duration = 1;
        public float LineWidth = .25f;
        LineRenderer lineRend = null;
        public int Segments = 360;
        float timer = 0;
        bool ini = false;
        // Start is called before the first frame update

        private void OnEnable()
        {
            AddTicker();
        }
        private void OnDisable()
        {
            RemoveTicker();
        }
        void Start()
        {
            if (ForAura.HasAOE() == false)
            {
                Debug.Log("Visualizer needs an AOE setting otherwise it doesn't really make much sense. Deleting Aura circle");
                Destroy(this);
                return;
            }
      
            lineRend = gameObject.AddComponent<LineRenderer>();
            lineRend.material = LineMat;
            lineRend.useWorldSpace = false;
            lineRend.startWidth = LineWidth;
            lineRend.endWidth = LineWidth;

            if (ForAura.HasPulse())
            {
                Duration = ForAura.AuraData.PulseRate;
            }
            ini = true;
        }

     
        private void UpdateMethod()
        {
            if (ini == false) return;

            if (ForAura == null)
            {
                Debug.LogError("Aura Circle needs an Aura in order to work", this);
                return;
            }
            timer += Time.deltaTime;
            if (timer >= Duration)
            {
                if (ForAura.HasPulse())
                {
                    //if we have a pulse, repeat
                    timer = 0;
                }
                else
                {
                    //if we don't, stay at max value
                    timer = Duration;
                }
            }

            float newRadius = Mathf.Lerp(.1f, ForAura.AuraData.AreaRadius, timer / Duration);
            DrawCircle(newRadius);
        }

        private void DrawCircle(float radius)
        {
            lineRend.positionCount = Segments + 1;
            int pointCount = Segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
            Vector3[] points = new Vector3[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                var rad = Mathf.Deg2Rad * (i * 360f / Segments);
                points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
            }

            lineRend.SetPositions(points);
        }

        #region ticker interface
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
        #endregion
    }
}