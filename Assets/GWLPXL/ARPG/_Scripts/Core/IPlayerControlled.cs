using GWLPXL.ARPGCore.Leveling.com;
using GWLPXL.ARPGCore.PlayerInput.com;
using GWLPXL.ARPGCore.Quests.com;
using GWLPXL.ARPGCore.Shopping.com;
using UnityEngine;
namespace GWLPXL.ARPGCore.com
{


    public interface IPlayerControlled
    {
        int GetPlayerNumber();
        GameObject GetGameObject();
        IActorHub GetActorHub();
        IShopper Shopper { get; set; }
        IPlayerCanvasHub CanvasHub { get; set; }

    }

    
       


}




