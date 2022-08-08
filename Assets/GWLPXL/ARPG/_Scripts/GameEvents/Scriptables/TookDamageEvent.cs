using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.GameEvents.com
{
    public struct DamageEvent
    {
        public IReceiveDamage DamageTaker { get; set; }
        public int DamageAmount { get; set; }
        public ElementType EleType { get; set; }
        public string Text { get; set; }
        public Vector3 Position { get; set; }

        public DamageEvent(IReceiveDamage damageT, int damageAmount, ElementType type, string text, Vector3 pos)
        {
            DamageTaker = damageT;
            DamageAmount = damageAmount;
            EleType = type;
            Text = text;
            Position = pos;
        }
    }
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/GameEvents/NEW_TookDamage")]
    public class TookDamageEvent : GameEvent
    {
        public DamageEvent EventVars;
      
    }
}
