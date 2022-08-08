
using GWLPXL.ARPGCore.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.Demo.com
{

    /// <summary>
    /// allows spawning one actor at a time
    /// </summary>
    public class OnClickSpawnActor : MonoBehaviour, IInteract
    {

        public GameObject ActorPrefab = null;
        public Transform SpawnPoint = null;
        public float InteractDistance = 2;
        public int Level = 1;
        public bool RandomLevel = true;
        public Vector2Int RandomLevelRange = new Vector2Int(1, 99);

        GameObject previousActor = null;


        public bool DoInteraction(GameObject invUser)
        {
            bool performed = false;
            
            GameObject newActor = Instantiate(ActorPrefab);
            Enemy stats = newActor.GetComponent<Enemy>();
            if (stats != null)
            {
                int level = Level;
                if (RandomLevel)
                {
                    level = Random.Range(RandomLevelRange.x, RandomLevelRange.y + 1);
                   

                }
                stats.InitialLevel = level;
            }

            newActor.transform.position = SpawnPoint.transform.position;
            newActor.transform.rotation = SpawnPoint.transform.rotation;

            performed = true;


            if (performed)//cleans up old one, this only allows one instance spawn per interact
            {
                if (previousActor != null)
                {
                    Destroy(previousActor);
                }
                previousActor = newActor;
            }
            return performed;
        }

        public bool IsInRange(GameObject invUser)
        {
            Vector3 dir = invUser.transform.position - this.transform.position;
            float sqrdst = dir.sqrMagnitude;
            if (sqrdst <= InteractDistance * InteractDistance)
            {
                return true;
            }
            return false;
        }
    }
}