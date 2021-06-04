using System.Collections;
using UnityEngine.AI;
using UnityEngine;

//Base script for enemies
public class EnemyAI : MonoBehaviour
{
    [Header("Random Patrolling")]
    public float walkPointRange;
    public bool walkPointSet;
    public Vector3 walkPoint;

    [Header("Preset Patrolling")]
    public PresetPath waypoints;
    private int waypointIndex;
    private float dist;

    [Header("Detection")]
    public float detectionRadius;
    public float attackRange;
    public float alertRange;
    private float minimumDetectionAngle = -50f, maximumDetectionAngle = 50f;
    public bool playerInDetectionRadius, playerInAttackRange, playerDetected;
    public bool moving;

    [Header("Attack")]
    public float timeBetweenAttacks;
    public int maxAmmo;
    private int currentAmmo;
    public float reloadTime;
    public bool isAlive = true;

    //References
    private NavMeshAgent agent;
    private Transform player;
    public GameObject[] weapons;
    private GameObject weapon;
    public GameObject weaponToDrop;
    public Transform equipLocation;
    public EnemyWeapon enemyWeapon;
    public LayerMask whatIsGround, whatIsPlayer;

    private void Awake()
    {
        //Assign references
        player = GameObject.Find("Obj_Character").transform;
        agent = GetComponent<NavMeshAgent>();

        //Randomly choose weapon
        int weaponTouse = Random.Range(0, weapons.Length);
        weapon = weapons[weaponTouse];

        //Spawn weapon
        GameObject chosenWeapon = Instantiate(weapon, equipLocation.transform.position, Quaternion.Euler(0f, 100f, 0f));
        chosenWeapon.transform.parent = equipLocation;
        weapon = chosenWeapon;

        //Prepare enemy weapon
        enemyWeapon = chosenWeapon.GetComponent<EnemyWeapon>();
        //currentAmmo = maxAmmo;
    }

    private void Start()
    {
        waypointIndex = 0;
        transform.LookAt(waypoints.points[waypointIndex].position);
    }

    private void Update()
    {
        EnemyLoop();
    }

    //Execute AI functionality
    private void EnemyLoop()
    {
        if (isAlive)
        {
            Detection();

            if (!playerDetected)
            {
                if (!playerInDetectionRadius)
                {
                    PresetPatrolling();
                }
                
                else
                {
                    RandomPatrolling();
                }
            }

            else
            {
                AttackPlayer();
                RandomPatrolling();
                AlertOtherEnemies();
            }
        }
    }

    //Walk to randomly generated spots
    private void RandomPatrolling()
    {
        if (!walkPointSet) 
        {
            SearchRandomPatrolling();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            moving = true;
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
            moving = false;
        }
    }

    //Calculate random spot
    private void SearchRandomPatrolling()
    {
        float randomZ;
        float randomX;

        if (!playerDetected)
        {
            randomZ = Random.Range(-walkPointRange, walkPointRange);
            randomX = Random.Range(-walkPointRange, walkPointRange);
        }

        else
        {
            randomZ = Random.Range((-walkPointRange / 2), (walkPointRange / 2));
            randomX = Random.Range((-walkPointRange / 2), (walkPointRange / 2));
        }

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 1f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    //Patrol preset chosen points
    private void PresetPatrolling()
    {
        dist = Vector3.Distance(transform.position, waypoints.points[waypointIndex].position);

        if (dist < 1f)
        {
            PresetIncreaseIndex();
        }

        PresetPatrolWalk();
    }

    //Walk to preset chosen points
    private void PresetPatrolWalk()
    {
        agent.SetDestination(waypoints.points[waypointIndex].position);
    }

    //Incement preset patrol point index
    private void PresetIncreaseIndex()
    {
        waypointIndex ++;

        if (waypointIndex >= waypoints.points.Length)
        {
            waypointIndex = 0;
        }

        transform.LookAt(waypoints.points[waypointIndex].position);
    }

    //Player detection loop
    private void Detection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, whatIsPlayer);

        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        Collider[] playerCollider = Physics.OverlapSphere(transform.position, detectionRadius, whatIsPlayer);

        for (int j = 0; j < playerCollider.Length; j++)
        {
            if (playerCollider[j].tag == "Player") playerInDetectionRadius = true;
            else playerInDetectionRadius = false;
        }

        if (playerInDetectionRadius)
        {
            for(int i = 0; i < colliders.Length; i++)
            {
                PlayerStats playerStats = colliders[i].transform.GetComponent<PlayerStats>();

                if (playerStats != null)
                {
                    Vector3 targetDirection = playerStats.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if (viewableAngle > minimumDetectionAngle && viewableAngle < maximumDetectionAngle)
                    {
                        playerDetected = true;
                    }
                }
            }
        }

        else
        {
            playerDetected = false;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }

    private void AttackPlayer()
    {
        if (playerInAttackRange)
        {
            agent.SetDestination(transform.position);
            transform.LookAt(player);
            enemyWeapon.WeaponInput();
        }

        else
        {
            ChasePlayer();
        }
    }

    //Alert other nearby enemies to the player
    private void AlertOtherEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, alertRange, whatIsPlayer);

        for(int i = 0; i < colliders.Length; i++)
        {
            EnemyAI enemyAI = colliders[i].transform.GetComponent<EnemyAI>();

            if (enemyAI != null)
            {
                enemyAI.playerDetected = true;
            }
        }
    }

    public void DropWeapon()
    {
        weapon.transform.parent = null;
        weapon.gameObject.tag = "Weapon";
        weapon.GetComponent<Rigidbody>().isKinematic = false;
    }

    //Visualization
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Vector3 fovLine1 = Quaternion.AngleAxis(maximumDetectionAngle, transform.up) * transform.forward * detectionRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(minimumDetectionAngle, transform.up) * transform.forward * detectionRadius;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, alertRange);
    }
}
