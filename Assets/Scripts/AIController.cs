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

    public void Run()
    {
        aiConfig = Root.ConfigManager.AI;

        StartCoroutine(RoundRoutine());
    }

    private IEnumerator RoundRoutine()
    {
        while (true)
        {
            yield return StartCoroutine(ChoosePosionRoutine());
            yield return StartCoroutine(AttackRoutine());

            yield return new WaitForSeconds(aiConfig.postAttackDelay);
        }
    }


    private IEnumerator ChoosePosionRoutine()
    {
        float finishTime = Time.time + aiConfig.stanceChooseDuration;
        while (Time.time < finishTime)
        {
            yield return new WaitForSeconds(aiConfig.stanceChooseDelay);
            StanceType randomStance = GetRandomStance();
            body.SetStance(randomStance);
        }   
    }

    private IEnumerator AttackRoutine()
    {
        body.SetState(BodyShell.BodyState.ATTACK_WARMUP);
        yield return new WaitForSeconds(aiConfig.attackWarmupDuration);

        OnAttack(body.CurrentStanceType);
    }

    private StanceType GetRandomStance()
    {
        Array values = Enum.GetValues(typeof(StanceType));
        System.Random random = new System.Random();
        return (StanceType)values.GetValue(random.Next(values.Length));
    }
}
