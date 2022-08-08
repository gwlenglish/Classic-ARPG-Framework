using GWLPXL.ARPGCore.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace GWLPXL.ARPGCore.Combat.com
{
   
    /// <summary>
    /// used to expose the log to the inspector for the time being
    /// </summary>
    public class CombatLog : MonoBehaviour
    {
        [Tooltip("Limit to combat results print")]
        public int Limit = 100;
        [Tooltip("Read only log of combat results.")]
        [SerializeField]
        protected List<CombatResults> log;
        private void OnEnable()
        {
            CombatLogger.OnResultAdded += AddLog;
        }

        private void OnDisable()
        {
            CombatLogger.OnResultAdded -= AddLog;

        }

        void AddLog(CombatResults results)
        {
            if (log.Count > Limit && Limit > 0)
            {
                log.RemoveAt(0);
            }
            log.Add(results);
        }

      
    }


    /// <summary>
    /// temp log for now to see results, eventually translate results into a string.
    /// </summary>
    public static class CombatLogger
    {
        public static event Action<CombatResults> OnResultAdded;

        static List<CombatResults> log = new List<CombatResults>();
        public static void AddResult(CombatResults results)
        {
            log.Add(results);
            OnResultAdded?.Invoke(results);
        }





    }
}