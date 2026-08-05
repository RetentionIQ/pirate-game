using UnityEngine;

public class SpawnerManagerUI : MonoBehaviour
{
    [SerializeField] private ChooseColorButton chooseColorButtonPrefab;
    
    private void Start()
    {
        foreach(Pirate pirate in SpawnerManager.instance.GetPiratesPrefabs())
        {
            ChooseColorButton chooseColorButton = Instantiate(chooseColorButtonPrefab, transform);
            TutorialManager.instance.chooseColorButtons.Add(chooseColorButton);

            chooseColorButton.SetPirate(pirate);
        }
        
        // TutorialManager.instance.UpdateTutorial(); // Auskommentiert, um Probleme mit der Skript-Priorität zu vermeiden
    }
}