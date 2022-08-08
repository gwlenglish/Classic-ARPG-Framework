
using GWLPXL.ARPGCore.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Shopping.com
{


    public abstract class ShopRequirement : ScriptableObject
    {
        public abstract bool MeetsRequirement(IActorHub shopper, IShopKeeper shop);
        public abstract string GetDescription();
    }
}