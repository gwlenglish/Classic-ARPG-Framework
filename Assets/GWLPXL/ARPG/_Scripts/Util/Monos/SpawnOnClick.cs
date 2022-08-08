
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Movement.com;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace GWLPXL.ARPGCore.Demo.com
{


    public class SpawnOnClick : MonoBehaviour, IInteract
    {
        public Enemy EnemyPrefab = null;
        public Transform SpawnPoint = null;
        public TextMeshPro LevelText = null;
        public int Level = 1;
        public bool RandomLevel = true;
        public Vector2Int RandomLevelRange = new Vector2Int(1, 99);

        private void Awake()
        {
            LevelText.text = Level.ToString();
        }
        public bool DoInteraction(GameObject invUser)
        {
            bool performed = false;
            Enemy newEnemy = Instantiate(EnemyPrefab);
            newEnemy.InitialLevel = Level;
            newEnemy.MyStats.GetRuntimeAttributes().LevelUp(Level);
            
            NavMeshAgent agent = newEnemy.GetComponent<NavMeshAgent>();
            agent.Warp(SpawnPoint.position);

            performed = true;
            return performed;
        }

        public bool IsInRange(GameObject invUser)
        {
            return true;
        }
    }
}

