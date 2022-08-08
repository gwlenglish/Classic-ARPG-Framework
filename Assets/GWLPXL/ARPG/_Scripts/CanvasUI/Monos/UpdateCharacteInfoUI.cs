
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Statics.com;
using TMPro;
using UnityEngine;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
    //for the player

    public class UpdateCharacteInfoUI : MonoBehaviour, IDescribePlayerStats
    {
        [SerializeField]
        TextMeshProUGUI text;

        public virtual void DisplayStats(IActorHub user)
        {

            text.text = PlayerDescription.GetCharacterInfoDescription(user);

        }


    }
}
