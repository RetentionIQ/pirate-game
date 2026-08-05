using UnityEngine;

public class SeparateAbility : Ability
{
    public override float GetCooldownTime()
    {
        return 50f;
    }

    public override void TakeAction()
    {
        // Spawner auf diesen Piraten setzen und Abklingzeit starten
        SpawnerManager.instance.SetCurrentPirate(piratePrefab);
        StartCoroutine(StartCooldown());
    }
}