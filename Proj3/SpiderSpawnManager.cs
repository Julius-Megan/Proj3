using System.Collections;
using UnityEngine;

public class SpiderSpawnManager : MonoBehaviour
{
    public GameObject prefabToSpawn; // Reference to the prefab you want to spawn
    public Transform spawnPoint; // Point where the prefab will spawn
    public float spawnInterval = 2f; // Time interval between spawns
    public int numberOfPrefabsToSpawn = 1; // Number of prefabs to spawn each time
    public Transform player; // Reference to the player
    public string prefabTag = "spider";

    private bool playerInRange = false; // Flag to check if the player is in range

    private void Start()
    {

        StartCoroutine(SpawnPrefabRoutine()); // Start spawning routine
    }

    private void Update()
    {
        // Check if the player is within range
        if (Vector3.Distance(transform.position, player.position) < 10f) // Change 10f to your desired range
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
    }

    private IEnumerator SpawnPrefabRoutine()
    {
        while (true)
        {
            // Check if the player is in range before spawning
            if (playerInRange)
            {
                float circleRadius = 5f; // Radius of the circular formation
                float angleStep = 360f / numberOfPrefabsToSpawn; // Angle between each prefab

                for (int i = 0; i < numberOfPrefabsToSpawn; i++)
                {
                    // Calculate position in a circle around the spawn point
                    float angle = i * angleStep;
                    Vector3 spawnPosition = spawnPoint.position + Quaternion.Euler(0, angle, 0) * (Vector3.forward * circleRadius);

                    // Spawn the prefabs at calculated positions
                    Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
                    yield return new WaitForSeconds(0.1f); // Delay between each spawn
                }

                // Wait until all spawned prefabs are destroyed before continuing
                yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag(prefabTag).Length == 0);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
