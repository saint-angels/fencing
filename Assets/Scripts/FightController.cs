using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour
{
    public event Action<bool> OnUserBlock = (success) => { };

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
        bool blockSuccess = userBody.CurrentStance == aiAttackStance;
        userBody.BlockFinished(blockSuccess);

        OnUserBlock(blockSuccess);

        if (blockSuccess)
        {
            print($"Player is not hit");
            //Shake camera
            //Spawn particles
        }
        else
        {
            print($"Player is hit");
        }
    }
}
