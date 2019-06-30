using Promises;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public event Action<StanceType> OnAttack = (stance) => { };

    [SerializeField] private BodyShell body = null;

    private AIConfig aiConfig;
    private int difficultyLevel = 0;

    public void Run()
    {
        aiConfig = Root.ConfigManager.AI;

        StartCoroutine(FightRoutine());
    }

    public void UpdateDifficulty(int difficulty)
    {
        difficultyLevel = Mathf.Min(difficulty, aiConfig.maxDifficulty);
    }

    private IEnumerator FightRoutine()
    {
        while (true)
        {
            yield return StartCoroutine(ChoosePosionRoutine());
            yield return StartCoroutine(AttackRoutine());

            float postAttackDelay = GetAdjastedConfigValue(aiConfig.postAttackDelayDefault, aiConfig.postAttackDelayMin);
            yield return new WaitForSeconds(postAttackDelay);
        }
    }

    private IEnumerator ChoosePosionRoutine()
    {
        float stanceChooseDuration = GetAdjastedConfigValue(aiConfig.stanceChooseDurationDefault, aiConfig.stanceChooseDurationMin);
        float finishTime = Time.time + stanceChooseDuration;
        while (Time.time < finishTime)
        {
            float stanceChooseDelay = GetAdjastedConfigValue(aiConfig.stanceChooseDelayDefault, aiConfig.stanceChooseDelayMin);
            print($"stance choose delay {stanceChooseDelay}");
            yield return new WaitForSeconds(stanceChooseDelay);

            StanceType randomStance = GetRandomStance();
            body.SetStance(randomStance);

            float stancePostChoiceDelay = GetAdjastedConfigValue(aiConfig.stancePostChoiceDelayDefault, aiConfig.stancePostChoiceDelayMin);
            yield return new WaitForSeconds(stancePostChoiceDelay);
        }   
    }

    private IEnumerator AttackRoutine()
    {
        body.SetState(BodyState.ATTACK_WARMUP);
        float attackWarmupDuration = GetAdjastedConfigValue(aiConfig.attackWarmupDurationDefault, aiConfig.attackWarmupDurationMin);
        yield return new WaitForSeconds(attackWarmupDuration);

        body.SetState(BodyState.ATTACKING);
        OnAttack(body.CurrentStance);

        yield return new WaitForSeconds(aiConfig.attackStanceHoldDuration);
        body.SetState(BodyState.IDLE);
    }

    private StanceType GetRandomStance()
    {
        Array values = Enum.GetValues(typeof(StanceType));
        System.Random random = new System.Random();
        return (StanceType)values.GetValue(random.Next(values.Length));
    }

    private float GetAdjastedConfigValue(float defaultValue, float lastValue)
    {
        return Remap(difficultyLevel, 0, aiConfig.maxDifficulty, defaultValue, lastValue);
    }

    private float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = value - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }
}
