using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public enum Mode : int
    {
        Continue = 0,
        End = 1
    }

    [SerializeField] private Player player;
    [SerializeField] private RectTransform speedMater;
    [SerializeField] private RectTransform getItemStatus;
    [SerializeField] private Sprite[] selectY;
    [SerializeField] private Sprite[] selectN;
    [SerializeField] private Image[] selectMode;
    [SerializeField] private PauseGetItem getItemDisplay;

    private int selectNo = 0;
    private float interval = 0.25f;
    private bool selectCoolDown = false;

    public void ResetPause()
    {
        selectNo = 0;
        for(int i = 0; i < selectMode.Length; i++)
        {
            selectMode[i].sprite = selectN[i];
        }
        selectMode[selectNo].sprite = selectY[selectNo];
        speedMater.gameObject.SetActive(false);
        selectCoolDown = false;
        getItemDisplay.SetDisplay();
    }

    public void EndPause()
    {
        speedMater.gameObject.SetActive(true);
    }

    public Mode SelectMode()
    {
        return (Mode)selectNo;
    }

    // Update is called once per frame
    public void PauseDisplay()
    {
        float verticalRaw = InputManager.Instance.VerticalRaw;
        Debug.Log(verticalRaw);

        if (selectCoolDown)
        {
            return;
        }

        if(verticalRaw > 0.75f)
        {
            SetSprite();
            selectNo--;
            if (selectNo < 0)
            {
                selectNo = selectMode.Length - 1;
            }
        }
        else if(verticalRaw < -0.75f)
        {
            SetSprite();
            selectNo++;
            if (selectNo > selectMode.Length - 1)
            {
                selectNo = 0;
            }
        }
        else
        {
            return;
        }
        selectMode[selectNo].sprite = selectY[selectNo];
    }

    private void SetSprite()
    {
        StartCoroutine(Interval());
        selectMode[selectNo].sprite = selectN[selectNo];
    }

    private IEnumerator Interval()
    {
        selectCoolDown = true;
        yield return new WaitForSecondsRealtime(interval);
        selectCoolDown = false;
    }
}