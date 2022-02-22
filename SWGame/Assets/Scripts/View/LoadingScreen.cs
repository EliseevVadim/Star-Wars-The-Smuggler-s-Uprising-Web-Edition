using UnityEngine;
using UnityEngine.SceneManagement;

namespace SWGame.View
{
    public class LoadingScreen : MonoBehaviour
    {
        private void OnEnable()
        {
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
    }
}
