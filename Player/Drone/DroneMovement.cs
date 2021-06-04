using UnityEngine;
using Cinemachine;
using System.Collections;

//Movement script for the playable drone
public class DroneMovement : MonoBehaviour
{
    //References
    public CharacterController controller;
    public Transform camPos;

    //Turn values
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    //Physics values
    public float speed = 6f;
    public float verticalForce = 100f;
    private Vector3 velocity;

    //Speed boost
    private bool canBoost = true;
    public float speedModifier;
    public float boostTime = 1f;

    private void Update()
    {
        //Input values
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        //Direction vector
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        //If drone moved
        if (direction.magnitude >= 0.1f)
        {
            //Angle for drone rotation
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camPos.eulerAngles.y;

            //Smooth out drone rotation
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            //Rotate drone to where its facing
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //Move drone
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        VerticalMovement();
        SpeedBoost();

        controller.Move(velocity * Time.deltaTime);
    }

    private void VerticalMovement()
    {
        //Fly up
        if (Input.GetKey(KeyCode.E))
        {
            velocity.y = +verticalForce * Time.deltaTime;
        }

        //Fly down
        else if (Input.GetKey(KeyCode.Q))
        {
            velocity.y = -verticalForce * Time.deltaTime;
        }

        //No movement
        else
        {
            velocity.y = -0.3f * Time.deltaTime;
        }
    }

    private void SpeedBoost()
    {
        if (canBoost)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine(DroneBoost());
            }
        }
    }

    private IEnumerator DroneBoost()
    {
        float speedOriginal = speed;
        canBoost = false;
        speed *= speedModifier;
        yield return new WaitForSeconds(boostTime);
        speed = speedOriginal;
        canBoost = true;
    }
}
