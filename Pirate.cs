using UnityEngine;
using DG.Tweening; 

public class Pirate : MonoBehaviour
{
    [SerializeField] private PirateData pirateData;

    public float currentHealth;
    public int currentDamage;
    public float currentSpeed;

    private PirateMovement pirateMovement;

    [Header("Separated Pirates")]
    public bool isSeparable = false;
    [SerializeField] private SeparateSpawner separateSpawner;

    [Header("Security Blocker Pirates")]
    public bool isBlocker = false;
    public bool isAbility = false;

    [Header("Animation Settings")]
    [SerializeField] private float punchAmount = 0.25f;
    [SerializeField] private float punchDuration = 0.12f;
    [SerializeField] private int punchVibrato = 6;
    [SerializeField] private float punchElasticity = 1f;
    [SerializeField] private AudioClip popSound;

    private Vector3 baseScale = Vector3.one;

    private void Awake()
    {
        pirateMovement = GetComponent<PirateMovement>();
        baseScale = transform.localScale;
    }

    public void SetBaseScale(Vector3 newScale)
    {
        baseScale = newScale;
        transform.localScale = baseScale;
    }

    public void TakeDamage(Projectile projectile)
    {
        SoundManager.instance.PlayAudio(popSound);
        
        // Schaden berechnen basierend auf dem Piratentyp
        if (pirateData.pirateType == projectile.GetPirateType() && !isAbility)
        {
            currentHealth -= projectile.GetDamage() * projectile.damageMultiplier;
        }
        else
        {
            currentHealth -= projectile.GetDamage();
        }

        if (currentHealth <= 0)
        {
            HandleDeathAbilities(projectile);
            TutorialManager.instance.AddToCounter();
            projectile.OnPirateDied(this);
            Die(); 
        }
        else
        {
            PlayHitBounce();
        }
    }

    private void HandleDeathAbilities(Projectile projectile)
    {
        if (isSeparable)
        {
            separateSpawner.Spawn(transform.position, SpawnerManager.instance.GetCurrentPirate(), GetPirateMovement().currentRoute, GetPirateMovement().waypointIndex, 3, 0.3f);
            separateSpawner.transform.parent = null;
        }
        if (isBlocker) 
        {
            projectile.HackIt(3f);
        }
    }

    private void Die()
    {
        transform.DOKill(complete: true); 
        transform.localScale = baseScale; 

        // Zurück in den Object Pool anstatt zu zerstören
        if (SpawnerManager.instance != null)
        {
            SpawnerManager.instance.ReturnPirateToPool(this);
        }
        else
        {
            gameObject.SetActive(false); 
        }
    }

    private void PlayHitBounce()
    {
        transform.DOKill(complete: true);
        transform.localScale = baseScale;
        transform.DOPunchScale(Vector3.one * punchAmount, punchDuration, punchVibrato, punchElasticity);
    }

    private void OnDisable()
    {
        transform.DOKill();
    }

    public int GetDamage() => pirateData.damage;
    public PirateData GetPirateData() => pirateData;
    public PirateMovement GetPirateMovement() => pirateMovement;
}