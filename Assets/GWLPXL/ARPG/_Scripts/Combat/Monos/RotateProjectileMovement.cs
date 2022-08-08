using GWLPXL.ARPGCore.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Combat.com
{

    [RequireComponent(typeof(Rigidbody))]
    public class RotateProjectileMovement : MonoBehaviour, ITick
    {
        public float AnglePerSecond = 45;
        public Vector3 Axis = new Vector3(0, 1, 0);
        float tickrate = .02f;
        Rigidbody rb = null;
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            rb.MoveRotation(rb.rotation * Quaternion.AngleAxis(AnglePerSecond * GetTickDuration(), Axis.normalized));
        }

        public float GetTickDuration()
        {
            return tickrate;
        }

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }

        // Start is called before the first frame update
        void Start()
        {
            AddTicker();
        }
        private void OnDestroy()
        {
            RemoveTicker();
        }
       
    }
}