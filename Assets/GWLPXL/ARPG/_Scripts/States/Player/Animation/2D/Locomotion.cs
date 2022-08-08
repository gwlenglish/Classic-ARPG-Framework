using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.States.com
{


    public abstract class Locomotion : ScriptableObject
    {

        public abstract void DoLocomotion(IStateMachineEntity forEntity, float dt);



    }

    public abstract class Movement2D : Locomotion
    {
        public MoveVars2D Vars { get; set; }
    }

    public abstract class Movement : Locomotion
    {
        public MoveVars Vars { get; set; }
    }
}