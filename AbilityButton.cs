using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public Button abilityButton;
    public Ability currentAbility;
    
    [SerializeField] private Image abilityImage;
    [SerializeField] private Image cooldownOverlay;

    private bool isUnderCooldown = false;
    private float currentCooldownTime;
    private float maxCooldownTime;

    private void Awake()
    {
        abilityButton = GetComponent<Button>();
    }

    private void Start()
    {
        cooldownOverlay.fillAmount = 0;
    }

    private void Update()
    {
        if (!isUnderCooldown) return;

        currentCooldownTime -= Time.deltaTime;

        if (currentCooldownTime <= 0)
        {
            isUnderCooldown = false;
            cooldownOverlay.fillAmount = 0;
        }
        else
        {
            // Visuelles Feedback für die Abklingzeit
            cooldownOverlay.fillAmount = currentCooldownTime / maxCooldownTime;
        }
    }

    public void SetCurrentUpgrade(Ability ability)
    {
        currentAbility = ability;
        abilityImage.sprite = currentAbility.abilitySprite;

        abilityButton.onClick.AddListener(() =>
        {
            if (isUnderCooldown || SpawnerManager.instance.IsCurrentPirateAbility()) return;
            
            isUnderCooldown = true;
            maxCooldownTime = currentAbility.GetCooldownTime();
            currentCooldownTime = maxCooldownTime;
            
            currentAbility.TakeAction();
        });
    }
}