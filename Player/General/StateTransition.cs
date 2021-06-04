using UnityEngine;

//Script controlling which state the player is in
public class StateTransition : MonoBehaviour
{
    //References
    public GameObject characterCam;
    public GameObject droneCam;
    public GameManager gameManager;

    private void Start()
    {
        droneCam.SetActive(false);
        characterCam.SetActive(true);
        gameManager.CharacterState();
    }
    
    private void Update()
    {
        TransitionLoop();
    }

    private void TransitionLoop()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //Character => drone
            if (gameManager.playerState == 1)
            {
                characterCam.SetActive(false);
                droneCam.SetActive(true);
                gameManager.playerState +=1;

                gameManager.DroneState();
            }

            //Drone => character
            else if (gameManager.playerState == 2)
            {
                droneCam.SetActive(false);
                characterCam.SetActive(true);
                gameManager.playerState -=1;

                gameManager.CharacterState();
            }
        }
    }
}
