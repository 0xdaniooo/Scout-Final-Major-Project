using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

//Ability which temporarily increases player max health
public class HealthBoost : Ability
{
    //Referenecs
    public PlayerStats playerStats;
    public Image img;
    public TextMeshProUGUI textDescription;

    //Variables  
    public float healthBoost;
    public float boostTime;

    public override void Cast()
    {
        StartCoroutine(Boost());
        StartCoroutine(Display());
    }

    private IEnumerator Boost()
    {
        float healthBackup = playerStats.maxHealth;
        playerStats.maxHealth += healthBoost;
        playerStats.healthbar.maxValue += healthBoost;
        playerStats.TakeDamage(0);
        yield return new WaitForSeconds(boostTime);
        playerStats.maxHealth = healthBackup;
        playerStats.healthbar.maxValue = healthBackup;
        playerStats.currentHealth -= healthBoost;
        playerStats.TakeDamage(0);
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
