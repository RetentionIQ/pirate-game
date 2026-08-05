using UnityEngine;

public class SecurityBlockerAbility : Ability
{
    public override float GetCooldownTime()
    {
        return 25f;
    }

    public override void TakeAction()
    {
        // Wählt den Blocker-Piraten aus und startet den Cooldown
        SpawnerManager.instance.SetCurrentPirate(piratePrefab);
        StartCoroutine(StartCooldown());
    }
}