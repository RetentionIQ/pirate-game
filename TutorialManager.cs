using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    public List<ChooseColorButton> chooseColorButtons = new List<ChooseColorButton>();
    public List<AbilityButton> abilityButtons = new List<AbilityButton>();
    public List<UpgradeButton> upgradeButtons = new List<UpgradeButton>();

    [SerializeField] private GameObject[] tutorialPanels;

    private int currentLevel;
    private int previousLevel = -1;
    private int deadPiratesCount;
    private int maxDeadPirates = 2;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentLevel = PlayerPrefs.GetInt(LevelManager.instance.LEVEL, 1);
        LevelManager.instance.OnLevelSwitch += HandleLevelSwitch;

        UpdateUIInteractability();

        if (currentLevel == 10) UpdateTutorial();
    }

    private void HandleLevelSwitch(object sender, int level)
    {
        currentLevel = level;
        deadPiratesCount = 0;

        UpdateUIInteractability();

        if (currentLevel == 10) UpdateTutorial();
    }

    // Schaltet UI-Elemente abhängig vom aktuellen Level frei
    private void UpdateUIInteractability()
    {
        switch (currentLevel)
        {
            case 1:
                SetInteractableState(false, false, false, false);
                break;
            case 2:
                SetInteractableState(true, false, false, false);
                break;
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
                bool upgradesOn = currentLevel >= 4;
                SetInteractableState(true, true, false, upgradesOn);
                break;
            default:
                SetInteractableState(true, true, true, true);
                break;
        }
    }

    private void SetInteractableState(bool colorsOn, bool firstAbilityOn, bool allAbilitiesOn, bool upgradesOn)
    {
        foreach (ChooseColorButton btn in chooseColorButtons) btn.button.interactable = colorsOn;
        foreach (UpgradeButton btn in upgradeButtons) btn.button.interactable = upgradesOn;

        for (int i = 0; i < abilityButtons.Count; i++)
        {
            if (i == 0)
            {
                abilityButtons[i].button.interactable = firstAbilityOn || allAbilitiesOn;
            }
            else
            {
                abilityButtons[i].button.interactable = allAbilitiesOn;
            }
        }
    }

    public void UpdateTutorial()
    {
        int currentLevelIndex = currentLevel - 1;

        switch (currentLevel)
        {
            case 1:
                foreach (ChooseColorButton colorButton in chooseColorButtons)
                {
                    bool isGreenPirate = colorButton.pirate.GetPirateData().pirateType == PirateType.GREEN_PIRATE;
                    if (!isGreenPirate) continue;

                    ShowTutorialPanel(currentLevelIndex);
                    colorButton.button.interactable = true;

                    colorButton.button.onClick.RemoveListener(ClearTutorials);
                    colorButton.button.onClick.AddListener(ClearTutorials);
                }
                break;

            case 2:
                foreach (AbilityButton abilityButton in abilityButtons)
                {
                    bool isExplodingPirate = abilityButton.currentAbility is BigExplosionAbility;
                    if (!isExplodingPirate) continue;

                    ShowTutorialPanel(currentLevelIndex);
                    abilityButton.button.interactable = true;

                    abilityButton.button.onClick.RemoveListener(ClearTutorials);
                    abilityButton.button.onClick.AddListener(ClearTutorials);
                }
                break;

            case 3:
                foreach (UpgradeButton upgradeButton in upgradeButtons)
                {
                    bool isHealthUpgrade = upgradeButton.currentUpgrade is HealthUpgrade;
                    if (!isHealthUpgrade) continue;

                    ShowTutorialPanel(currentLevelIndex);
                    upgradeButton.button.interactable = true;

                    upgradeButton.button.onClick.RemoveListener(ClearTutorials);
                    upgradeButton.button.onClick.AddListener(ClearTutorials);
                }
                break;

            case 10:
                for (int i = 1; i < abilityButtons.Count; i++)
                {
                    AbilityButton abilityButton = abilityButtons[i];

                    ShowTutorialPanel(currentLevelIndex);
                    abilityButton.button.interactable = true;

                    abilityButton.button.onClick.RemoveListener(ClearTutorials);
                    abilityButton.button.onClick.AddListener(ClearTutorials);
                }
                break;
        }
    }

    private void ShowTutorialPanel(int index)
    {
        if (index < tutorialPanels.Length && tutorialPanels[index] != null)
        {
            tutorialPanels[index].SetActive(true);
        }
        else
        {
            Debug.LogWarning("Tutorial panel for this level is missing in the inspector!");
        }
    }

    // Alle Tutorial-Panels ausblenden
    private void ClearTutorials()
    {
        foreach (GameObject tutorialPanel in tutorialPanels)
        {
            if (tutorialPanel != null) tutorialPanel.SetActive(false);
        }
    }

    public void AddToCounter()
    {
        if (currentLevel > 3) return;

        deadPiratesCount++;

        if (deadPiratesCount < maxDeadPirates) return;
        if (currentLevel == previousLevel) return;

        UpdateTutorial();
        previousLevel = currentLevel;
        deadPiratesCount = 0;
    }
}