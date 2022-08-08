using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.com
{

    /// <summary>
    /// possibly revise into scriptable object
    /// </summary>
    public class SpawnRandomEnemy : MonoBehaviour
    {

        public GameObject[] ActorPrefab = new GameObject[0];
        public Transform SpawnPoint = null;
        public int Level = 1;
        [Tooltip("If true, will use random range below. If false, will use level set above.")]
        public bool RandomLevel = true;
        public Vector2Int RandomLevelRange = new Vector2Int(1, 99);

        GameObject previousActor = null;

        public void Spawn()
        {
            int rando = Random.Range(0, ActorPrefab.Length - 1);
            if (previousActor != null)
            {
                Destroy(previousActor);
            }
            previousActor = Instantiate(ActorPrefab[rando], SpawnPoint.position, SpawnPoint.rotation);
            Enemy stats = previousActor.GetComponent<Enemy>();
            if (stats != null)
            {
                int level = Level;
                if (RandomLevel)
                {
                    level = Random.Range(RandomLevelRange.x, RandomLevelRange.y + 1);


                }
                stats.InitialLevel = level;
            }
        }
    }
}