using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Movement.com;
using GWLPXL.ARPGCore.Saving.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.States.com
{


    public class EnemyIdle2D : IState
    {
        float aggroRange = 0;
        List<GameObject> aggroList = new List<GameObject>();
        List<GameObject> nonAggro = new List<GameObject>();
        Vector2 homeDestination;


        IActorHub hub;



        public EnemyIdle2D(IActorHub mover, Vector3 home, float aggroRange)
        {
            this.hub = mover;
            this.homeDestination = (Vector2)home;
            this.aggroRange = aggroRange;

        }
        public void Enter()
        {


        }

        public void Exit()
        {

        }

        public void Tick()
        {
            PlayerSceneReference[] players = DungeonMaster.Instance.GetAllSceneReferences();
            aggroList.Clear();
            nonAggro.Clear();

            for (int i = 0; i < players.Length; i++)
            {
                Vector3 dir = players[i].SceneRef.transform.position - hub.MyTransform.position;
                float sqrd = dir.sqrMagnitude;
                if (sqrd <= aggroRange * aggroRange)
                {
                    aggroList.Add(players[i].SceneRef.gameObject);
                }
                else
                {
                    nonAggro.Add(players[i].SceneRef.gameObject);
                }
            }

            if (nonAggro.Count == players.Length)
            {
                //found no one, return home.
                hub.MyMover.SetDesiredDestination(homeDestination, .05f);
                return;
            }

            bool hasTarget = false;
            GameObject targetInstance = null;
            if (aggroList.Count > 0)
            {
                //we found someone

                if (aggroList.Count == 1)
                {
                    //only one.
                    hasTarget = true;
                    Vector3 targetdestination = aggroList[0].transform.position;
                    hub.MyMover.SetDesiredDestination(targetdestination, 1f);
                    targetInstance = aggroList[0].gameObject;



                }
                else
                {
                    //find closest
                }
            }
            else
            {
                hasTarget = false;
            }


            if (hasTarget)
            {

                Ability ability = hub.MyAbilities.GetLastIntendedAbility();
                if (ability == null)
                {
                    ability = hub.MyAbilities.GetRuntimeController().GetEquippedAbility(0);
                    hub.MyAbilities.SetIntendedAbility(ability);
                }
                Vector2 dir = (Vector2)targetInstance.transform.position - (Vector2)this.hub.MyTransform.position;
                Vector2 normalizedDir = dir.normalized;


                float sqrd = dir.sqrMagnitude;

                if (hub.MyProjectiles != null)
                {
                    hub.MyProjectiles.GetProjectileFirePoint().transform.right = normalizedDir;

                }

                if (sqrd <= ability.GetRangeSquaredWithBuffer() && hub.MyAbilities.GetInCooldown() == false && ability.HasSight(hub, targetInstance.transform, EditorPhysicsType.Unity2D) == true)
                {
                    hub.MyAbilities.TryCastAbility(ability);
                }

            }
        }

      
    }
}