using System.Collections.Generic;
using UnityEngine;

public class SeparateSpawner : MonoBehaviour
{
    public static SeparateSpawner Instance;
    
    [SerializeField] private float spawnDuration;
    
    private float spawnTimer;
    private Vector3 spawnPosition;
    private Pirate prefab;
    private List<Transform> currentRoute;
    private int waypointIndex;
    private int totalToSpawn;
    private int spawnCount;

    private bool shouldSpawn = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!shouldSpawn) return;

        if (spawnTimer <= 0)
        {
            spawnTimer = spawnDuration;
            spawnCount++;

            // spawnPosition explizit als dritten Parameter übergeben
            SpawnerManager.instance.SpawnAndGetPirate(prefab, spawnPosition)
                .GetPirateMovement()
                .InitializeRouteSeparable(currentRoute, waypointIndex, spawnPosition, false);

            if (spawnCount == totalToSpawn)
            {
                // Direkt auf false setzen, um Fehler beim Umschalten zu vermeiden
                shouldSpawn = false; 
                spawnCount = 0;
            }
        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }
    }

    public void Spawn(Vector3 spawnPosition, Pirate prefab, List<Transform> currentRoute,
                      int waypointIndex, int totalToSpawn, float spawnDuration)
    {
        this.spawnPosition = spawnPosition;
        this.prefab = prefab;
        this.currentRoute = currentRoute;
        this.waypointIndex = waypointIndex;
        this.totalToSpawn = totalToSpawn;
        this.spawnDuration = spawnDuration;
        
        shouldSpawn = !shouldSpawn;
    }
}