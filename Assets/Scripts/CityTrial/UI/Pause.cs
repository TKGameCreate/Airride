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
    [SerializeField] private Sprite[] selectY;
    [SerializeField] private Sprite[] selectN;
    [SerializeField] private Image[] selectMode;
    [SerializeField] private GetItemListDisplay getItemDisplay;
    [SerializeField] private AudioClip pauseSE;

    private int selectNo = 0;
    private float interval = 0.25f;
    private bool selectCoolDown = false;
    private float coolDownTimeMeasure = 0;

    public void ResetPause()
    {
        AudioManager.Instance.PlaySE(pauseSE);
        selectNo = 0;
        for(int i = 0; i < selectMode.Length; i++)
        {
            selectMode[i].sprite = selectN[i];
        }
        selectMode[selectNo].sprite = selectY[selectNo];
        speedMater.gameObject.SetActive(false);
        selectCoolDown = false;
        if(player.Machine != null)
        {
            //playerの状態がMachineの場合
            getItemDisplay.gameObject.SetActive(true);
            getItemDisplay.SetDisplay();
        }
        else
        {
            //playerの状態がHumanの場合
            getItemDisplay.gameObject.SetActive(false);
        }
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
        AudioManager.Instance.PlaySystemSE(0);
        selectMode[selectNo].sprite = selectY[selectNo];
    }

    private void SetSprite()
    {
        selectCoolDown = true;
        selectMode[selectNo].sprite = selectN[selectNo];
    }

    private void Update()
    {
        if (selectCoolDown)
        {
            coolDownTimeMeasure += Time.unscaledDeltaTime;
            if(coolDownTimeMeasure > interval)
            {
                selectCoolDown = false;
                coolDownTimeMeasure = 0;
            }
        }
    }
}