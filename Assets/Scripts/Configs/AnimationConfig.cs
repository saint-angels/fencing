using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationConfig", menuName = "Config/AnimationConfig")]
public class AnimationConfig : ScriptableObject
{
    [Header("Body view")]
    public float bodyStanceChangeDuration = .3f;
    public Ease bodyStanceChangeEase = Ease.OutCirc;


    public float bodyAttackWarmupMinScaleY = .5f;
    public float bodyAttackWarmupCycleDuration = .5f;
}
