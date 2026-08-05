using System.Collections;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public Pirate piratePrefab;
    public Sprite abilitySprite;
    public bool isOnCooldown = false;

    public abstract float GetCooldownTime();
    public abstract void TakeAction();

    // Startet die Abklingzeit
    public IEnumerator StartCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(GetCooldownTime());
        isOnCooldown = false;
    }
}