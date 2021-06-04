using System.Collections;
using UnityEngine;

//Makes dynamic trees that can be collapsed
public class TreeCollapse : EnvironmentalObjectHealth
{
    //Variables
    public GameObject centerOfMass;
    public float force;
    public float radius;
    private float posRange = 2f;

    //Promixity damage variables
    public float damageAmount;
    public float damageRadius;
    public  LayerMask layer;

    public override void Action()
    {
        float posX = Random.Range(-posRange, posRange);
        float posZ = Random.Range(-posRange, posRange);

        centerOfMass.transform.localPosition = new Vector3(posX, transform.localPosition.y, posZ);

        StartCoroutine(TreeFall());
    }

    private void ProximityDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius, layer);

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
    }

    private IEnumerator TreeFall()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.AddExplosionForce(force, centerOfMass.transform.position, radius, 3f);
        yield return new WaitForSeconds(5);

        ProximityDamage();
        rb.isKinematic = true;
    }
}
