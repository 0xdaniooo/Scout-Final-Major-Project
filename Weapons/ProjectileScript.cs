using UnityEngine;

//Base script for projectiles shot from weapons
public class ProjectileScript : MonoBehaviour
{
    [Header("Regular projectile values")]
    public float damage;
    public float range;
    public float speed;
    public int predictionStepsPerFrame = 6;

    [Header("Explosive projectile values")]
    public bool explosive;
    public float radius;
    public LayerMask explosionLayer;
    public GameObject explosionEffect;
    private Vector3 bulletVelocity;
    public GameObject bulletHole;

    private void Start()
    {
        bulletVelocity = this.transform.forward * speed;
    }

    private void Update()
    {
        Vector3 point1 = this.transform.position;

        float stepSize = 1.0f / predictionStepsPerFrame;

        for (float step = 0; step < 1; step += stepSize)
        {
            //Apply physics, can be replaced with drag and wind physics
            bulletVelocity += Physics.gravity * stepSize * Time.deltaTime;

            Vector3 point2 = point1 + bulletVelocity * stepSize * Time.deltaTime;

            Ray ray = new Ray(point1, point2 - point1);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, (point2 - point1).magnitude))
            {
                if (explosive)
                {
                    Explosion();
                }
                
                else
                {
                    //Check if any object with health was hit
                    if (PlayerHit(hit)) return;
                    else if (EnemyHit(hit)) return;
                    else if (ObjectHit(hit)) return;

                    //Else spawn bullet hole
                    else
                    {
                        GameObject newHole = Instantiate(bulletHole, hit.point + hit.normal * 0.001f, Quaternion.identity) as GameObject;
                        newHole.transform.LookAt(hit.point + hit.normal);
                        newHole.transform.parent = hit.collider.GetComponent<Transform>();
                        Destroy(gameObject);
                    }

                    Destroy(gameObject);
                }
            }

            point1 = point2;
        }

        this.transform.position = point1;

        Destroy(gameObject, 4f);
    }

    private void Explosion()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, explosionLayer);

        foreach (Collider hit in colliders)
        {
            PlayerStats playerStats = hit.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
            }

            EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

            if (enemyStats != null)
            {
                EnemyAI enemyScript = enemyStats.GetComponent<EnemyAI>();

                if (!enemyScript.playerDetected)
                {
                    enemyStats.TakeDamage(damage * 1.5f);
                }

                else enemyStats.TakeDamage(damage);
            }

            EnvironmentalObjectHealth objectHealth = hit.GetComponent<EnvironmentalObjectHealth>();

            if (objectHealth != null)
            {
                objectHealth.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

    private bool PlayerHit(RaycastHit hitInfo)
    {
        if (hitInfo.collider.CompareTag("Player"))
        {
            PlayerStats playerStats = hitInfo.collider.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
                return true;
            }
        }

        return false;
    }

    private bool EnemyHit(RaycastHit hitInfo)
    {
        if (hitInfo.collider.CompareTag("Enemy"))
        {
            EnemyStats enemyStats = hitInfo.collider.GetComponent<EnemyStats>();

            if (enemyStats != null)
            {
                EnemyAI enemyScript = enemyStats.GetComponent<EnemyAI>();

                if (!enemyScript.playerDetected)
                {
                    enemyStats.TakeDamage(damage * 1.5f);

                    return true;
                }

                else 
                {
                    enemyStats.TakeDamage(damage);

                    return true;
                }
            }
        }

        return false;
    }

    private bool ObjectHit(RaycastHit hitInfo)
    {
        if (hitInfo.collider.CompareTag("EnvironmentalObject"))
        {
            EnvironmentalObjectHealth objectHealth = hitInfo.collider.GetComponent<EnvironmentalObjectHealth>();

            if (objectHealth != null)
            {
                objectHealth.TakeDamage(damage);
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 point1 = this.transform.position;
        Vector3 predictedBulletVelocity = bulletVelocity;

        float stepSize = 0.01f;
        for (float step = 0; step < 1; step += stepSize)
        {
            predictedBulletVelocity += Physics.gravity * stepSize;
            Vector3 point2 = point1 + predictedBulletVelocity * stepSize;
            Gizmos.DrawLine(point1, point2);
            point1 = point2;
        }
    }
}
