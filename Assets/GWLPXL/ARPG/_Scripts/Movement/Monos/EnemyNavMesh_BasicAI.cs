

using GWLPXL.ARPGCore.States.com;

using UnityEngine;
using UnityEngine.AI;

namespace GWLPXL.ARPGCore.Movement.com
{
    /// <summary>
    /// not yet implemented in a meaningful way.
    /// </summary>


    
    public class EnemyNavMesh_BasicAI : MonoBehaviour
    {
        NavMeshAgent agent;
        IChangeStates statemachine;
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            statemachine = GetComponent<IChangeStates>();
        }

        public void ChangeLocoState(IState newState)
        {
            statemachine.ChangeState(newState);
        }

        public void EnableAI(bool isEnabled)
        {
            //we need to check because the enemy might die during the attack sequence, so the coroutine will still be running until the gameobject is destroyed
            if (agent == null) return;
            if (agent.isActiveAndEnabled == false) return;
            agent.isStopped = !isEnabled;//if we are stopped, then we are not enabled.
            if (isEnabled == false)
            {
                agent.enabled = false;
            }
            else
            {
                agent.enabled = true;
            }
        }

        public bool GetIsEnabled()
        {
            return agent.enabled;
        }

        public bool GetIsReadyToAttack()
        {
            return agent.enabled;// && abilities.GetIsUsingSkill() == false;
        }


    }
}