
using GWLPXL.ARPGCore.com;
using UnityEngine;
using UnityEngine.AI;

namespace GWLPXL.ARPGCore.Movement.com
{

    /// <summary>
    /// Used for navigation, such as navmesh.
    /// </summary>
    public interface IMover
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="byAmount"></param>
        void ModifySpeedMultiplier(float byAmount);
        float GetSpeedMultiplier();
        float GetVelocitySquaredMag();
        void SetUpMover();
        IActorHub GetHub();
        void SetActorHub(IActorHub newHub);
        void SetVelocity(Vector3 newVel);
        Vector3 GetVelocityDirection();
        void ResetState();
        void SetDesiredDestination(Vector3 newDestination, float stoppingDistance);
        void SetDesiredRotation(Vector3 towards, float stoppingDistance);
        void DisableMovement(bool isStopped);
        void SetNewSpeed(float newTopSpeed, float newAcceleration);
        void ResetSpeed();
        bool GetMoverEnabled();

    }

    public interface INavMeshMover
    {
        NavMeshAgent GetAgent();
        void StopAgentMovement(bool isStopped);
        void SetAgentBaseHeight(float byamount);
        void ResetBaseHeight();
        void EnableUpdate(bool updatePosition, bool updateRotation);
        void SetAgentPositionRotaion(Vector3 newpos, Quaternion newRot);
    }
}