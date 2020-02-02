using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeSetting : MonoBehaviour
{
    private const int maxNo = 3;
    private bool selectCoolDown = false;

    [SerializeField] private RectTransform modeSelect = null;
    [SerializeField] private TextMeshProUGUI timeText = null;
    private int selectNo = 0;
    public static float LastSetting { private set; get; } = StateManager.Time;

    // Update is called once per frame
    void Update()
    {
        if (InputManager.Instance.InputB)
        {
            AudioManager.Instance.PlaySystemSE(0);
            gameObject.SetActive(false);
            modeSelect.gameObject.SetActive(true);
            LastSetting = StateManager.Time;
        }
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