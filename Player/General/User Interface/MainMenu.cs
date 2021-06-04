using UnityEngine.SceneManagement;
using UnityEngine;

//User interface functinoality for the main menu
public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MainLevel");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
