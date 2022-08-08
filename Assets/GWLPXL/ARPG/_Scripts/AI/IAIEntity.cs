using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.AI.com
{
    public interface IAIEntity
    {
        void SetDirection(Vector3 newDirection);
        Vector3 GetDirection();
        void SetStateKey(string newState);
        string GetStateKey();
        IActorHub GetActorHub();
        void SetMoveTarget(GameObject newTarget);
        void SetAttackTarget(GameObject newTarget);
        GameObject GetMoveTarget();
        GameObject GetAttackTarget();
        float GetIdleDistance();
        bool GetActiveAbilityInUse();
        void SetActiveAbility(Ability newActive);
        Ability GetActiveAbility();

    }
}