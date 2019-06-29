using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BodyShell : MonoBehaviour
{
    public event Action<StanceType> OnStanceChanged = (stanceType) => { };

    public enum BodyState
    {
        IDLE,
        ATTACK_WARMUP,
    }

    public StanceType CurrentStanceType { get; private set; }

    [SerializeField] private BodyView bodyView = null;

    private AnimationConfig animationCfg;

    public void Init()
    {
        animationCfg = Root.ConfigManager.Animation;

        bodyView.Init(this);

        SetStance(StanceType.MIDDLE);
    }

    public void SetState(BodyState newState)
    {
        switch (newState)
        {
            case BodyState.IDLE:
                break;
            case BodyState.ATTACK_WARMUP:
                //DOTween.Sequence().repe
                //transform.DOScaleY(animationCfg.attackWarmupMinScaleY, 1f).SetEase(Ease.InOutFlash, 2, 0);
                break;
        }
    }

    //TODO: Return promise ?
    public void SetStance(bool changeUp)
    {
        switch (CurrentStanceType)
        {
            case StanceType.HIGH:
                if (!changeUp)
                {
                    SetStance(StanceType.MIDDLE);
                }
                break;
            case StanceType.MIDDLE:
                if (changeUp)
                {
                    SetStance(StanceType.HIGH);
                }
                else
                {
                    SetStance(StanceType.LOW);
                }
                break;
            case StanceType.LOW:
                if (changeUp)
                {
                    SetStance(StanceType.MIDDLE);
                }
                break;
        }
    }

    public void SetStance(StanceType newStance)
    {
        CurrentStanceType = newStance;
        OnStanceChanged(CurrentStanceType);
    }
}
