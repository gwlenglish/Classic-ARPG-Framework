
namespace GWLPXL.ARPGCore.CanvasUI.com
{


    public interface ILoadingCanvas
    {
        void SetProgress(float newPercent);
        void EnableLoading(bool isEnabled);
        void SetTransitionEffects(float transitionPercent);
        void EnableLoadingBar(bool isEnabled);
        void LoadingComplete();

    }
}