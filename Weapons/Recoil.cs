using UnityEngine;
using Cinemachine;

//Recoil for player used weapons
public class Recoil : MonoBehaviour
{
    [Header("Vertical Recoil")]
    public float verticalRangeMin;
    public float verticalRangeMax;

    [Header("Horizontal Recoil")]
    public float horizontalRange;
    public float duration;

    //Variables
    private float time;
    private float verticalRecoil;
    private float horizontalRecoil;

    //References
    public CharacterMovement camRotation;
    public CinemachineVirtualCamera playerCamera;

    public void GenerateRecoil()
    {
        time = duration;
        verticalRecoil = Random.Range(verticalRangeMin, verticalRangeMax);
        horizontalRecoil = Random.Range(-horizontalRange, horizontalRange);
    }

    private void Update()
    {
        if (time > 0)
        {
            camRotation.xRotation -= ((verticalRecoil / 10) * Time.deltaTime) / duration;
            camRotation.yRotation -= ((horizontalRecoil / 10) * Time.deltaTime) / duration;
            time -= Time.deltaTime;
        }
    }
}
