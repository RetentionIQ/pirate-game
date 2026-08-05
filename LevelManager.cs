using System;
using UnityEngine;
using DG.Tweening; 

public class LevelManager : MonoBehaviour
{
    public float cannonDamage = 0.6f; 
    public static LevelManager instance;
    
    public string LEVEL { get; private set; } = "Level";
    public int currentLvl { get; private set; }

    public GameObject[] LevelPrefabs;
    private GameObject spawnedLevel;
    private int lastSpawnedPrefabIndex = -1;

    public event EventHandler<int> OnLevelSwitch;

    public Sprite[] differentBgs;
    public SpriteRenderer bgSpriteRenderer;

    [Header("Animation Settings")]
    [SerializeField] private float mapSpawnDuration = 0.6f;

    private void Awake()
    {
        instance = this;
        currentLvl = PlayerPrefs.GetInt(LEVEL, 1);
    }

    private void Start()
    {
        SpawnMap();
    }

    private void ClearAllActivePirates()
    {
        Pirate[] activePirates = FindObjectsOfType<Pirate>();
        foreach (Pirate pirate in activePirates)
        {
            Destroy(pirate.gameObject);
        }
    }

    public void PassedALevel()
    {
        currentLvl++;
        PlayerPrefs.SetInt(LEVEL, currentLvl);
        
        if (spawnedLevel != null)
        {
            // Vorheriges Level sauber zerstören
            spawnedLevel.transform.DOKill();
            Destroy(spawnedLevel); 
        }
        
        GoldManager.Instance.PassedALevel(currentLvl);
        ClearAllActivePirates();
        SpawnMap();
    }

    private void SpawnMap()
    {
        int maxCurrentLevels = 15;
        int levelIndex = 0;

        // Schwierigkeitsgrad nach Level 15 skalieren
        if (currentLvl > maxCurrentLevels)
        {
            int scalingPhase = (currentLvl - 16) / 5;
            cannonDamage = 1.0f + (scalingPhase * 0.1f);

            if (differentBgs != null && differentBgs.Length > 0)
            {
                int bgIndex = scalingPhase % differentBgs.Length;
                bgSpriteRenderer.sprite = differentBgs[bgIndex];
            }

            int minPrefabIndex = 5;
            int maxPrefabIndex = 15;

            do
            {
                levelIndex = UnityEngine.Random.Range(minPrefabIndex, maxPrefabIndex);
            }
            while (levelIndex == lastSpawnedPrefabIndex);

            lastSpawnedPrefabIndex = levelIndex;
        }
        else
        {
            cannonDamage = 0.6f;
            levelIndex = currentLvl - 1;
        }

        OnLevelSwitch?.Invoke(this, currentLvl);
        
        spawnedLevel = Instantiate(LevelPrefabs[levelIndex], Vector3.zero, Quaternion.identity);
        AnimateMapSpawn();
    }

    private void AnimateMapSpawn()
    {
        if (spawnedLevel == null) return;

        spawnedLevel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        spawnedLevel.transform.DOScale(Vector3.one, mapSpawnDuration).SetEase(Ease.OutBack);
    }
}