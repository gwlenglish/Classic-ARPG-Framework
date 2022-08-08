

using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Movement.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.States.com
{
    /// <summary>
    /// not yet implemented in demo, experimental, deprecated, delete
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(IChangeStates))]
    public class EnemyKnockbackReceiver : MonoBehaviour
    {
        //IActorHub mover;
        //IChangeStates ai;
        //IReceiveDamage myDamage;
        //Rigidbody rb;
        //private void Awake()
        //{
        //    mover = GetComponent<IActorHub>();
        //    ai = GetComponent<IChangeStates>();
        //    rb = GetComponent<Rigidbody>();
        //    myDamage = GetComponent<IReceiveDamage>();
        //}
        //public void DoKnockback(KnockbackVars knockbackstate)
        //{
        //    if (mover == null || ai == null || myDamage.IsDead()) return;
        //    knockbackstate.hitObj = rb;
        //    KnockbackState knockbackState = new KnockbackState(knockbackstate, mover, myDamage);
        //    ai.ChangeState(knockbackState);
        //}


    }
}
