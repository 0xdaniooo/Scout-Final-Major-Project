using UnityEngine;

//Manages enemy count and spawns new enemies
public class EnemySpawnManager : MonoBehaviour
{
    //References
    public GameObject[] enemies;
    public GameObject spawnParent;
    public int currentEnemyCount;
    public int maxEnemyCount;
    public PresetPath pathToWalk;

    //Variables
    private float counter;
    private int rand;
    private float startTimeSpawn;
    public float currentTimer;
    public float spawnTime;

    private void Start()
    {
        AdvanceTick();

        startTimeSpawn = spawnTime;
        currentTimer = startTimeSpawn;
    }

    private void Update()
    {
        if (currentTimer <= 0)
        {
            if (currentEnemyCount < maxEnemyCount)
            {
                rand = Random.Range(0, enemies.Length);
                GameObject enemy = Instantiate(enemies[rand], transform.position, Quaternion.Euler(0f, 180f, 0f));
                enemy.GetComponent<EnemyAI>().waypoints = pathToWalk;
                enemy.transform.parent = spawnParent.transform;
                currentEnemyCount = spawnParent.transform.childCount;
                currentTimer = startTimeSpawn;
            }

            else
            {
                currentTimer = startTimeSpawn;
            }
        }

        else
        {
            currentTimer -= Time.deltaTime;
        }

        AdvanceTick();
    }

    private void AdvanceTick()
    {
        if (Time.time > counter)
        {
            currentEnemyCount = spawnParent.transform.childCount;
            counter = Time.time + 5f;
        }
    }
}
