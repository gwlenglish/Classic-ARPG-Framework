
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace GWLPXL.ARPGCore.CanvasUI.com
{
    /// <summary>
    /// put on the loading canvas to direct its fade and text
    /// </summary>
    /// 


    public class InitializeLoadingCanvasUI : MonoBehaviour, ILoadingCanvas
    {
        #region fields
        [SerializeField] protected GameObject mainPanel;
        [SerializeField] protected GameObject progressBarGo;
        [SerializeField] protected Slider progressBar;
        [SerializeField] protected TextMeshProUGUI progressText;
        [SerializeField] protected TextMeshProUGUI pressButtonText;
        [SerializeField] protected Image loadingCurtain;
        [SerializeField] protected bool startEnabled;
        #endregion



        protected void Start()
        {
            //EnableMainPanel(startEnabled);
        }


        #region private methods
        void SetSliderProgress(float newProgress)
        {
            progressBar.value = newProgress;
        }

        void SetProgressText(string newText)
        {
            progressText.text = newText;
        }

        void SetPressButtonText(string newText)
        {
            pressButtonText.text = newText;
        }
        void SetLoadingCurtainAlpha(float newAlpha)
        {
            Color temp = loadingCurtain.color;
            temp.a = newAlpha;
            loadingCurtain.color = temp;
        }
        void EnableProgressBar(bool isActive)
        {
            progressBarGo.SetActive(isActive);
        }
        #endregion


        public void EnableMainPanel(bool isEnabled)
        {
            if (mainPanel != null)
            {
                mainPanel.SetActive(isEnabled);
            }
        }

        #region interface
        public void SetProgress(float newPercent)
        {
            SetSliderProgress(newPercent);
            SetProgressText("Loading Percent: " + newPercent.ToString());
            SetLoadingCurtainAlpha(newPercent);


        }

        public void EnableLoading(bool isEnabled)
        {
            EnableMainPanel(isEnabled);
            EnableProgressBar(isEnabled);
        }


        public void LoadingComplete()
        {
            SetSliderProgress(1f);//show that loading is complete
            SetProgressText("Loading Complete!");
            SetPressButtonText("Press spacebar to continue");
        }

        public void SetTransitionEffects(float transitionPercent)
        {
            SetLoadingCurtainAlpha(transitionPercent);
        }

        public void EnableLoadingBar(bool isEnabled)
        {
            if (isEnabled)
            {
                SetLoadingCurtainAlpha(1f);
                SetSliderProgress(0f);
                SetProgressText("Loading percent...");
                SetPressButtonText("");
                EnableProgressBar(true);
            }
            else
            {
                EnableProgressBar(false);
            }

        }

        #endregion
    }
}
