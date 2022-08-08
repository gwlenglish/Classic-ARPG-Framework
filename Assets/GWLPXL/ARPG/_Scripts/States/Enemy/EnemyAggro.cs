
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Movement.com;
using GWLPXL.ARPGCore.Saving.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.States.com
{


    public class EnemyAggro : IState
    {
        IActorHub hub;
        IAbilityUser abilities;
        Transform target;
        public EnemyAggro(IActorHub _mover, IAbilityUser _abilities, Transform _target)
        {
            hub = _mover;
            target = _target;
            abilities = _abilities;
        }
        public void Enter()
        {

        }



        public void Exit()
        {

        }


        public void Tick()
        {
            //chase the player and keep at range of our weapon
            if (target == null)
            {
                //doesn't have a target
                PlayerSceneReference[] targets = DungeonMaster.Instance.GetAllSceneReferences();
                if (targets == null || targets.Length == 0)
                {
                    Debug.LogError("No Player found in scene");
                }
                else
                {
                    target = targets[0].SceneRef.transform;
                }
            }

            float range = 1f;
            if (abilities != null && abilities.GetLastIntendedAbility() != null)
            {
                range = abilities.GetLastIntendedAbility().GetRange();
            }

            hub.MyMover.SetDesiredDestination(target.position, range);
           hub.MyMover.SetDesiredRotation(target.position, range);

        }
    }


}
