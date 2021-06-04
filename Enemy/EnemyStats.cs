using System.Collections;
using UnityEngine.UI;
using UnityEngine;

//Enemy status and health
public class EnemyStats : CharacterStats
{
    public Slider healthbar;
    private EnemyAI enemy;
    private EnemyAnimManager animManager;

    private void Awake()
    {
        currentHealth = maxHealth;
        healthbar.value = HealthPercentage();

        enemy = GetComponent<EnemyAI>();
        animManager = GetComponent<EnemyAnimManager>();
    }

    public void TakeDamage(float damage)
    {
        if (!enemy.isAlive)
        {
            return;
        }

        currentHealth -= damage;
        healthbar.value = HealthPercentage();
        StartCoroutine(DisplayHealthbar());

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            healthbar.value = 0;
            enemy.isAlive = false;
            StartCoroutine(Death());
        }
    }

    private float HealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    private IEnumerator DisplayHealthbar()
    {
        if (currentHealth <= 0)
        {
            healthbar.gameObject.SetActive(false);
        }

        else
        {
            healthbar.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            healthbar.gameObject.SetActive(false);
        }
    }

    private IEnumerator Death()
    {
        animManager.DeathAnim();
        enemy.DropWeapon();

        ObjectMarker om = GetComponent<ObjectMarker>();
        if (om != null)
        {
            om.DestroyMarker();
        }
        
        yield return new WaitForSeconds(2f);
        KillCountManager.KillManager.KillCount(1);
        Destroy(gameObject);
    }
}
