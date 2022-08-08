namespace GWLPXL.ARPGCore.States.com
{


    public interface IState
    {
        void Enter();
        void Tick();
        void Exit();

    }
}