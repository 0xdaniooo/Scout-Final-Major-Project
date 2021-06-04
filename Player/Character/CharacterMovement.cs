using UnityEngine;
using Cinemachine;

//Movement script for the playable character
public class CharacterMovement : MonoBehaviour
{
    //Physics values
    public float baseSpeed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    private Vector3 velocity;

    //Turn values
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;

    //Ground check variables
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance = 0.4f;
    private bool isGrounded;

    //Sprinting values
    public float speedModifier;
    public float sprintFOVModifier = 1.25f;
    private float playerFOV;

    //Aiming values
    public float mouseSensitivity = 100f;
    public float xRotation = 0f;
    public float yRotation = 0f;
    public bool aiming = false;

    //Other variables
    public bool moving;
    public bool jumping;
    public bool sprinting;

    //References
    public PlayerStats playerStats;
    public CharacterController controller;
    public GameObject weaponEquip;
    public GameObject regularCam;
    public GameObject aimCam;
    public Transform camPos;
    public CinemachineVirtualCamera cinemachineRef;
    public CharacterAnimManager characterAnimManager;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        //Input values
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //<<Start of grounding code>>
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && !aiming)
        {
            if (Input.GetButtonDown("Jump") && playerStats.currentStamina >= 10)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                playerStats.UseStamina(10);
                jumping = true;
            }
        }

        //CHeck to see if player is not falling
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            jumping = false;
        }
        

        //Gravity calculations
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        //<<End of grounding code>>

        //<<Start of sprinting code>>
        bool sprint = Input.GetKey(KeyCode.LeftShift);
        bool isSprinting = sprint;// && verticalInput > 0;
        float speed = baseSpeed;

        if (isSprinting && !aiming && playerStats.currentStamina >= 0.1f && !jumping)
        {
            speed *= speedModifier;
            playerStats.UseStamina(0.1f);
            sprinting = true;
        }

        else
        {
            sprinting = false;
        }
        //<<End of sprinting code>>
        
        //<<Start of movement code>>
        if (!aiming)
        {
            regularCam.SetActive(true);
            aimCam.SetActive(false);

            Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

            if (direction.magnitude >= 0.1f)
            {
                moving = true;

                //Angle where cam is facing
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camPos.eulerAngles.y;

                //Smooth out player rotation
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                //Rotate player based on angle
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                //Move player
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }

            else moving = false;
        }

        else if (aiming)
        {
            aimCam.SetActive(true);
            regularCam.SetActive(false);

            //Calculations
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -30f, 25f);

            //Camera rotation
            aimCam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

            //Player rotation
            this.transform.Rotate(Vector3.up * mouseX);

            Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;
            controller.Move(move * speed * Time.deltaTime);
        }
        //<<End of movement code>>

        //<<Start of aiming code>>
        WeaponScript weaponScript = weaponEquip.GetComponentInChildren<WeaponScript>();
        
        if (Input.GetMouseButton(1) && weaponScript != null)
        {
            aiming = true;
            weaponScript.readyToShoot = true;
        }

        else if (!Input.GetMouseButton(1) && weaponScript != null)
        {
            aiming = false;
            weaponScript.readyToShoot = false;
        }

        else
        {
            aiming = false;
        }
        //<<End of aiming code>>
    }
}
