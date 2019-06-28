using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIConfig", menuName = "Config/AIConfig")]
public class AIConfig : ScriptableObject
{
    public float decisionDelay = 1f;
}
