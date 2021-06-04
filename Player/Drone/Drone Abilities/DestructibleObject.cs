using UnityEngine;

//Adds explsoive functinoality to objects
public class DestructibleObject : MonoBehaviour
{
    //Variables
    public float damageAmount;
    public float radius;
    private bool hasCollided;
    private bool hasExploded = false;
    public LayerMask ignoreLayer;
    public LayerMask explodeLayer;
    public GameObject explosionEffect;

    private void Update()
    {
        LayerMask explodeLayer = ~ignoreLayer;

        hasCollided = Physics.CheckSphere(transform.position, 2f, explodeLayer);

        if (hasCollided)
        {
            if (!hasExploded)
            {
                Explosion();
                hasExploded = true;
            }
        }
    }

    private void Explosion()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, explodeLayer);

        foreach (Collider hit in colliders)
        {
            PlayerStats playerStats = hit.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                playerStats.TakeDamage(damageAmount);
            }

            EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

            if (enemyStats != null)
            {
                enemyStats.TakeDamage(damageAmount);
            }

            EnvironmentalObjectHealth objHealth = hit.GetComponent<EnvironmentalObjectHealth>();

            if (objHealth != null)
            {
                objHealth.TakeDamage(damageAmount);
            }
        }

        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
