using UnityEngine;
using UnityEngine.SceneManagement;

namespace SWGame.View.Scenes
{
    public class Menu : MonoBehaviour
    {
        public void StartRegistration()
        {
            SceneManager.LoadScene("Registration", LoadSceneMode.Single);
        }
        public void StartAuthorization()
        {
            SceneManager.LoadScene("Authorization", LoadSceneMode.Single);
        }
        public void FinishGame()
        {
            Application.Quit();
        }
    }
}
