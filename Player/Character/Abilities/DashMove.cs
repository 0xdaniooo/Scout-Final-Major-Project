using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

//Allows the player to dash forwards
public class DashMove : Ability
{
    //Variables
    public float dashForce;

    //References
    public CharacterController characterController;
    public TextMeshProUGUI textDescription;
    public Image img;

    public override void Cast()
    {
        Dash();
        StartCoroutine(Display());
    }

    private void Dash()
    {
        characterController.Move(Camera.main.transform.forward * dashForce * Time.deltaTime);
    }

    private IEnumerator Display()
    {
        textDescription.SetText(AbilityName + " activated");
        textDescription.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        textDescription.gameObject.SetActive(false);

        img.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(AbilityCooldown);
        img.color = new Color32(0, 255, 40, 255);
    }
}
