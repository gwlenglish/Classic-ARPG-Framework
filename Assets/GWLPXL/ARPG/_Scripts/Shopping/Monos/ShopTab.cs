using GWLPXL.ARPGCore.Types.com;
using TMPro;
using UnityEngine;

namespace GWLPXL.ARPGCore.Shopping.com
{


    public class ShopTab : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI text = null;
        ItemType forTypes;
        IShopKeeperCanvas canvas = null;

        public void SetTypes(ItemType type, IShopKeeperCanvas canvas)
        {
            forTypes = type;
            this.canvas = canvas;
            text.SetText(type.ToString());
        }
        public void EnableTab()
        {
            canvas.DisplayItems(new ItemType[1] { forTypes });
        }

    }
}