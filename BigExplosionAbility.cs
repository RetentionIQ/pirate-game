using UnityEngine;

public class BigExplosionAbility : Ability
{
    public override float GetCooldownTime()
    {
        return 30f; 
    }

    public override void TakeAction()
    {
        SpawnerManager.instance.SetCurrentPirate(piratePrefab);
        StartCoroutine(StartCooldown());
    }
}