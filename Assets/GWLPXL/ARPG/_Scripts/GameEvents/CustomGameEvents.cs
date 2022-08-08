using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.GameEvents.com
{

    /// <summary>
    /// Non-scene specific unique events, typically for players
    /// </summary>///move to the attributes?
    [System.Serializable]
    public class PlayerHealthGameEvents
    {
        [Tooltip("Custom Game Event specific, non scene specific")]
        public TookDamageEvent TookDamageEvent;
        [Tooltip("Custom Game Event specific, non scene specific")]
        public DeathEvent DeathEvent;
    }

}