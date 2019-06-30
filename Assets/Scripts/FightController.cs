using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour
{
    [SerializeField] private AIController aiController = null;
    [SerializeField] private BodyShell userBody = null;
    [SerializeField] private BodyShell enemyBody = null;

    public void Run()
    {
        userBody.Init(Faction.USER);
        enemyBody.Init(Faction.ENEMY);

        aiController.OnAttack += AIController_OnAttack;
        aiController.Run();
    }

    private void AIController_OnAttack(StanceType aiAttackStance)
    {
        bool playerHit = userBody.CurrentStanceType != aiAttackStance;
        if (playerHit)
        {
            print($"Player is hit");
        }
        else
        {
            print($"Player is not hit");
            //Shake camera
            //Spawn particles
        }
    }
}
