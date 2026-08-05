using UnityEngine;
using UnityEngine.UI;

public class ChooseColorButton : MonoBehaviour
{
    public Pirate SelectedPirate { get; private set; }
    public Button ButtonComponent { get; private set; }
    
    [SerializeField] private Image buttonImage;

    public void SetPirate(Pirate pirate)
    {
        SelectedPirate = pirate;
        ButtonComponent = GetComponent<Button>();
        
        buttonImage.color = pirate.GetPirateData().pirateColor;
        ButtonComponent.onClick.AddListener(SwitchPirate);
    }

    // Spawner auf diesen Piraten umstellen
    public void SwitchPirate()
    {
        SpawnerManager.instance.SetCurrentPirate(SelectedPirate);
    }
}