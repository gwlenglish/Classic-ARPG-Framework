



using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Movement.com;

namespace GWLPXL.ARPGCore.States.com
{

    /// <summary>
    /// delete, using agent disabled instead
    /// </summary>
    public class InventoryInput : IState
    {
        /// <summary>
        /// do nothing. we just want to stop our movements while in the UI screen.
        /// </summary>

        IActorHub iNavMover;
        public InventoryInput(IActorHub mover)
        {
            iNavMover = mover;
        }

        public void Enter()
        {
          //  DungeonMaster.Instance.GetDungeonUISceneRef().SetDungeonState(0);
            iNavMover.MyMover.DisableMovement(true);
        }


        public void Exit()
        {
            iNavMover.MyMover.DisableMovement(false);
         //   DungeonMaster.Instance.GetDungeonUISceneRef().SetDungeonState(1);

        }



        public void Tick()
        {

        }
    }
}