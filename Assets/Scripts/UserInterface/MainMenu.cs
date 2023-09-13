using UnityEngine;
using UnityEngine.SceneManagement;

namespace UserInterface
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private string mainScene;
        
        public void NewGame()
        {
            SceneManager.LoadScene(mainScene);
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}