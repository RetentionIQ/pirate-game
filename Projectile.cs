using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Pirate target;
    public int damageMultiplier = 2;
    
    private float damage;
    private Cannon sourceCannon;
    private float timeToReach = 4f;

    void Update()
    {
        // Prüfen, ob das Ziel zerstört wurde oder im Pool ist
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            if (sourceCannon != null)
            {
                sourceCannon.ReturnProjectile(this); 
            }
            else
            {
                Destroy(gameObject); 
            }
            return;
        }

        Vector3 offset = new Vector3(0, 0.1f, 0);
        Vector3 targetPosition = target.transform.position + offset;
        
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, timeToReach * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPosition) < 0.05f)
        {
            target.TakeDamage(this);

            if (sourceCannon != null)
            {
                sourceCannon.ReturnProjectile(this);
            }
        }
    }

    public void SetTarget(Pirate target, float damage, Cannon sourceCannon)
    {
        this.target = target;
        this.sourceCannon = sourceCannon;
        this.damage = damage;
    }

    public void OnPirateDied(Pirate pirate) => sourceCannon.OnPirateDied(pirate);
    public PirateType GetPirateType() => sourceCannon.GetPirateType();
    public Cannon GetItsCannon() => sourceCannon;
    public float GetDamage() => damage;
    public void HackIt(float duration) => sourceCannon.HackIt(duration);
}