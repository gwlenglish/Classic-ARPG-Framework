using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Leveling.com
{
    /// <summary>
    /// used to control the XP curve and to award XP to the player
    /// the interface speaks to the thing that provides the level flaire when we level up
    /// </summary>

    public class PlayerLeveling : MonoBehaviour, ILevel
    {
        public IVisualizeXP VisualizeXP { get; set; }
        public System.Action<int> OnExperienceGain;
        public System.Action<int> OnLevelGain;
        public System.Action OnExperienceModify;

        [SerializeField]
        PlayerLevelEvents levelEvents = new PlayerLevelEvents();
        #region fields
        [SerializeField]
        AnimationCurve XPCurve = null;
        [SerializeField]
        int firstLevelXp = 100;
        [SerializeField]
        int lastLevelXp = 999999;

        //int nextLevelXP;
        int carryOver = default;
        IAttributeUser stats = null;
        bool leveling = default;
        Queue<int> levels = new Queue<int>();
        #endregion
        int previous = 0;
        #region private
        void Awake()
        {
            stats = GetComponent<IAttributeUser>();
            
        }


        int SetNextLevelXP()
        {
            float multi = ((float)stats.GetRuntimeAttributes().MyLevel / (float)stats.GetRuntimeAttributes().MaxLevel);//linear is default is no curve set
            float newBase = Mathf.Lerp(firstLevelXp, lastLevelXp, multi);
            float baseNextLevel = (float)newBase + ((float)newBase * multi);

            if (XPCurve == null)
            {
                baseNextLevel = (float)newBase + (newBase * XPCurve.Evaluate(multi));
            }

         
            float newXP = baseNextLevel;
            int nextLevelXP = Mathf.RoundToInt(baseNextLevel);
            ARPGDebugger.DebugMessage("Next Level XP " + nextLevelXP, this);
            return nextLevelXP;
        }

        void AddXP(int newAmount)
        {
            stats.GetRuntimeAttributes().SetCurrentXP(stats.GetRuntimeAttributes().CurrentXP + newAmount);
            levelEvents.SceneEvents.OnXPGain.Invoke(newAmount);
            OnExperienceGain?.Invoke(newAmount);
            OnExperienceModify?.Invoke();
        }

        #endregion

        #region public
        public void EarnXP(int newAmount)
        {
            if (stats.GetRuntimeAttributes().MyLevel >= stats.GetRuntimeAttributes().MaxLevel) return;//we can't level anymore

            AddXP(newAmount);
            while (GetCurrentXP() >= GetNextLevelXP())
            {
                carryOver = GetCurrentXP() - GetNextLevelXP();
                stats.GetRuntimeAttributes().SetCurrentXP(0);
                stats.GetRuntimeAttributes().SetCurrentXP(carryOver);
                int currentLevel = stats.GetRuntimeAttributes().MyLevel;
                SetNextLevelXP();
                int nextLevel = currentLevel + 1;
                stats.GetRuntimeAttributes().LevelUp(nextLevel);
                RaiseSceneEvents(nextLevel);
                LevelFlaire(nextLevel);
                if (stats.GetRuntimeAttributes().MyLevel >= stats.GetRuntimeAttributes().MaxLevel)
                {
                    break;
                }
            }


        }

        private void RaiseSceneEvents(int nextLevel)
        {
            if (levelEvents == null) return;
            if (levelEvents.SceneEvents == null) return;
            levelEvents.SceneEvents.OnLevelUp.Invoke(nextLevel);
            OnLevelGain?.Invoke(nextLevel);

        }

        public int GetCurrentXP()
        {
            return stats.GetRuntimeAttributes().CurrentXP;
        }

        public int GetNextLevelXP()
        {
            return SetNextLevelXP();

        }

        public void LevelFlaire(int whichLevel)
        {
            levels.Enqueue(whichLevel);

            if (leveling == false)
            {

                StartCoroutine(DoLevelFlaire());

            }
            leveling = true;



        }



        public bool Leveling()
        {
            return leveling;
        }

        public int GetCurrentLevel()
        {
            return stats.GetRuntimeAttributes().MyLevel;
        }


        public void SetVisualizer(IVisualizeXP visualXP)
        {
            VisualizeXP = visualXP;
        }

        #endregion

        #region coroutines

        IEnumerator DoLevelFlaire()
        {
            while (levels.Count > 0)
            {
                int forLevel = levels.Dequeue();
                VisualizeXP.LevelUpFlaire(forLevel);
                yield return null;
                if (VisualizeXP != null)
                {
                    yield return new WaitUntil(() => VisualizeXP.LevelUpFlaireComplete());
                    VisualizeXP.ResetFlaireComplete();
                }

                if (levels.Count == 0)
                {
                    leveling = false;
                    break;
                }

            }

        }

        #endregion
    }
}