using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private AbilityButton abilityButtonPrefab;

    void Start()
    {
        // Alle Fähigkeiten laden und Buttons erstellen
        foreach (Ability ability in GetComponents<Ability>())
        {
            AbilityButton buttonInstance = Instantiate(abilityButtonPrefab, transform);
            TutorialManager.instance.abilityButtons.Add(buttonInstance);
            buttonInstance.SetCurrentUpgrade(ability);
        }
    }
}