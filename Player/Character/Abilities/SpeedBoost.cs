using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

//Faster movement speed ability
public class SpeedBoost : Ability
{
    //Variables
    public float boostMultiplier;
    public float boostTime;

    //References
    public CharacterMovement characterMovement;
    public Image img;
    public TextMeshProUGUI textDescription;

    public override void Cast()
    {
        StartCoroutine(Boost());
        StartCoroutine(Display());
    }

    private IEnumerator Boost()
    {
        float speedBackup = characterMovement.baseSpeed;
        characterMovement.baseSpeed *= boostMultiplier;
        yield return new WaitForSeconds(boostTime);
        characterMovement.baseSpeed = speedBackup;
    }

    private IEnumerator Display()
    {
        textDescription.SetText(AbilityName + " activated");
        textDescription.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        textDescription.gameObject.SetActive(false);

        img.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(boostTime + AbilityCooldown);
        img.color = new Color32(0, 255, 40, 255);
    }
}
