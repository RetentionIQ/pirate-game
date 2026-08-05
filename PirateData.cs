using UnityEngine;

[CreateAssetMenu()]
public class PirateData : ScriptableObject
{
    public string pirateName;
    public GameObject splashPrefab;
    public Pirate piratePrefab;
    public Sprite pirateIconSprite;
    public Color pirateColor;
    public PirateType pirateType;
    
    // Basiswerte
    public int health;
    public int damage;
    public float speed;
    public float timeToSpawn;
    public float intelligence;
}