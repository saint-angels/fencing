using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BodyShell : MonoBehaviour
{
    public event Action<StanceType> OnStanceChanged = (stanceType) => { };
    public event Action<BodyState> OnBodyStateChanged = (bodyState) => { };
    public event Action<bool> OnBlockFinished = (success) => { };

    public StanceType CurrentStance { get; private set; }

    [SerializeField] private BodyView bodyView = null;

    public void Init(Faction faction)
    {
        bodyView.Init(this);
        Root.UIManager.SetupBodyHUD(bodyView, faction);

        SetState(BodyState.IDLE);
        SetStance(StanceType.MIDDLE);
    }

    public void BlockFinished(bool success)
    {
        OnBlockFinished(success);
    }

    public void SetState(BodyState newState)
    {
        OnBodyStateChanged(newState);
        SetStance(CurrentStance);
    }

    //TODO: Return promise ?
    public void SetStance(bool changeUp)
    {
        switch (CurrentStance)
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
        CurrentStance = newStance;
        OnStanceChanged(CurrentStance);
    }
}
