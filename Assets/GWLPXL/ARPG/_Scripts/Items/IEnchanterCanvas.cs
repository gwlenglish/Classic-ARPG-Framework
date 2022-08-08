using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Items.com
{

    public interface IEnchanterCanvas
    {
        void Open(IUseEnchanterCanvas user);
        void Close();
        void Toggle();
        void SetStation(EnchantingStation station);
        bool GetCanvasEnabled();
        bool GetFreezeMover();
    }

   
}