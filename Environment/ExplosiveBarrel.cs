using UnityEngine;

//Harmful explosive barrel environmental object
public class ExplosiveBarrel : EnvironmentalObjectHealth
{
    //Variables
    public float radius;
    public float damageAmount;
    public LayerMask explosionLayer;
    public GameObject explosionEffect;

    public override void Action()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, explosionLayer);

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
        }

        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    //Visualization
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
