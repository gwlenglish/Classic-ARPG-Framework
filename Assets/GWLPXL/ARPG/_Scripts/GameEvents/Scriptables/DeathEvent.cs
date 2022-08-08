
using UnityEngine;

namespace GWLPXL.ARPGCore.GameEvents.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/GameEvents/NEW_Death")]
    public class DeathEvent : GameEvent
    {
        public GameObject DiedObj { get; set; }
    }
}
