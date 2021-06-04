using UnityEngine.AI;
using UnityEngine;

//Moves the player around the menu
public class MenuPlayerTravel : MonoBehaviour
{
    /*public Transform[] waypoints;
    public float speed;
    private int waypointIndex;
    private float dist;
    private NavMeshAgent agent;

    private void Start()
    {
        waypointIndex = 0;
        transform.LookAt(waypoints[waypointIndex].position);

        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        PresetPatrolling();
    }

    private void PresetPatrolling()
    {
        dist = Vector3.Distance(transform.position, waypoints[waypointIndex].position);

        if (dist < 1f)
        {
            PresetIncreaseIndex();
        }

        PresetPatrolWalk();
    }

    private void PresetPatrolWalk()
    {
        agent.SetDestination(waypoints[waypointIndex].position);
    }

    private void PresetIncreaseIndex()
    {
        waypointIndex ++;

        if (waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }

        transform.LookAt(waypoints[waypointIndex].position);
    }*/

    public float walkPointRange;
    public bool walkPointSet;
    public Vector3 walkPoint;
    private NavMeshAgent agent;
    public LayerMask whatIsGround;
    private Animator anim;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        anim.Play("WalkAnim");
    }

    private void Update()
    {
        RandomPatrolling();
    }

    private void RandomPatrolling()
    {
        if (!walkPointSet)
        {
            SearchRandomPatrolling();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchRandomPatrolling()
    {
        float randomZ;
        float randomX;

        randomZ = Random.Range(-walkPointRange, walkPointRange);
        randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 1f, whatIsGround))
        {
            walkPointSet = true;
        }
    }
}
