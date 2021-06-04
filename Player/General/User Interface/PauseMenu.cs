using UnityEngine.SceneManagement;
using UnityEngine;

//User interafce functionality for pause behaviours
public class PauseMenu : MonoBehaviour
{
    //Variables
    public static bool pauseState;

    //References
    public GameManager gameManager;
    public GameObject pauseScreen;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseState)
        {
            Pause();
        }

        else if (Input.GetKeyDown(KeyCode.Escape) && pauseState)
        {
            Resume();
        }
    }

    void Pause()
    {
        pauseState = true;
        gameManager.PauseState();
        pauseScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    void Resume()
    {
        pauseState = false;
        gameManager.UnpausedState();
        pauseScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        pauseState = false;
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
