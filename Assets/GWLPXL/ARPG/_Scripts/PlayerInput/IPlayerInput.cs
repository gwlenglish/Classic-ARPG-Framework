
using UnityEngine;

namespace GWLPXL.ARPGCore.PlayerInput.com
{



    public interface IPlayerMouseInput
    {
        bool GetMouseButtoneOne();
        bool GetMouseButtonOneDown();
        bool GetMouseButtonTwo();
        bool GetMouseButtonTwoDown();
        Vector3 GetMousePosition();
    }
}
