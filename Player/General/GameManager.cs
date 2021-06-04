using UnityEngine.SceneManagement;
using UnityEngine;

//Script for controlling game flow
public class GameManager : MonoBehaviour
{
    //Variables
    public int playerState = 1;

    //References
    public GameObject deathScreen;
    public GameObject abilities;
    public CharacterMovement characterMovement;
    public DroneMovement droneMovement;

    public void PauseState()
    {
        Time.timeScale = 0f;

        //Character state
        if (playerState == 1)
        {
            characterMovement.enabled = false;
        }

        //Drone state
        else if (playerState == 2)
        {
            droneMovement.enabled = false;
        }

        abilities.SetActive(false);
    }

    //Used for unpausing the game
    public void UnpausedState()
    {
        Time.timeScale = 1f;

        //Character state
        if (playerState == 1)
        {
            characterMovement.enabled = true;
        }

        //Drone states
        else if (playerState == 2)
        {   
            droneMovement.enabled = true;
        }

        abilities.SetActive(true);
    }

    //Used for switching to character state
    public void CharacterState()
    {
        //Enables
        characterMovement.enabled = true;

        //Disables
        droneMovement.enabled = false;

    }

    //Used for switching to drone state
    public void DroneState()
    {
        //Enables
        droneMovement.enabled = true;

        //Disables
        characterMovement.enabled = false;
    }

    public void PlayerDeath()
    {
        PauseState();
        deathScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartGame()
    {
        UnpausedState();
        SceneManager.LoadScene("MainLevel");
    }
}
