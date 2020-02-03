using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeSetting : MonoBehaviour
{
    private const int maxNo = 3;
    public static float LastSetting { private set; get; }
    private static int selectNo = 0; //時間の設定は保存しておく

    [SerializeField] private TextMeshProUGUI timeText = null;

    private bool selectCoolDown = false;

    public void LastTimeSetting()
    {
        LastSetting = StateManager.Time;
    }

    // Update is called once per frame
    public void Setting()
    {
        if (selectCoolDown)
        {
            return;
        }
        Select();
        SetTime();
    }

    private void SetTime()
    {
        int minute = 0;
        switch (selectNo)
        {
            case 0:
                minute = 3; //3分
                break;
            case 1:
                minute = 5; //5分
                break;
            case 2:
                minute = 7; //7分
                break;
            default:
                break;
        }
        timeText.text = minute.ToString() + "分";
        StateManager.Time = 60 * minute;
    }

    private void Select()
    {
        float horizontalRaw = -InputManager.Instance.HorizontalRaw;

        if (horizontalRaw > 0.75f)
        {
            selectCoolDown = true;
            AudioManager.Instance.PlaySystemSE(0);
            StartCoroutine(CoolDown());
            selectNo--;
            if (selectNo < 0)
            {
                selectNo = maxNo - 1;
            }
        }
        else if (horizontalRaw < -0.75f)
        {
            selectCoolDown = true;
            AudioManager.Instance.PlaySystemSE(0);
            StartCoroutine(CoolDown());
            selectNo++;
            if (selectNo > maxNo - 1)
            {
                selectNo = 0;
            }
        }
        else
        {
            return;
        }
    }

    private IEnumerator CoolDown()
    {
        float interval = 0.15f;
        yield return new WaitForSeconds(interval);
        selectCoolDown = false;
    }
}