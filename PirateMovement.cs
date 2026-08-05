using System.Collections.Generic;
using UnityEngine;

public class PirateMovement : MonoBehaviour
{
    [Header("Pirate Stats")]
    public float health;
    public float speed;
    public float damage;
    public bool isGenius;

    [Header("Movement Logic")]
    public List<Transform> currentRoute;
    public int waypointIndex = 0;
    public bool hasReachedEnd = false;

    private Pirate pirate;
    private bool isSmall = false;

    private void Awake()
    {
        pirate = GetComponent<Pirate>();
        speed = pirate.GetPirateData().speed;
        damage = pirate.GetPirateData().damage;
    }

    private void Start()
    {
        if (isSmall) return;
        
        if (pirate.isAbility)
        {
            InitializeRoute(RoadManager.Instance.GetRouteUsingIndex(0).waypoints);
            return;
        }
        InitializeRoute(RoadManager.Instance.GetRandomRouteData().waypoints);
    }

    public void InitializeRoute(List<Transform> points)
    {
        currentRoute = points;
        waypointIndex = 0;
        hasReachedEnd = false;
        isSmall = false; 

        transform.position = points[0].position; 
    }

    public void InitializeRouteSeparable(List<Transform> points, int waypointIndex, Vector3 spawnPosition, bool hasReachedEnd = false)
    {
        currentRoute = points;
        isSmall = true;
        this.waypointIndex = waypointIndex;
        this.hasReachedEnd = hasReachedEnd;

        // Position explizit setzen, wo die Elterneinheit gestorben ist
        transform.position = spawnPosition;
    }

    void Update()
    {
        if (currentRoute == null || hasReachedEnd) return;
        Move();
    }

    void Move()
    {
        Transform target = currentRoute[waypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Sprite basierend auf der Bewegungsrichtung spiegeln
        Vector3 direction = target.position - transform.position;

        if (direction.x > 0.01f) 
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < -0.01f) 
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            waypointIndex++;
            if (waypointIndex >= currentRoute.Count)
            {
                hasReachedEnd = true;
                OnReachDestination();
            }
        }
    }

    void OnReachDestination()
    {
        if (SpawnerManager.instance != null)
        {
            SpawnerManager.instance.ReturnPirateToPool(pirate);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void ResetPathing()
    {
        waypointIndex = 0;
        hasReachedEnd = false;
        isSmall = false; 

        if (RoadManager.Instance != null)
        {
            if (pirate != null && pirate.isAbility)
            {
                InitializeRoute(RoadManager.Instance.GetRouteUsingIndex(0).waypoints);
            }
            else
            {
                InitializeRoute(RoadManager.Instance.GetRandomRouteData().waypoints);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetSpeed(float bonusSpeed)
    {
        float baseSpeed = pirate.GetPirateData().speed;
        this.speed = baseSpeed + bonusSpeed;
    }
}