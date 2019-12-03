using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeedMater : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject speedTextObject; //Machine時表示するspeedText
    [SerializeField] private GameObject playerImage; //Human時表示するPlayer画像
    [SerializeField] private TextMeshProUGUI speedText; //Speed
    [SerializeField] private Image[] chargeGages; //ChargeGage
    private bool humanCharge = false;

    // Update is called once per frame
    void Update()
    {
        Player.Condition condition = player.PlayerCondition;
        switch (condition)
        {
            case Player.Condition.Human:
                //スピードメーターにUnityちゃんの画像を表示
                if (!playerImage.activeSelf)
                {
                    playerImage.SetActive(true);
                }
                //スピードメーターを非表示に
                if (speedTextObject.activeSelf)
                {
                    speedTextObject.SetActive(false);
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
                break;
            case Player.Condition.Machine:
                if (playerImage.activeSelf)
                {
                    playerImage.SetActive(false);
                }
                if (!speedTextObject.activeSelf)
                {
                    speedTextObject.SetActive(true);
                }
                
                //speed表示
                speedText.text = player.Machine.SpeedMaterText();

                float charge = player.Machine.NormalizeCharge();

                Color color;
                int r = 255, g = 255, b = 255;

                if(charge > 0)
                {
                    if (charge < 0.25)
                    {
                        Debug.Log("r");
                        //青
                        r = 40; //濃さの固定
                        g = Random.Range(0, 255);
                    }
                    else if (charge < 0.5)
                    {
                        Debug.Log("g");
                        //緑
                        r = Random.Range(0, 200);
                        b = 0;
                    }
                    else if (charge < 0.75)
                    {
                        Debug.Log("orenge");
                        //オレンジ
                        g = Random.Range(100, 255);
                        b = g - 100;
                    }
                    else
                    {
                        Debug.Log("red");
                        //赤
                        int colorNum = Random.Range(0, 200);
                        g = colorNum;
                        b = colorNum;
                    }
                }
                
                color = new Color(r, g, b);

                //chargeGageの表示
                foreach (var chargeGage in chargeGages)
                {
                    chargeGage.fillAmount = charge;
                    chargeGage.color = color;
                }

                humanCharge = false;
                break;
            default:
                break;
        }
    }
}
