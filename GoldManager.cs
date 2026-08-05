using System;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }

    public event EventHandler<int> OnGoldUpdated;

    [SerializeField] private float timeToGiveGold = 0.3f;
    
    private int currentGold = 0;
    private float goldTimer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        goldTimer = timeToGiveGold;
        LevelManager.instance.OnLevelSwitch += HandleLevelSwitch;
        
        OnGoldUpdated?.Invoke(this, currentGold);
    }

    private void HandleLevelSwitch(object sender, int level)
    {
        if (LevelManager.instance.currentLvl == 3)
        {
            currentGold = 70;
            OnGoldUpdated?.Invoke(this, currentGold);
        }
    }

    private void Update()
    {
        HandleCheats();

        if (LevelManager.instance.currentLvl <= 3) return;

        goldTimer -= Time.deltaTime;

        if (goldTimer <= 0)
        {
            goldTimer = timeToGiveGold;
            
            // Passives Einkommen generieren
            int goldPerTick = Mathf.Max(1, LevelManager.instance.currentLvl / 2);
            AddGold(goldPerTick);
        }
    }

    private void HandleCheats()
    {
        // Nur für Testzwecke
        if (Input.GetKeyDown(KeyCode.G)) AddGold(1000); 
        if (Input.GetKeyDown(KeyCode.D) && Boss.instance != null) Boss.instance.DestroyCore();
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        OnGoldUpdated?.Invoke(this, currentGold);
    }

    public void SubtractGold(int amount)
    {
        currentGold -= amount;
        OnGoldUpdated?.Invoke(this, currentGold);
    }

    public void PassedALevel(int currentLvl)
    {
        if (LevelManager.instance.currentLvl <= 3) return;
        
        int bonus = currentLvl * 10;
        AddGold(bonus);
    }

    public int GetGold() => currentGold;
}