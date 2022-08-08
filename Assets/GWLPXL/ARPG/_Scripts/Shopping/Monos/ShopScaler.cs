using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Leveling.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Shopping.com
{

    [RequireComponent(typeof(ShopKeeper))]
    public class ShopScaler : MonoBehaviour, IScale
    {
        IShopKeeper shop = null;
        IActorHub hub = null;
        private void Awake()
        {
            shop = GetComponent<IShopKeeper>();
        }
        public int GetUNScaledLevel()
        {
            return shop.GetShopLevel();
        }

        public int GetScaledLevel()
        {
            return Formulas.GetShopLevel(GetUNScaledLevel());
        }

        public void SetUNScaledLevel(int unscaled)
        {
            shop.SetShopLevel(unscaled);
        }

        public void SetActorHub(IActorHub newHub) => hub = newHub;
        
    }
}