using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Cannon : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private float range = 0.3f;
    [SerializeField] private float timeToShoot = 0.3f;
    [SerializeField] private float damage = 0.6f;

    [Header("References")]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    public SpriteRenderer cannonSpriteRenderer;

    [Header("State Tracking")]
    [SerializeField] private PirateType lastPirateKilledType = PirateType.BLUE_PIRATE;
    private Pirate currentPirate;
    private float shootTimer;
    private bool isWorking = true;

    [Header("Hacking UI")]
    [SerializeField] private GameObject hackedUIHolder;
    [SerializeField] private Image hackedTimeImage;
    private float hackedTimer;
    private float maxHackedTime;

    [Header("Animation Settings")]
    [SerializeField] private float recoilDistance = 0.25f;
    [SerializeField] private float recoilDuration = 0.2f;
    [SerializeField] private Vector2 localRecoilDirection = new Vector2(-1f, 0f);
    [SerializeField] private Vector3 barrelBulgeAmount = new Vector3(0.15f, 0.35f, 0f);
    [SerializeField] private float bulgeDuration = 0.15f;

    [SerializeField] private bool damageAdjustedForTutorial = false;

    private Vector3 originalVisualScale;
    private Vector3 originalVisualLocalPosition;
    private Transform visualTransform;

    private Queue<Projectile> projectilePool = new Queue<Projectile>();

    private void Awake()
    {
        cannonSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        if (cannonSpriteRenderer != null)
        {
            visualTransform = cannonSpriteRenderer.transform;
            originalVisualScale = visualTransform.localScale;
            originalVisualLocalPosition = visualTransform.localPosition;
        }
    }

    private void Start()
    {
        if (!damageAdjustedForTutorial)
        {
            damage = LevelManager.instance.cannonDamage; 
        }
        
        foreach (Pirate pirate in SpawnerManager.instance.piratesPrefab)
        {
            if (pirate.GetPirateData().pirateType == lastPirateKilledType)
            {
                cannonSpriteRenderer.color = pirate.GetPirateData().pirateColor;
            }
        }
    }

    private void Update()
    {
        HandleShooting();
        HandleHacking();
    }

    private void HandleShooting()
    {
        if (shootTimer <= 0 && isWorking)
        {
            Shoot();
        }
        else
        {
            shootTimer -= Time.deltaTime;
        }
    }

    private void HandleHacking()
    {
        if (isWorking) return;

        if (hackedTimer <= 0)
        {
            hackedUIHolder.SetActive(false);
            isWorking = true;
        }
        else
        {
            hackedTimer -= Time.deltaTime;
            hackedTimeImage.fillAmount = hackedTimer / maxHackedTime;
        }
    }

    // Schießt auf den nächsten Piraten in Reichweite
    private void Shoot()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, range);
        
        if (hit && hit.TryGetComponent(out Pirate pirate))
        {
            currentPirate = pirate;
            Projectile currentProjectile;

            if (projectilePool.Count > 0)
            {
                currentProjectile = projectilePool.Dequeue();
                currentProjectile.transform.position = projectileSpawnPoint.position;
                currentProjectile.gameObject.SetActive(true);
            }
            else
            {
                currentProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            }

            currentProjectile.SetTarget(pirate, damage, this);
            shootTimer = timeToShoot;

            PlayShootAnimation();
        }
        else
        {
            currentPirate = null;
        }
    }

    public void ReturnProjectile(Projectile returningProjectile)
    {
        returningProjectile.gameObject.SetActive(false);
        projectilePool.Enqueue(returningProjectile);
    }

    private void PlayShootAnimation()
    {
        if (visualTransform == null) return;

        visualTransform.DOKill(complete: true);
        visualTransform.localScale = originalVisualScale;
        visualTransform.localPosition = originalVisualLocalPosition;

        Vector3 worldRecoilVector = transform.TransformDirection(localRecoilDirection).normalized;
        visualTransform.DOPunchPosition(worldRecoilVector * recoilDistance, recoilDuration, vibrato: 0, elasticity: 0);
        visualTransform.DOPunchScale(barrelBulgeAmount, bulgeDuration, vibrato: 2, elasticity: 0.2f);
    }

    public Pirate GetTargetedPirate() => currentPirate;
    public PirateType GetPirateType() => lastPirateKilledType;

    public void OnPirateDied(Pirate pirate)
    {
        cannonSpriteRenderer.color = pirate.GetPirateData().pirateColor;
        lastPirateKilledType = pirate.GetPirateData().pirateType;
    }

    // Kanone kurzzeitig deaktivieren
    public void HackIt(float duration)
    {
        isWorking = false;
        hackedUIHolder.SetActive(true);
        maxHackedTime = duration;
        hackedTimer = duration;
    }

    private void OnDisable()
    {
        if (visualTransform != null)
        {
            visualTransform.DOKill();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}