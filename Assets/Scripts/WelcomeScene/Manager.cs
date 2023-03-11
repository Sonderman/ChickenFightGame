using UnityEngine;
using UnityEngine.SceneManagement;

namespace WelcomeScene
{
    public class Manager : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
