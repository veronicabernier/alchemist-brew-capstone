using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToCloserX : MonoBehaviour
{
    public enum SettingType
    {
        Small,
        Medium,
        Large
    }
    [System.Serializable]
    public class Setting
    {
        public SettingType name;
        public float snapPointX;
    }

    [LabeledArrayAttribute(typeof(SettingType))]
    public Setting[] settings = new Setting[3];

    private bool snapIsActive = true;
    private SettingType curSetting = SettingType.Large;


    public void OnMouseUp()
    {
        if (snapIsActive)
        {
            OnDragEnded();
        }
    }

    private void OnDragEnded()
    {
        float curMinDistance = float.PositiveInfinity;
        int curMinDistanceIndex = 0;
        for (int i = 0; i < settings.Length; i++)
        {
            float curDistance = Mathf.Abs(transform.localPosition.x - settings[i].snapPointX);

            if (curDistance < curMinDistance)
            {
                curMinDistance = curDistance;
                curMinDistanceIndex = i;
            }
        }
        transform.localPosition = new Vector3(settings[curMinDistanceIndex].snapPointX, transform.localPosition.y, transform.localPosition.z);
        curSetting = settings[curMinDistanceIndex].name;
    }

    public void DeactivateSnap()
    {
        this.GetComponent<Drag>().dragIsActive = false;
        snapIsActive = false;
        foreach (SpriteRenderer sr in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.4f);
        }
    }

    public SettingType GetSettingUsed()
    {
        return curSetting;
    }

}
