using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RouteData
{
    public bool leadsToCore;
    public List<Transform> waypoints;
}

public class RoadManager : MonoBehaviour
{
    [SerializeField] private List<RouteData> allRoutes = new List<RouteData>();
    public static RoadManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public RouteData GetRandomRouteData()
    {
        if (allRoutes == null || allRoutes.Count == 0) return null;

        int roll = Random.Range(0, 100);

        // 85% Wahrscheinlichkeit für die Hauptroute
        if (roll < 85)
        {
            return allRoutes[0];
        }
        else
        {
            if (allRoutes.Count == 1) return allRoutes[0];

            int randomNumber = Random.Range(1, allRoutes.Count);
            return allRoutes[randomNumber];
        }
    }

    public RouteData GetRouteUsingIndex(int index)
    {
        return allRoutes[index];
    }
}