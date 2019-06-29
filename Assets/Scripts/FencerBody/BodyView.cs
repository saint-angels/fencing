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

    [SerializeField] private StanceSetting[] stanceSettings = null;
    [SerializeField] private SpriteRenderer arm = null;

    public void Init(BodyShell ownerShell)
    {
        ownerShell.OnStanceChanged += OnStanceChanged;
    }

    private void OnStanceChanged(StanceType newStance)
    {
        StanceSetting? positionSetting = GetStanceSettingForType(newStance);
        if (positionSetting.HasValue)
        {
            arm.transform.localPosition = new Vector3(positionSetting.Value.offsetX, positionSetting.Value.offsetY);
            arm.transform.localRotation = Quaternion.Euler(0, 0, positionSetting.Value.rotationZ);
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
