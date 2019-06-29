﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationConfig", menuName = "Config/AnimationConfig")]
public class AnimationConfig : ScriptableObject
{
    [Header("Body")]
    public float attackWarmupMinScaleY = .5f;
}
