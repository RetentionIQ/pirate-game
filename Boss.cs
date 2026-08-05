using System;
using UnityEngine;
using DG.Tweening;

public class BossAttackedEventArgs : EventArgs
{
    public int CurrentHealth { get; set; }
    public int MaxHealth { get; set; }
}

public class Boss : MonoBehaviour
{
    public static Boss instance;

    public int maxHealth = 16;
    public Transform bossSkinTransform;
    public event EventHandler<BossAttackedEventArgs> OnAttacked;
    
    [Header("Juice / Animation Settings")]
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private Vector3 hitScalePunch = new Vector3(0.15f, 0.15f, 0f);
    [SerializeField] private float animationDuration = 0.15f;
    [SerializeField] private int vibrato = 5;
    [SerializeField] private float elasticity = 0.3f;

    private int currentHealth;
    private Vector3 originalRootScale;

    private void Awake()
    {
        instance = this;
        originalRootScale = transform.localScale;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        SpinBoss();
    }

    // Kollision mit einem Piraten überprüfen
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Pirate pirate))
        {
            TakeDamage(pirate.GetDamage());
            GameObject splash = Instantiate(pirate.GetPirateData().splashPrefab, pirate.transform.position, Quaternion.identity);
            splash.transform.parent = bossSkinTransform;
        }
    }

    private void SpinBoss()
    {
        if (bossSkinTransform != null)
        {
            bossSkinTransform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        TutorialManager.instance.AddToCounter();
        
        OnAttacked?.Invoke(this, new BossAttackedEventArgs { CurrentHealth = currentHealth, MaxHealth = maxHealth });

        if (currentHealth <= 0)
        {
            DestroyCore();
        }
        else
        {
            PlayHitAnimation();
        }
    }

    private void PlayHitAnimation()
    {
        transform.DOKill(complete: true);
        transform.localScale = originalRootScale;
        transform.DOPunchScale(hitScalePunch, animationDuration, vibrato, elasticity);
    }

    // Boss besiegen und Level beenden
    public void DestroyCore()
    {
        transform.DOKill();
        if (bossSkinTransform != null) bossSkinTransform.DOKill();

        LevelManager.instance.PassedALevel();
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        transform.DOKill();
        if (bossSkinTransform != null) bossSkinTransform.DOKill();
    }
}