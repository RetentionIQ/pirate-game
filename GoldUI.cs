using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;

    private void Start()
    {
        GoldManager.Instance.OnGoldUpdated += UpdateGoldText;
    }

    // Textfeld mit dem neuen Goldwert aktualisieren
    private void UpdateGoldText(object sender, int goldAmount)
    {
        goldText.text = goldAmount.ToString();
    }
}