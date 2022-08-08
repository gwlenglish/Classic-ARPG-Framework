using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;

using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
    [System.Serializable]
    public class MouseRotateVars
    {
        public LayerMask Ground;
        public float RayLength = 100;
    }

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Abilities/Mods/New_MouseRotate Mod")]

    public class MouseRotate : AbilityLogic
    {
        public MouseRotateVars Vars;

        [System.NonSerialized]
        protected Dictionary<IActorHub, PlayerMouseRotateOnly> rotatedic = new Dictionary<IActorHub, PlayerMouseRotateOnly>();
        public override bool CheckLogicPreRequisites(IActorHub forUser)
        {
            if (rotatedic.ContainsKey(forUser)) return false;

            return forUser.InputHub != null && forUser.InputHub.MouseInputs != null;
        }

        public override void EndCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (rotatedic.ContainsKey(skillUser))
            {
                PlayerMouseRotateOnly only = rotatedic[skillUser];
                only.RemoveTicker();
                rotatedic.Remove(skillUser);
            }
        }

        public override void StartCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (rotatedic.ContainsKey(skillUser) == false)
            {
                PlayerMouseRotateOnly rotate = new PlayerMouseRotateOnly(skillUser, Vars);
                rotatedic.Add(skillUser, rotate);
            }
        }

      
    }

    public class PlayerMouseRotateOnly : ITick
    {
        IActorHub actor;
        MouseRotateVars vars;
        public PlayerMouseRotateOnly(IActorHub actor, MouseRotateVars vars)
        {
            this.vars = vars;
            this.actor = actor;
            AddTicker();
        }
        public void AddTicker()
        {
            
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            TryGroundMove(actor.InputHub.MouseInputs.GetMousePosition());
          

        }

        bool TryGroundMove(Vector3 atPosition)
        {
            RaycastHit groundHit;
            bool hitGround = Physics.Raycast(DungeonMaster.Instance.GetMainCamera().ScreenPointToRay(atPosition), out groundHit, vars.RayLength, vars.Ground);

            if (hitGround)
            {
                actor.MyMover.SetDesiredDestination(groundHit.point, 1f);
                Vector3 lookRotation = groundHit.point - actor.MyTransform.position;
                lookRotation.y = 0;
                actor.MyTransform.rotation = Quaternion.LookRotation(lookRotation);
            }
            return hitGround;

        }

        public float GetTickDuration() => Time.deltaTime;
       

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }
    }
}