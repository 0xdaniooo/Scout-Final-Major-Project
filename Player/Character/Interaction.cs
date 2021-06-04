using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Interaction script for the player character
public class Interaction : MonoBehaviour
{
    //Interact values
    public Camera cam;
    public float interactionDistance;
    public LayerMask ignoreLayer;
    private KeyCode interactKey = KeyCode.F;

    //UI components
    public Text interactionText;
    public TextMeshProUGUI interactText;
    public Image interactionProgressImage;
    public GameObject interactionProgressParent;

    //Weapon pickup
    public Transform equipPosition;
    private GameObject currentWeapon;
    private GameObject potentialWeapon;
    private bool canGrab;
    public WeaponScript weaponInUse;

    private void Update()
    {
        WeaponInteractions();

        //Ignore player with raycast
        LayerMask rayLayer = ~ignoreLayer;

        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        bool successfulHit = false;

        if (Physics.Raycast(ray, out hit, interactionDistance, rayLayer))
        {
            //Check for regular interaction
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();

            if (interactable != null)
            {
                //Prepare interaction input
                HandleIntercation(interactable);

                //Prepare text
                interactText.SetText(interactable.GetDescription());

                successfulHit = true;

                //Present hold progress UI if type matches
                interactionProgressParent.SetActive(interactable.interactionType == InteractableObject.InteractionType.Hold);
            }

            //Check for weapon interaction
            else if (hit.transform.tag == "Weapon")
            {
                canGrab = true;
                successfulHit = true;
                potentialWeapon = hit.transform.gameObject;
                interactText.SetText("Press [F] to pick up weapon");
            }

            else
            {
                canGrab = false;
            }
        }

        //No interaction found
        if (!successfulHit)
        {
            interactText.SetText("");

            interactionProgressParent.SetActive(false);
        }
    }

    private void HandleIntercation(InteractableObject interactable)
    {
        switch (interactable.interactionType)
        {
            //Click interaction
            case InteractableObject.InteractionType.Click:
                if (Input.GetKeyDown(interactKey))
                {
                    interactable.Interact();
                }
                break;

            //Hold interaction
            case InteractableObject.InteractionType.Hold:
                if (Input.GetKey(interactKey))
                {
                    interactable.IncreaseHoldTime();

                    if (interactable.GetHoldTime() > 1f)
                    {
                        interactable.Interact();

                        interactable.ResetHoldTime();
                    }

                    else
                    {
                        interactable.ResetHoldTime();
                    }

                    interactionProgressImage.fillAmount = interactable.GetHoldTime();
                }
            break;

            default:
                throw new System.Exception("Unsupported type of interactable, pls fix.");
        }
    }

    private void WeaponInteractions()
    {
        if (canGrab)
        {
            if (Input.GetKeyDown(interactKey))
            {
                if (currentWeapon != null)
                {
                    Drop();
                }

                Pickup();
            }
        }

        if (currentWeapon != null)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Drop();
            }
        }
    }

    private void Pickup()
    {
        //Disable weapon before putting it away
        if (currentWeapon != null)
        {
            weaponInUse = null;
            currentWeapon.GetComponent<WeaponScript>().enabled = false;
        }

        //Prepare new weapon
        currentWeapon = potentialWeapon;
        currentWeapon.GetComponent<WeaponScript>().enabled = true;
        weaponInUse = currentWeapon.GetComponent<WeaponScript>();
        currentWeapon.transform.tag = "Untagged";

        //Positioning weapon
        currentWeapon.transform.position = equipPosition.position;
        currentWeapon.transform.parent = equipPosition;
        currentWeapon.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void Drop()
    {
        //Remove weapon
        currentWeapon.GetComponent<WeaponScript>().Status();
        weaponInUse = null;
        currentWeapon.GetComponent<WeaponScript>().enabled = false;
        currentWeapon.transform.parent = null;
        currentWeapon.GetComponent<Rigidbody>().isKinematic = false;
        currentWeapon.transform.tag = "Weapon";
        currentWeapon = null;
    }
}
