
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Util.com
{
  


    public class AlignCamera : MonoBehaviour, ITick
    {
        public float ShowDistance = 20f;
        [Tooltip("Defines your own update rate for the alignment. For moving objects, use delta time or a number lower than fixed update for smooth results (.02f).")]
        public float UpdateRate = .02f;
        [Tooltip("If enabled, will ignore UpdateRate and default to deltaTime.")]
        public bool UseDeltaTime = false;
        [Tooltip("Use 0 in the X rotation for alignment. Might be useful for different camera placements.")]
        public bool ZeroLookRotX = true;
        [Tooltip("Use 0 in the Z rotation for alignment.Might be useful for different camera placements.")]
        public bool ZeroLookRotY = true;

        public float ZOffset = 0;
        public float XOffset = 0;
        public float YOffset = 1;

        protected  Camera mainCamera = null;
        protected IResourceBar[] resourceBars = new IResourceBar[0];
        protected ILabel[] labels = new ILabel[0];
        protected bool hasdmg = false;
        protected IReceiveDamage damagereceiver;
        protected virtual void Awake()
        {
            mainCamera = Camera.main;
            resourceBars = GetComponentsInChildren<IResourceBar>();
            labels = GetComponentsInChildren<ILabel>();
            transform.localPosition = new Vector3(XOffset, YOffset, ZOffset);
            damagereceiver = GetComponentInParent<IReceiveDamage>();
            if (damagereceiver != null)
            {
                hasdmg = true;
            }
        }

        protected virtual void Start()
        {
            AddTicker();
            DoTick();//force a tick for the first update
        }
        protected virtual void OnDestroy()
        {
            RemoveTicker();
        }
        public virtual void AddTicker()
        {
            TickManager.Instance.AddTicker(this);

        }

     
        public virtual void DoTick()
        {
            if (hasdmg == true && damagereceiver.IsDead())
            {
                gameObject.SetActive(false);
                return;
            }
            Vector3 worldpos = transform.position;
            Vector3 camCenter = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
            Vector3 dir = worldpos - camCenter;
            float sqrdMag = dir.sqrMagnitude;
            if (sqrdMag < ShowDistance * ShowDistance)
            {
                gameObject.SetActive(true);
                AlignToCamera();
                for (int i = 0; i < resourceBars.Length; i++)
                {
                    resourceBars[i].UpdateBar();
                }
                for (int i = 0; i < labels.Length; i++)
                {
                    labels[i].UpdateLabel();
                }

            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        //doesn't work with cameras straight down
        protected virtual void AlignToCamera()
        {
            if (mainCamera == null) return;//can't align without camera
            transform.localPosition = new Vector3(XOffset, YOffset, ZOffset);
            Transform camXform = mainCamera.transform;
                Vector3 forward = transform.position - camXform.position;
                

            if (ZeroLookRotX)
            {
                forward.x = 0;
            }
            if (ZeroLookRotY)
            {
                forward.y = 0;

            }
            forward.Normalize();
            Vector3 up = Vector3.Cross(forward, camXform.right);
            transform.rotation = Quaternion.LookRotation(forward, up);
            
        }

        public virtual float GetTickDuration()
        {
            if (UseDeltaTime) return Time.deltaTime;
            return UpdateRate;
        }

        public virtual void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);

        }
    }
}