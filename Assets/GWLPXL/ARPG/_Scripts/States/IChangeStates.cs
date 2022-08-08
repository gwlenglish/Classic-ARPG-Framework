
namespace GWLPXL.ARPGCore.States.com
{
    /// <summary>
    /// meh this state machine interface
    /// </summary>
    public interface IChangeStates
    {
        void ChangeState(IState newstate);
        void DefaultMoveState();
    }


}