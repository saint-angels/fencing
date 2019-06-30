using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour
{
    public event Action<uint> OnUserBlock = (combo) => { };

    [SerializeField] private AIController aiController = null;
    [SerializeField] private BodyShell userBody = null;
    [SerializeField] private BodyShell enemyBody = null;

    private uint userBlockCombo = 0;

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


        if (blockSuccess)
        {
            print($"Player is not hit");
            userBlockCombo++;
        }
        else
        {
            userBlockCombo = 0;
            print($"Player is hit");
        }

        OnUserBlock(userBlockCombo);
    }
}
