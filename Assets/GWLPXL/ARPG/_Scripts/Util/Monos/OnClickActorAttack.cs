
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Looting.com;
using GWLPXL.ARPGCore.Types.com;
using TMPro;
using UnityEngine;

namespace GWLPXL.ARPGCore.Demo.com
{
    /// <summary>
    /// for the bare demo scene
    /// </summary>
    public class OnClickActorAttack : BareClass, IMouseClickable
    {
        public GameObject Attacker;
        public GameObject Defender;
        public TextMeshProUGUI MessageText;
        public void DoClick()
        {
            //get the attacker stats based on interface, we use strength as the attack damage
            IAttributeUser getAttackerStats = Attacker.GetComponent<IAttributeUser>();
            int strength = getAttackerStats.GetRuntimeAttributes().GetStatNowValue(StatType.Strength);

            //get the defender stats based on interface, 
            IAttributeUser defenderStats = Defender.GetComponent<IAttributeUser>();
            int currenthealth = defenderStats.GetRuntimeAttributes().GetResourceNowValue(ResourceType.Health);

            //update the defender health, in this case strength is the attack damage so we subtract that mount 
            defenderStats.GetRuntimeAttributes().ModifyNowResource(ResourceType.Health, -strength);

            string message = Attacker.name + " attacked for " + strength.ToString() + "\n" + Defender.name +
                " had " + currenthealth + " health BUT now has " +
                defenderStats.GetRuntimeAttributes().GetResourceNowValue(ResourceType.Health) + " health";
            Debug.Log(message); 
            MessageText.text = message;

            if (defenderStats.GetRuntimeAttributes().GetResourceNowValue(ResourceType.Health) <= 0)
            {
                //simulate death
                IDropLoot dropLoot = GetComponent<IDropLoot>();
                if (dropLoot != null)
                {
                    dropLoot.DropLoot();
                }
            }

        }
    }
}
