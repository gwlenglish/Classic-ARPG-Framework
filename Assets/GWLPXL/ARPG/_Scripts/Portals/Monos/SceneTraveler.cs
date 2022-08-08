using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Statics.com;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace GWLPXL.ARPGCore.Portals.com
{
    /// <summary>
    /// not final, but works for now on 3d for navmhes
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class SceneTraveler : MonoBehaviour, IPortalUser
    {
        public virtual void Travel()
        {
            string fromScene = DungeonMaster.Instance.Last.SceneName;
            int intendedPos = DungeonMaster.Instance.Last.Position;
            if (intendedPos == 0) return;//if it's zero, just start at our default location

            List<IPortal> portals = FindInterfaces.FindAll<IPortal>();//ugly but works for now
            for (int i = 0; i < portals.Count; i++)
            {
                if (portals[i].GetSceneName() == DungeonMaster.Instance.Last.SceneName)
                {
                    GetComponent<NavMeshAgent>().Warp(portals[i].GetLocation());

                }

            }

        }
    }
}
