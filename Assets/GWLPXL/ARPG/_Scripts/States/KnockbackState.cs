
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Movement.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.States.com
{


    //[System.Serializable]
    //public class KnockbackVars
    //{
    //    public float Force;
    //    public float Duration;
    //    [HideInInspector]
    //    public Vector3 hitDirection;
    //    [HideInInspector]
    //    public Rigidbody hitObj;

    //    public KnockbackVars(float distance, float duration, Vector3 hitD, Rigidbody _hitObj)
    //    {
    //        Force = distance;
    //        Duration = duration;
    //        hitDirection = hitD;
    //        hitObj = _hitObj;

    //    }
    //}

    //maybe just delete this
    //public class KnockbackState : IState
    //{
    //    KnockbackVars vars;
    //    IActorHub mover;
    //    IReceiveDamage damageTaker;


    //    float timer = 0;

    //    public KnockbackState(KnockbackVars _vars, IActorHub _mover, IReceiveDamage _damageTaker)
    //    {
    //        vars = _vars;
    //        mover = _mover;
    //        damageTaker = _damageTaker;
    //    }
    //    public void Enter()
    //    {
    //        timer = 0;
    //        mover.MyMover.DisableMovement(true);
    //        vars.hitObj.AddForce(vars.hitDirection * vars.Force, ForceMode.Impulse);

    //    }


    //    public void Exit()
    //    {
    //        mover.MyMover.SetVelocity(Vector3.zero);
    //        mover.MyMover.DisableMovement(false);
    //        vars.hitObj.isKinematic = true;
    //        mover.MyMover.SetDesiredDestination(vars.hitObj.transform.position, 1f);
    //    }



    //    public void Tick()
    //    {
    //        if (damageTaker.IsDead() && damageTaker != null)
    //        {
    //            return;
    //        }
    //        timer += Time.deltaTime;

    //        if (timer >= vars.Duration)
    //        {
    //            return;
    //        }
    //        mover.MyMover.SetVelocity(vars.hitObj.velocity);
    //    }
    //}
}
