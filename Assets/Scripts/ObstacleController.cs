using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject[] obstacles; // Array of obstacle prefabs
    public float spawnDistance = 5f; // Distance from player to spawn obstacles

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        foreach (GameObject obstacle in obstacles)
        {
            if (Vector2.Distance(player.transform.position, obstacle.transform.position) < spawnDistance)
            {
                ActivateObstacle(obstacle);
            }
        }
    }

    private void ActivateObstacle(GameObject obstacle)
    {
        // Enable the obstacle
        obstacle.SetActive(true);

        // Optional: Add specific behaviors for different obstacles
        // Example for fire: obstacle.GetComponent<FireBehavior>().Ignite();
    }
}
