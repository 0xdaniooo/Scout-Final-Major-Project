using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

//Higher jump height ability
public class JumpBoost : Ability
{
    public CharacterMovement characterMovement;
    public Image img;
    public TextMeshProUGUI textDescription;

    public float jumpMultiplier;
    public float boostTime;

    public override void Cast()
    {
        StartCoroutine(Boost());
        StartCoroutine(Display());
    }

    private IEnumerator Boost()
    {
        float jumpBackup = characterMovement.jumpHeight;
        characterMovement.jumpHeight += jumpMultiplier;
        yield return new WaitForSeconds(boostTime);
        characterMovement.jumpHeight = jumpBackup;
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
