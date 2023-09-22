using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UserInterface
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject mainUIObject;
        [SerializeField] private GameObject pauseMenuObject;

        public void Open()
        {
            Time.timeScale = 0;
            mainUIObject.SetActive(false);
            pauseMenuObject.SetActive(true);
        }

        public void Close()
        {
            Time.timeScale = 1;
            mainUIObject.SetActive(true);
            pauseMenuObject.SetActive(false);
        }

        public void Resign()
        {
            GameManager.GetInstance().GameOver();
            SceneManager.LoadScene("MainMenu");
        }
    }
}