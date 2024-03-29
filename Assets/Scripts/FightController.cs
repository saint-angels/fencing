﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour
{
    public event Action<int> OnUserBlock = (combo) => { };

    [SerializeField] private AIController aiController = null;
    [SerializeField] private BodyShell userBody = null;
    [SerializeField] private BodyShell enemyBody = null;

    private int userBlockCombo = 0;

    public void Run()
    {
        userBody.Init(Faction.USER);
        enemyBody.Init(Faction.ENEMY);

        aiController.OnAttack += AIController_OnAttack;

        //Don't run enemy ai until user input
        Root.PlayerInput.OnFirstButtonPressed += PlayerInput_OnFirstButtonPressed;
    }

    private void PlayerInput_OnFirstButtonPressed()
    {
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
        aiController.UpdateDifficulty(userBlockCombo);

        OnUserBlock(userBlockCombo);
    }
}
