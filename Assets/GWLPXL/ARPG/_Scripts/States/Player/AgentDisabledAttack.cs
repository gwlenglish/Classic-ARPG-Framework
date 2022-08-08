

using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;


namespace GWLPXL.ARPGCore.States.com
{



    public class AgentDisabledAttack : IState
    {
        IActorHub playerMover;
        IAbilityUser abilities;


        public AgentDisabledAttack(IActorHub _player, IAbilityUser _abilities)
        {
            playerMover = _player;
            abilities = _abilities;
        }
        public void Enter()
        {
            //Player.MyMover.SetDesiredDestination(Player.transform.position, 1f);
            playerMover.MyMover.DisableMovement(true);
            abilities.TryCastAbility(abilities.GetLastIntendedAbility());
        }



        public void Exit()
        {
            //Player.MyMover.SetDesiredDestination(Player.transform.position, 1f);
            playerMover.MyMover.DisableMovement(false);
        }



        public void Tick()
        {

        }
    }
}