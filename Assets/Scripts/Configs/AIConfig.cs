using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIConfig", menuName = "Config/AIConfig")]
public class AIConfig : ScriptableObject
{
    [Header("Choosing position")]
    public float stanceChooseDuration = 2f;
    public float stanceChooseDelay = 1f;

    [Header("Attacking")]
    public float attackWarmupDuration = 3f;


    public float postAttackDelay = 3f;
}
