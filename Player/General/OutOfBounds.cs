using UnityEngine;

//Tells the player to return to playable area
public class OutOfBounds : MonoBehaviour
{
    //References
    public GameObject msg;

    void OnTriggerStay(Collider other)
    {
        msg.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        msg.SetActive(false);
    }
}
