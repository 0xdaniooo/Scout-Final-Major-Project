using UnityEngine.SceneManagement;
using UnityEngine;

//User interface for player death
public class DeathScreen : MonoBehaviour
{
    //References
    public GameManager gameManager;
    public GameObject deathScreen;

    public void Restart()
    {
        gameManager.RestartGame();
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
