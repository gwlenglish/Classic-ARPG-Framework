using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.States.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/Player/2D/Locomotion/Dash")]

    public class RB2DDash : Dash
    {

        public override void DoLocomotion(IStateMachineEntity forEntity, float dt)
        {
            Vars.Timer += dt;
            Vector3 lerp = Vector3.Lerp(Vars.Start, Vars.End, Vars.Timer / Vars.Duration);
            forEntity.Get2D().GetRigidbody().MovePosition(lerp);

        }
    }


    [System.Serializable]
    public class DashLocoVars
    {
        public string AnimatorStateName = "Dash";
        public Vector3 Start;
        public Vector3 End;
        public float Duration = 1;
        public float Timer = 0;
    }
    public abstract class Dash : Locomotion
    {
        public DashLocoVars Vars { get; set; }
    }
    public abstract class GenericAttack : Locomotion
    {

    }
}