using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    public AIConfig AI => aiConfig;
    public AnimationConfig Animation => animationConfig;

    [SerializeField] private AIConfig aiConfig = null;
    [SerializeField] private AnimationConfig animationConfig = null;

}
