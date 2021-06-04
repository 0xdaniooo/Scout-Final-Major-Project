using UnityEngine;

//Script for moving objects based on waypoints
public class MovingObject : EnvironmentalObjectHealth
{
    //Variables
    public Transform[] waypoints;
    public float speed;
    private int waypointIndex;
    private float dist;

    private void Start()
    {
        waypointIndex = 0;
        transform.LookAt(waypoints[waypointIndex].position);
    }

    private void Update()
    {
        PresetPatrolling();

        if (destroyed)
        {
            Destroy(gameObject);
        }
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
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void PresetIncreaseIndex()
    {
        waypointIndex ++;

        if (waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }

        transform.LookAt(waypoints[waypointIndex].position);
    }

    public override void Action()
    {
        Destroy(gameObject);
    }
}
