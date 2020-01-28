using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelect : MonoBehaviour
{
    [SerializeField] private Image[] selectModeImage = { };
    [SerializeField] private RectTransform[] selectModeTransform = { };
    private int selectNo = 0;
    private float interval = 0.25f;
    private bool selectCoolDown = false;
    private float coolDownTimMeasure = 0;
    private float notActiveSizeMag = 0.8f;
    private List<Vector2> selectModeSize = new List<Vector2>();

    private void Start()
    {
        foreach(var trans in selectModeTransform)
        {
            selectModeSize.Add(trans.sizeDelta);
        }
    }

    private void ResetSelect()
    {
        selectNo = 0;
        selectCoolDown = false;
    }

    private void Select()
    {
        float horizontalRaw = InputManager.Instance.HorizontalRaw;

        if (selectCoolDown)
        {
            return;
        }

        if (horizontalRaw > 0.75f)
        {
            selectCoolDown = true;
            NotActiveIcon();
            selectNo--;
            if (selectNo < 0)
            {
                selectNo = selectModeImage.Length - 1;
            }
        }
        else if (horizontalRaw < -0.75f)
        {
            selectCoolDown = true;
            NotActiveIcon();
            selectNo++;
            if (selectNo > selectModeImage.Length - 1)
            {
                selectNo = 0;
            }
        }
        else
        {
            return;
        }

        ActiveIcon();
    }


    private void ActiveIcon()
    {
        selectModeTransform[selectNo].sizeDelta = selectModeSize[selectNo] * notActiveSizeMag;
        selectModeImage[selectNo].color = new Color(0, 0, 0, 1f);
    }

    private void NotActiveIcon()
    {
        selectModeTransform[selectNo].sizeDelta = selectModeSize[selectNo];
        selectModeImage[selectNo].color = new Color(0, 0, 0, 0.5f);
    }

    private void Update()
    {
        if (selectCoolDown)
        {
            coolDownTimMeasure += Time.unscaledDeltaTime;
            if (coolDownTimMeasure > interval)
            {
                selectCoolDown = false;
                coolDownTimMeasure = 0;
            }
        }
    }
}
