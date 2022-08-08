
using UnityEngine;
using UnityEngine.SceneManagement;
namespace GWLPXL.ARPGCore.Util.com
{


    public class SceneLoader : MonoBehaviour
    {
        public string sceneName = string.Empty;
        public bool LoadAtStart = false;
        // Start is called before the first frame update
        void Start()
        {
            if (LoadAtStart == false) return;
            LoadScene(sceneName);
        }

        public void LoadScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName)) return;

            Scene scene = SceneManager.GetSceneByName(sceneName);
            SceneManager.LoadScene(sceneName);
        }
    }

}