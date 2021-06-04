using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

//Stats for the main player character
public class PlayerStats : CharacterStats
{
    //Life variables
    public bool isAlive = true;
    //public float healthOverTimer;
    public Slider healthbar;
    private Coroutine healthRegen;
    private WaitForSeconds healthTick = new WaitForSeconds(0.1f);

    //Stamina variables
    public float maxStamina = 100;
    public float currentStamina;
    public Slider staminaBar;
    private Coroutine regen;
    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);

    //References
    public GameManager gameManager;
    public DeathScreen deathScreen;

    public TextMeshProUGUI healthText;
    

    private void Start()
    {
        //Set health
        currentHealth = maxHealth;
        healthbar.maxValue = maxHealth;
        healthbar.value = maxHealth;
        healthText.SetText(currentHealth + " / " + maxHealth);

        //Set stamina
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
    }

    private IEnumerator RegenHealth()
    {
        yield return new WaitForSeconds(2f);

        while (currentHealth < maxHealth)
        {
            currentHealth += maxHealth / 100;
            healthbar.value = currentHealth;
            healthText.SetText(currentHealth + " / " + maxHealth);
            yield return healthTick;
        }

        healthRegen = null;
    }

    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(2f);

        while (currentStamina < maxStamina)
        {
            currentStamina += maxStamina / 100;
            staminaBar.value = currentStamina;
            yield return regenTick;
        }

        regen = null;
    }

    public void UseStamina(float amount)
    {
        if (currentStamina - amount >= 0)
        {
            currentStamina -= amount;
            staminaBar.value = currentStamina;

            if (regen != null)
            {
                StopCoroutine(regen);
            }

            regen = StartCoroutine(RegenStamina());
        }
    }

    public void TakeDamage(float damage)
    {
        if (!isAlive)
        {
            return;
        }

        currentHealth -= damage;
        healthbar.value = currentHealth;
        healthText.SetText(currentHealth + " / " + maxHealth);

        if (healthRegen != null)
        {
            StopCoroutine(healthRegen);
        }

        if (currentHealth > 0)
        {
            healthRegen = StartCoroutine(RegenHealth());
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            gameManager.PlayerDeath();
            deathScreen.enabled = true;
        }
    }
}
