using UnityEngine;

public class CannonMovement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float angleOffset = 0f;
    [SerializeField] private Transform cannonSpriteTransform;

    private Cannon cannon;

    private void Awake()
    {
        cannon = GetComponent<Cannon>();
    }

    private void Update()
    {
        if (cannon.GetTargetedPirate() != null)
        {
            AimAtTarget();
        }
    }

    // Kanone in Richtung des Piraten drehen
    private void AimAtTarget()
    {
        Vector2 direction = cannon.GetTargetedPirate().transform.position - cannonSpriteTransform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;
        
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
        
        cannonSpriteTransform.rotation = Quaternion.Slerp(
            cannonSpriteTransform.rotation, 
            targetRotation, 
            rotationSpeed * Time.deltaTime
        );
    }
}