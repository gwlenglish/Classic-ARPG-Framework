using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Factions.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.Shopping.com
{
    [CreateAssetMenu(menuName ="GWLPXL/ARPG/Shopping/Faction Requirement")]
    public class FactionRequirement : ShopRequirement
    {
        public FactionRequirementData Data;

        public override string GetDescription() => Data.Description;
       
        public override bool MeetsRequirement(IActorHub shopper, IShopKeeper shop)
        {
            IFactionMember member = shopper.MyFaction;

            IFactionMember shopmember = shop.GetInstance().GetComponent<IFactionMember>();
            if (member== null || shopmember == null)
            {
                Debug.LogWarning("Both SHOPPER and SHOP must inherit from Ifactionmember in order to have a faction requirement. One or both are missing.");
                return false;
            }

            if (member.GetFactionRep(shopmember.GetFaction()) >= Data.MinRelationshipValue)
            {
                //we have enough rep
                return true;
            }
            else
            {
                return false;
            }
            

        }
    }
}