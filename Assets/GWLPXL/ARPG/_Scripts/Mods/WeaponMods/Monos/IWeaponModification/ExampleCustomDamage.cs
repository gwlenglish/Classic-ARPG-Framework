
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using UnityEngine;


namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
 
    /// <summary>
    /// just an example to show how to write your own custom damage
    /// </summary>
    public class ExampleCustomDamage : MonoBehaviour, IWeaponModification
    {

        bool active = false;
        IActorHub self = null;
        public void DoModification(AttackValues other)
        {
            Debug.Log("Custom Mod");
            if (IsActive() == false) return;
            ///do the custom damage formula here.
            ///the attacker is self, the defender is other
            float damageamount = 4 * self.MyStats.GetRuntimeAttributes().GetStatNowValue(Types.com.StatType.Strength) + 5;
            int rounded = Mathf.FloorToInt(damageamount);
            Debug.Log("Expected custom damage: " + rounded);
            other.PhysicalAttack.Add(new PhysicalAttackResults(rounded, "Custom Example"));
            //pass the damage to the other

        }

        public bool DoChange(Transform other)
        {
            return false;
        }

        public Transform GetTransform() => this.transform;

        public bool IsActive() => active;

        public void SetActive(bool isEnabled)
        {
            active = isEnabled;
        }

        public void SetUser(IActorHub myself) => self = myself;
      
       
    }
}