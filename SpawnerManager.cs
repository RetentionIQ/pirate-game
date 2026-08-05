using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager instance;
    
    [SerializeField] public Pirate[] piratesPrefab;
    
    private int totalAddedDamage;
    private float totalSpawnRateModifier;
    private float totalAddedHealth;
    private float totalAddedSpeed;
    
    private float spawnTimer;
    private float timeToSpawn;
    
    private Pirate currentPirate;
    private Pirate previousPirate;

    // --- Object Pooling Dictionary ---
    private Dictionary<string, Queue<Pirate>> piratePool = new Dictionary<string, Queue<Pirate>>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        previousPirate = currentPirate;
        currentPirate = piratesPrefab[0];

        totalAddedHealth = PlayerPrefs.GetFloat("BonusHealth", 0);
        totalAddedDamage = PlayerPrefs.GetInt("BonusDamage", 0);
        totalAddedSpeed = PlayerPrefs.GetFloat("BonusSpeed", 0f);
        totalSpawnRateModifier = PlayerPrefs.GetFloat("BonusSpawnRate", 0f);

        timeToSpawn = currentPirate.GetPirateData().timeToSpawn + totalSpawnRateModifier;
        spawnTimer = timeToSpawn;
    }

    private void Update()
    {
        if (spawnTimer <= 0)
        {
            spawnTimer = timeToSpawn;
            SpawnPirate(currentPirate);
        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }
    }

    public Pirate[] GetPiratesPrefabs() => piratesPrefab;

    public void SetCurrentPirate(Pirate newPirate)
    {
        if (currentPirate != null && currentPirate.isAbility) return;
        
        previousPirate = currentPirate;
        currentPirate = newPirate;
    }

    public void SpawnRateUpgrade(float amount)
    {
        if (timeToSpawn <= 0.1f) return;
        
        totalSpawnRateModifier += amount;
        timeToSpawn = currentPirate.GetPirateData().timeToSpawn + totalSpawnRateModifier;
        PlayerPrefs.SetFloat("BonusSpawnRate", totalSpawnRateModifier);
    }

    public void HealthUpgrade(float amount)
    {
        totalAddedHealth += amount;
        PlayerPrefs.SetFloat("BonusHealth", totalAddedHealth);
    }

    public void DamageUpgrade(int amount)
    {
        totalAddedDamage += amount;
        PlayerPrefs.SetInt("BonusDamage", totalAddedDamage);
    }

    public void SpeedUpgrade(float amount)
    {
        totalAddedSpeed += amount;
        PlayerPrefs.SetFloat("BonusSpeed", totalAddedSpeed);
    }

    public void SpawnPirate(Pirate pirateToSpawn)
    {
        Pirate spawnedPirate = GetPirateFromPool(pirateToSpawn, transform.position);

        if (pirateToSpawn.isAbility)
        {
            currentPirate = previousPirate;
        }
    }

    public Pirate SpawnAndGetPirate(Pirate pirateToSpawn, Vector3 spawnPos)
    {
        return GetPirateFromPool(pirateToSpawn, spawnPos);
    }

    private Pirate GetPirateFromPool(Pirate prefab, Vector3 spawnPos)
    {
        string poolKey = prefab.GetPirateData().pirateName;

        // Queue initialisieren, falls dieser Piratentyp noch nicht im Pool ist
        if (!piratePool.ContainsKey(poolKey))
        {
            piratePool[poolKey] = new Queue<Pirate>();
        }

        Pirate pirateInstance;

        // Prüfen, ob ein inaktiver Pirat verfügbar ist
        if (piratePool[poolKey].Count > 0)
        {
            pirateInstance = piratePool[poolKey].Dequeue();
            pirateInstance.transform.position = spawnPos;
            pirateInstance.gameObject.SetActive(true);
        }
        else
        {
            // Falls der Pool leer ist, einen neuen instanziieren
            pirateInstance = Instantiate(prefab, spawnPos, Quaternion.identity);
        }

        // Gesammelte Upgrades anwenden
        pirateInstance.currentHealth = pirateInstance.GetPirateData().health + totalAddedHealth;
        pirateInstance.GetPirateMovement().SetSpeed(totalAddedSpeed);
        pirateInstance.currentDamage = pirateInstance.GetPirateData().damage + totalAddedDamage;

        // Pfad zurücksetzen, damit recycelte Einheiten ihre alten Routen vergessen
        pirateInstance.GetPirateMovement().ResetPathing();

        return pirateInstance;
    }

    public void ReturnPirateToPool(Pirate pirateToReturn)
    {
        string poolKey = pirateToReturn.GetPirateData().pirateName;

        // Sicherheitsprüfung, falls ein Pirat zurückgegeben wird, bevor seine Queue existiert
        if (!piratePool.ContainsKey(poolKey))
        {
            piratePool[poolKey] = new Queue<Pirate>();
        }

        pirateToReturn.gameObject.SetActive(false);
        piratePool[poolKey].Enqueue(pirateToReturn);
    }

    public Pirate GetCurrentPirate()
    {
        if (currentPirate != null && currentPirate.isAbility)
        {
            // Vorläufige Lösung
            return piratesPrefab[0]; 
        }
        return currentPirate;
    }

    public bool IsCurrentPirateAbility() => currentPirate.isAbility;
}