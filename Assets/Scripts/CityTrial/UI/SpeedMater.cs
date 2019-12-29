using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeedMater : MonoBehaviour
{
    [SerializeField] private Image playerImage; //Human時表示するPlayer画像
    [SerializeField] private TextMeshProUGUI speedText; //Speed
    [SerializeField] private Image[] chargeGages; //ChargeGage

    //各ゲージのカラー
    [SerializeField] private Color blue;
    [SerializeField] private Color green;
    [SerializeField] private Color orenge;
    [SerializeField] private Color red;
    //各ゲージのカラー（White版）
    [SerializeField] private Color whiteBlue;
    [SerializeField] private Color whiteGreen;
    [SerializeField] private Color whiteOrenge;
    [SerializeField] private Color whiteRed;

    //点滅速度
    [SerializeField] private float flashingTime;


    private Color color = new Color();
    private bool humanCharge = false;
    private float nextTime;

    public void HumanGage()
    {
        //スピードメーターにUnityちゃんの画像を表示
        if (!playerImage.gameObject.activeSelf)
        {
            playerImage.gameObject.SetActive(true);
        }
        //スピードメーターを非表示に
        if (speedText.gameObject.activeSelf)
        {
            speedText.gameObject.SetActive(false);
        }

        //chargeGageのリセット
        if (!humanCharge)
        {
            foreach (var chargeGage in chargeGages)
            {
                chargeGage.fillAmount = 0;
            }
            humanCharge = true;
        }
    }

    public void MachineGage(Machine machine)
    {
        if (playerImage.gameObject.activeSelf)
        {
            playerImage.gameObject.SetActive(false);
        }
        if (!speedText.gameObject.activeSelf)
        {
            speedText.gameObject.SetActive(true);
        }

        //speed表示
        speedText.text = machine.SpeedMaterText();

        float charge = machine.NormalizeCharge();

        #region GageColor処理
        if (charge > 0 && charge < 0.3f)
        {
            //青
            color = FlashingColor(blue, whiteBlue);
        }
        else if (charge < 0.5f)
        {
            //緑
            color = FlashingColor(green, whiteGreen);
        }
        else if (charge < 0.75f)
        {
            //オレンジ
            color = FlashingColor(orenge, whiteOrenge);
        }
        else
        {
            //赤
            color = FlashingColor(red, whiteRed);
        }
        #endregion

        //chargeGageの表示
        foreach (var chargeGage in chargeGages)
        {
            chargeGage.fillAmount = charge;
            chargeGage.color = color;
        }

        humanCharge = false;
    }

    private Color FlashingColor(Color fromColor, Color toColor)
    {
        if (nextTime > flashingTime * 2)
        {
            nextTime = 0;
        }

        nextTime += Time.deltaTime;

        float r;
        float g;
        float b;

        if (nextTime <= flashingTime)
        {
            r = fromColor.r -= toColor.r * flashingTime;
            g = fromColor.g -= toColor.g * flashingTime;
            b = fromColor.b -= toColor.b * flashingTime;
        }
        else
        {
            r = toColor.r -= fromColor.r * flashingTime;
            g = toColor.g -= fromColor.g * flashingTime;
            b = toColor.b -= fromColor.b * flashingTime;
        }
        return new Color(r, g, b);
    }
}