using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyView : MonoBehaviour
{
    [System.Serializable]
    private struct StanceSetting
    {
        public StanceType stanceType;
        public float offsetX;
        public float offsetY;
        public float rotationZ;
    }

    public Transform HUDPoint => hudPoint;

    [SerializeField] private StanceSetting[] stanceSettings = null;
    [SerializeField] private SpriteRenderer arm = null;
    [SerializeField] private Transform hudPoint = null;

    private AnimationConfig animationCfg;

    public void Init(BodyShell ownerShell)
    {
        animationCfg = Root.ConfigManager.Animation;

        ownerShell.OnStanceChanged += OnStanceChanged;
        ownerShell.OnBodyStateChanged += OnBodyStateChanged;
    }

    private void OnBodyStateChanged(BodyShell.BodyState newState)
    {
        //Kill all previous body animations
        transform.DOKill(true);

        switch (newState)
        {
            case BodyShell.BodyState.IDLE:
                break;
            case BodyShell.BodyState.ATTACK_WARMUP:
                //Repeat scaling until killed
                var scaleTween = transform.DOScaleY(animationCfg.bodyAttackWarmupMinScaleY, animationCfg.bodyAttackWarmupCycleDuration).SetEase(Ease.InOutFlash, 2, 0);
                scaleTween.OnComplete(() => scaleTween.Restart());
                break;
            default:
                break;
        }
    }

    private void OnStanceChanged(StanceType newStance)
    {
        StanceSetting? positionSetting = GetStanceSettingForType(newStance);
        if (positionSetting.HasValue)
        {
            arm.transform.DOKill(true);

            Vector3 armTargetPosition = new Vector3(positionSetting.Value.offsetX, positionSetting.Value.offsetY);
            arm.transform.DOLocalMove(armTargetPosition, animationCfg.bodyStanceChangeDuration).SetEase(animationCfg.bodyStanceChangeEase);

            Vector3 armTargetRotation = new Vector3(0, 0, positionSetting.Value.rotationZ);
            arm.transform.DOLocalRotate(armTargetRotation, animationCfg.bodyStanceChangeDuration).SetEase(animationCfg.bodyStanceChangeEase);
        }
    }

    private StanceSetting? GetStanceSettingForType(StanceType positionType)
    {
        foreach (var positionSetting in stanceSettings)
        {
            if (positionSetting.stanceType == positionType)
            {
                return positionSetting;
            }
        }

        Debug.LogError($"Can't find hand setting for type {positionType}");
        return null;
    }
}
