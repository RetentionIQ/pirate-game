using UnityEngine;
using TMPro;

public class LevelUI : MonoBehaviour
{
    public string levelKey = "Level";
    
    private TextMeshProUGUI levelTextUI;
    private string levelPrefix = "Level ";
    private int currentLevel;

    private void Awake()
    {
        levelTextUI = GetComponent<TextMeshProUGUI>();
        currentLevel = PlayerPrefs.GetInt(levelKey, 1);
    }

    private void Start()
    {
        LevelManager.instance.OnLevelSwitch += HandleLevelSwitch;
        UpdateLevelUI(currentLevel);
    }

    private void HandleLevelSwitch(object sender, int level)
    {
        UpdateLevelUI(level);
    }

    // Aktualisiert die Textanzeige mit dem neuen Level
    public void UpdateLevelUI(int level)
    {
        levelTextUI.text = levelPrefix + level;
    }
}