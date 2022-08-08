using GWLPXL.ARPGCore.Types.com;

using UnityEngine;
namespace GWLPXL.ARPGCore.Items.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Currency/NEW_")]
    public class Currency : Item
    {
        public int Amount = 100;
        public string CurrencyName = "Gold";

        ItemType type = ItemType.Currency;
        int stackingamount = 0;
        bool isStacking = false;

        public override string GetBaseItemName()
        {
            return CurrencyName;
        }

        public override string GetGeneratedItemName()
        {
            return Amount.ToString() + " " + CurrencyName; 
        }

        public override ItemType GetItemType() => type;


        public override int GetStackingAmount() => stackingamount;


        public override string GetUserDescription()
        {
            return GetGeneratedItemName();
        }


        public override bool IsStacking() => isStacking;



        public override void SetGeneratedItemName(string newName) => CurrencyName = newName;
       
    }
}