using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public enum HandPositionType
    {
        Up,
        Middle,
        Down
    }

    [System.Serializable]
    private struct HandPositionSetting
    {
        public HandPositionType positionType;
        public Transform handPoint;
    }

    [SerializeField] private HandPositionSetting[] handPositionSettings = null;
    [SerializeField] private SpriteRenderer hand = null;

    private HandPositionType currentHandPositionType;

    public void MoveHand(bool moveUp)
    {
        switch (currentHandPositionType)
        {
            case HandPositionType.Up:
                if (!moveUp)
                {
                    MoveHand(HandPositionType.Middle);
                }
                break;
            case HandPositionType.Middle:
                if (moveUp)
                {
                    MoveHand(HandPositionType.Up);
                }
                else
                {
                    MoveHand(HandPositionType.Down);
                }
                break;
            case HandPositionType.Down:
                if (moveUp)
                {
                    MoveHand(HandPositionType.Middle);
                }
                break;
        }
    }

    private void MoveHand(HandPositionType newPosition)
    {
        HandPositionSetting? positionSetting =  GetHandPositionForType(newPosition);
        if (positionSetting.HasValue)
        {
            hand.transform.position = positionSetting.Value.handPoint.position;


            currentHandPositionType = newPosition;
        }
    }


    private HandPositionSetting? GetHandPositionForType(HandPositionType positionType)
    {
        foreach (var positionSetting in handPositionSettings)
        {
            if (positionSetting.positionType == positionType)
            {
                return positionSetting;
            }
        }

        Debug.LogError($"Can't find hand setting for type {positionType}");
        return null;
    }

    private void Awake()
    {
        MoveHand(HandPositionType.Middle);
    }
}
