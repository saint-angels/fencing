using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform hudContainer = null;
    [SerializeField] private Transform userBodyHUD = null;
    [SerializeField] private TMPro.TextMeshProUGUI fenceCommandLabel = null;
    [SerializeField] private TMPro.TextMeshProUGUI blockComboLabel = null;

    [SerializeField] private Image keysPromptImage = null;

    private Transform userBodyHUDPoint;
    private AnimationConfig animationCfg;

    public void Init()
    {
        animationCfg = Root.ConfigManager.Animation;

        blockComboLabel.text = "0";

        //Hide fence command label
        fenceCommandLabel.enabled = false;

        Root.FightController.OnUserBlock += FightController_OnUserBlock;
        Root.PlayerInput.OnFirstButtonPressed += OnUserFirstButtonPressed;
    }

    private void OnUserFirstButtonPressed()
    {
        keysPromptImage.DOFade(0, animationCfg.buttonsPromptFadeDuration).SetEase(Ease.InOutQuad);

        fenceCommandLabel.enabled = true;
        fenceCommandLabel.rectTransform.DOLocalMoveY(animationCfg.fenceLabelHideOffsetY, animationCfg.fenceLabelHideDuration)
        .SetEase(animationCfg.fenceLabelHideEase)
        .OnComplete(() => fenceCommandLabel.enabled = false);
    }

    private void FightController_OnUserBlock(int blockCombo)
    {
        blockComboLabel.text = blockCombo.ToString();
        blockComboLabel.transform.DOShakePosition(animationCfg.blockComboLabelShakeDuration,
                                                    animationCfg.blockComboLabelShakeStrength,
                                                    animationCfg.blockComboLabelShakeVibrato,
                                                    animationCfg.blockComboLabelShakeRandomness);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FightController_OnUserBlock(1);
        }
    }

    public void SetupBodyHUD(BodyView bodyView, Faction faction)
    {
        switch (faction)
        {
            case Faction.USER:
                userBodyHUDPoint = bodyView.HUDPoint;
                break;
            case Faction.ENEMY:
                break;
        }
    }

    private void LateUpdate()
    {
        if (userBodyHUDPoint != null)
        {
            Vector2 screenPoint = Root.CameraController.WorldToScreenPoint(userBodyHUDPoint.position);
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(hudContainer, screenPoint, null, out localPoint))
            {
                userBodyHUD.localPosition = localPoint;
            }
        }
    }
}
