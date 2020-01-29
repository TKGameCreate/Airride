using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelect : MonoBehaviour
{
    private enum Mode : int
    {
        CityTrial = 0,
        Option = 1
    }

    [SerializeField] private Sprite[] active = { };
    [SerializeField] private Sprite[] notActive = { };
    [SerializeField] private Image[] mode = { };

    private int selectNo = 0;
    private bool selectCoolDown = false;

    public void ResetSelect()
    {
        selectNo = 0;
        selectCoolDown = false;
        for (int i = 0; i < mode.Length; i++)
        {
            mode[i].sprite = notActive[i];
        }
        mode[selectNo].sprite = active[selectNo];
    }

    private void Decision()
    {
        switch ((Mode)selectNo)
        {
            case Mode.CityTrial:
                AirrideSceneManager.Instance.LoadScene(AirrideScene.CityTrial);
                break;
            case Mode.Option:
                break;
            default:
                break;
        }
    }

    public void Select()
    {
        if (InputManager.Instance.InputA(InputType.Down))
        {
            Decision();
        }

        float horizontalRaw = InputManager.Instance.HorizontalRaw;

        if (selectCoolDown)
        {
            return;
        }

        if (horizontalRaw > 0.75f)
        {
            StartCoroutine(CoolDown());
            SetSprite();
            selectNo--;
            if (selectNo < 0)
            {
                selectNo = mode.Length - 1;
            }
        }
        else if (horizontalRaw < -0.75f)
        {
            StartCoroutine(CoolDown());
            SetSprite();
            selectNo++;
            if (selectNo > mode.Length - 1)
            {
                selectNo = 0;
            }
        }
        else
        {
            return;
        }
        mode[selectNo].sprite = active[selectNo];
    }

    private void SetSprite()
    {
        selectCoolDown = true;
        mode[selectNo].sprite = notActive[selectNo];
    }

    private IEnumerator CoolDown()
    {
        float interval = 0.15f;
        yield return new WaitForSeconds(interval);
        selectCoolDown = false;
    }
}