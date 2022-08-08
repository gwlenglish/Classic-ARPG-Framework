
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Movement.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.States.com
{


    public class EnemyAttack : IState
    {
        IActorHub enemy;

        float repeatTimer = 0;
        float randomDelay = 0;
        public EnemyAttack(IActorHub enemySkills)
        {
            enemy = enemySkills;

        }
        public void Enter()
        {
            if (enemy != null)
            {
                enemy.MyMover.DisableMovement(true);

            }
            enemy.MyAbilities.TryCastAbility(enemy.MyAbilities.GetLastIntendedAbility());
            repeatTimer = 0;
            int randoInt = Random.Range(0, 1000);
            float converted = (float)randoInt / 1000;
            randomDelay = converted;

         
        }

        public void Exit()
        {
            if (enemy != null)
            {
                enemy.MyMover.DisableMovement(false);

            }


        }

        public void Tick()
        {
            repeatTimer += Time.deltaTime;

            if (repeatTimer >= enemy.MyAbilities.GetLastIntendedAbility().Duration + enemy.MyAbilities.GetLastIntendedAbility().Delay + randomDelay)
            {
                repeatTimer = 0;
                enemy.MyAbilities.TryCastAbility(enemy.MyAbilities.GetLastIntendedAbility());
                int randoInt = Random.Range(0, 1000);
                float converted = (float)randoInt / 1000;
                randomDelay = converted;
            }

           
        }
    }
}