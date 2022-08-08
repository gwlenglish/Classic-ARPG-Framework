using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;


using UnityEngine;

namespace GWLPXL.ARPGCore.States.com
{

    [System.Serializable]
    public class EnemyChasing2D : IState
    {
        public Transform chaseTarget;
        float tickrate = 0;
        bool complete;
        public float chaseDuration;


        IActorHub hub;
        public float chaseTimer = 0;


        public EnemyChasing2D(IActorHub mover, float tickrate, float chaseDuration)
        {
            this.hub = mover;

            this.tickrate = tickrate;
            this.chaseDuration = chaseDuration;

        }
        public void Enter()
        {
            complete = false;
            chaseTimer = 0;

        }

        public void Exit()
        {
            chaseTimer = chaseDuration;
        }

        public void Tick()
        {
            if (complete) return;

            hub.MyMover.SetDesiredDestination(chaseTarget.transform.position, .05f);
            chaseTimer += tickrate;

            if (chaseTimer >= chaseDuration)
            {
                chaseTarget = null;
                return;
            }
            Vector3 chaseDir = chaseTarget.transform.position - hub.MyTransform.position;
            chaseDir.Normalize();

            if (hub.MyProjectiles != null)
            {
                hub.MyProjectiles.GetProjectileFirePoint().transform.right = chaseDir;
            }


            Ability ability = hub.MyAbilities.GetLastIntendedAbility();
            if (ability == null)
            {
                ability = hub.MyAbilities.GetRuntimeController().GetEquippedAbility(0);
                hub.MyAbilities.SetIntendedAbility(ability);
            }
            chaseDir.z = 0;
            float sqrd = chaseDir.sqrMagnitude;

            if (sqrd <= ability.GetRangeSquaredWithBuffer() && hub.MyAbilities.GetInCooldown() == false && ability.HasSight(hub, chaseTarget.transform, EditorPhysicsType.Unity2D) == true)
            {
                hub.MyAbilities.TryCastAbility(ability);
            }
        }

      
    }
}