using UnityEngine;

//Randomly switches cameras in the menu
public class MenuCameraSwitch : MonoBehaviour
{
    private float currentCameraer;
    private int rand;
    private float startTime;
    public float currentTimer;
    public float switchTime;

    public int currentCamera = 1;
    public GameObject characterCamera;
    public GameObject droneCamera;

    private void Start()
    {
        startTime = switchTime;
        currentTimer = startTime;
    }

    private void Update()
    {
        if (currentTimer <= 0)
        {
            //Character => drone
            if (currentCamera == 1)
            {
                currentCamera += 1;
                characterCamera.SetActive(false);
                droneCamera.SetActive(true);

                currentTimer = startTime;
            }

            //Drone => character
            else if (currentCamera == 2)
            {
                currentCamera -= 1;
                droneCamera.SetActive(false);
                characterCamera.SetActive(true);

                currentTimer = startTime;
            }
        }

        else
        {
            currentTimer -= Time.deltaTime;
        }
    }
}
