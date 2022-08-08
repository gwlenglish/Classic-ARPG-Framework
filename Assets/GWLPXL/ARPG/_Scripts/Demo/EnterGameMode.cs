using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace GWLPXL.ARPGCore.Demo.com
{


    public class EnterGameMode : MonoBehaviour
    {
        public string FirstSceneName = string.Empty;

        public void BeginPlay()
        {
            SceneManager.LoadScene(FirstSceneName);
        }
    }
}