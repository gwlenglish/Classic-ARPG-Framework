
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Leveling.com
{
    public class EnemyXP : MonoBehaviour, IGiveXP
    {
        public int BaseXP = 40;
        [Tooltip("Dungeon Scaling. If the Dungeon multi is set higher than 1, will use it to scale monster.")]
        public bool UseDungeonScaling = true;
        [Header("Diminishing Returns")]
        [Tooltip("Reduced XP gains for mobs under the player's level.")]
        public bool UseDiminishingReturns = true;
        [Tooltip("How many levels under the player does the mob stop giving XP? For instance, a delta of 2 would mean anything under the player level - 2 would not give xp.")]
        public int NoXPDelta = 2;
        [Tooltip("The rate of diminishing XP returns.")]
        public AnimationCurve DiminishCurve;

        public void GiveXP()
        {
            PlayerSceneReference[] players = DungeonMaster.Instance.GetAllSceneReferences();
            IScale scaler = GetComponent<IScale>();
            int moblevel = scaler.GetScaledLevel();

            //loop through all players
            for (int i = 0; i < players.Length; i++)
            {
                ILevel leveler = players[i].SceneRef.GetComponent<IActorHub>().Level;
                if (leveler != null)
                {
                    int scaled = BaseXP;
                    if (UseDungeonScaling == true)
                    {
                        scaled = Formulas.GetEnemyXP(scaled);
                    }


                    int currentlevel = leveler.GetCurrentLevel();
                    if (moblevel <= currentlevel - NoXPDelta)//under xp gains
                    {
                        scaled = 0;
                    }
                    else if (moblevel < currentlevel)
                    {
                        //diminish
                        int diff = currentlevel - moblevel;
                        if (DiminishCurve != null)
                        {
                            scaled = Mathf.RoundToInt(Mathf.Lerp(scaled, 0, DiminishCurve.Evaluate(diff / NoXPDelta)));
                        }
                        else
                        {
                            scaled = Mathf.RoundToInt(Mathf.Lerp(scaled, 0, diff / NoXPDelta));
                        }
                    }
                    leveler.EarnXP(scaled);

                }
            }
        }
    }
}