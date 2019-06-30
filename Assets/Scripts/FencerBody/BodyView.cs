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
        public float armOffsetX;
        public float armOffsetY;
        public float armRotationZ;
    }

    public Transform HUDPoint => hudPoint;

    [SerializeField] private Transform hudPoint = null;

    [Header("Idle")]
    [SerializeField] private StanceSetting[] idleStanceSettings = null;
    [SerializeField] private Transform idleSpriteContainer = null;
    [SerializeField] private SpriteRenderer idleArm = null;

    [Header("Attacking")]
    [SerializeField] private StanceSetting[] attackingStanceSettings = null;
    [SerializeField] private Transform attackingSpriteContainer = null;
    [SerializeField] private SpriteRenderer attackingArm = null;

    [Header("VFX")]
    [SerializeField] private ParticleSystem blockSuccessVFX = null;

    private SpriteRenderer CurrentArm
    {
        get
        {
            switch (currentState)
            {
                case BodyState.IDLE:
                case BodyState.ATTACK_WARMUP:
                    return idleArm;
                case BodyState.ATTACKING:
                    return attackingArm;
                default:
                    Debug.LogWarning($"Body {gameObject.name} doesn't have an arm sprite for state {currentState}!");
                    return idleArm;
            }
        }
    }

    private AnimationConfig animationCfg;
    private BodyState currentState;

    public void Init(BodyShell ownerShell)
    {
        animationCfg = Root.ConfigManager.Animation;

        ownerShell.OnStanceChanged += OnStanceChanged;
        ownerShell.OnBodyStateChanged += OnBodyStateChanged;
        ownerShell.OnBlockFinished += OnBlockFinished;
    }

    private void OnBlockFinished(bool blockSuccess)
    {
        if (blockSuccess && blockSuccessVFX != null)
        {
            blockSuccessVFX.Play();
        }
    }

    private void OnBodyStateChanged(BodyState newState)
    {
        currentState = newState;
        //Kill all previous body animations
        transform.DOKill(true);

        idleSpriteContainer.gameObject.SetActive(false);
        attackingSpriteContainer.gameObject.SetActive(false);

        switch (newState)
        {
            case BodyState.IDLE:
                idleSpriteContainer.gameObject.SetActive(true);
                break;
            case BodyState.ATTACK_WARMUP:
                idleSpriteContainer.gameObject.SetActive(true);
                //Repeat scaling until its killed
                var scaleTween = transform.DOScaleY(animationCfg.bodyAttackWarmupMinScaleY, animationCfg.bodyAttackWarmupCycleDuration).SetEase(Ease.InOutFlash, 2, 0);
                scaleTween.OnComplete(() => scaleTween.Restart());
                break;
            case BodyState.ATTACKING:
                attackingSpriteContainer.gameObject.SetActive(true);
                break;
            default:
                Debug.LogWarning($"Unknown body state {newState}");
                break;
        }
    }

    private void OnStanceChanged(StanceType newStance)
    {
        idleArm.transform.DOKill(true);
        attackingArm.transform.DOKill(true);

        StanceSetting? positionSetting = GetStanceSettingForType(newStance);

        if (positionSetting.HasValue)
        {
            Vector3 armTargetPosition = new Vector3(positionSetting.Value.armOffsetX, positionSetting.Value.armOffsetY);
            Vector3 armTargetRotation = new Vector3(0, 0, positionSetting.Value.armRotationZ);
            if (currentState != BodyState.ATTACKING)
            {
                CurrentArm.transform.DOLocalMove(armTargetPosition, animationCfg.bodyStanceChangeDuration).SetEase(animationCfg.bodyStanceChangeEase);   
                CurrentArm.transform.DOLocalRotate(armTargetRotation, animationCfg.bodyStanceChangeDuration).SetEase(animationCfg.bodyStanceChangeEase);
            }
            else
            {
                CurrentArm.transform.localRotation = Quaternion.Euler(armTargetRotation);
                CurrentArm.transform.localPosition = armTargetPosition;
            }
        }
    }

    private StanceSetting? GetStanceSettingForType(StanceType positionType)
    {
        StanceSetting[] stanceSettings = null;
        switch (currentState)
        {
            case BodyState.IDLE:
            case BodyState.ATTACK_WARMUP:
                stanceSettings = idleStanceSettings;
                break;
            case BodyState.ATTACKING:
                stanceSettings = attackingStanceSettings;
                break;
            default:
                Debug.LogWarning($"Unknown state {currentState}");
                return null;
        }

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
