using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform hudContainer = null;
    [SerializeField] private Transform userBodyHUD = null;

    private Transform userBodyHUDPoint = null;

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
