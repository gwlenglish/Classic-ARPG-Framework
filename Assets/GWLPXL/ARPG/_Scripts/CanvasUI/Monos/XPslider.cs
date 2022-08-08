
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Leveling.com;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GWLPXL.ARPGCore.CanvasUI.com
{


    public class XPslider : MonoBehaviour, IVisualizeXP, IActorUI
    {
        public ILevel Leveler = null;
        public Vector3 LevelUpSize = new Vector3(1, 1, 1);
        public float Duration = 1;
        [SerializeField]
        Slider slider = null;
        [SerializeField]
        TextMeshProUGUI xpvaluetext = null;
        [SerializeField]
        TextMeshProUGUI currentleveltext = null;
        bool flaireComplete = false;


        public void SetUI(IActorHub player)
        {
            Leveler = player.Level;
            Leveler.SetVisualizer(this);
            PlayerLeveling leveling = player.MyTransform.GetComponentInChildren<PlayerLeveling>();
            leveling.OnExperienceModify += UpdateXP;
            leveling.OnLevelGain += UpdateLeveleling;
            UpdateLeveleling(leveling.GetCurrentLevel());
        }

        void UpdateLeveleling(int newLevel)
        {
            UpdateLevel(newLevel);
            UpdateXP();
        }
        void UpdateXP()
        {
            UpdateXP(Leveler.GetCurrentXP(), Leveler.GetNextLevelXP());
        }
     

        void UpdateXP(int value, int max)
        {
            slider.maxValue = max;
            slider.value = value;
            xpvaluetext.text = value.ToString() + " / " + max.ToString();
        }

        void UpdateLevel(int newLevel)
        {
            currentleveltext.text = "Level: " + newLevel.ToString();
        }

        public void LevelUpFlaire(int forLevel)
        {

            StartCoroutine(IncreaseSize(forLevel));

        }

        IEnumerator IncreaseSize(int forLevel)
        {

            float timer = 0;
            currentleveltext.text = "Level: " + forLevel.ToString();
            while (timer <= Duration)
            {
                yield return null;
                timer += Time.deltaTime;
                float percent = timer / Duration;
                Vector3 lerp = Vector3.Lerp(Vector3.one, LevelUpSize, percent);
                currentleveltext.transform.localScale = lerp;
            }
            flaireComplete = true;
            currentleveltext.transform.localScale = Vector3.one;
            UpdateLevel(forLevel);
        }


        public bool LevelUpFlaireComplete()
        {
            return flaireComplete;
        }

        public void ResetFlaireComplete()
        {
            flaireComplete = false;
        }

     

       
     
    }
}