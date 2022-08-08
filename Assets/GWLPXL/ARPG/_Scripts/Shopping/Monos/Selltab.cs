using GWLPXL.ARPGCore.Types.com;
using TMPro;
using UnityEngine;

namespace GWLPXL.ARPGCore.Shopping.com
{


    public class Selltab : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI text = null;
        ItemType forTypes;
        ISellerCanvasUI canvas = null;

        public void SetTypes(ItemType type, ISellerCanvasUI canvas)
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