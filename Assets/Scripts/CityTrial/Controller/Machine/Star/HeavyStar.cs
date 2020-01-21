using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyStar : Machine
{
    [SerializeField] private float brakeCap = 0.1f;
    [SerializeField] private float chargeDashMag = 0.35f;
    [SerializeField] private float brakeCapMag = 0.1f;
    [SerializeField] private float decelerationMag = 0.35f;

    protected override void Move()
    {
        Input();

        //Aボタンを押している
        if (InputManager.Instance.InputA(InputType.Hold))
        {
            Charge();
            Brake();
        }
        //Aボタンを押していない
        else
        {
            ChargeDash();
        }

        transform.Rotate(0, horizontal * Status(StatusType.Turning) * Time.deltaTime, 0);
    }

    protected override void Brake()
    {
        if (!nowBrake)
        {
            chargePos = transform.forward;
            nowBrake = true;
        }

        //ブレーキ
        float brakeMag = 1.0f;
        float nowBrakeSpeed = speed - Status(StatusType.Brake) * Time.deltaTime;
        float brakeCapSpeed = Status(StatusType.MaxSpeed) * brakeCapMag;
        //現在の速度がブレーキキャップ値を下回った場合
        if (nowBrakeSpeed < brakeCapSpeed)
        {
            //倍率の変更
            brakeMag = brakeCapMag;
        }

        if (nowBrakeSpeed > 0)
        {
            speed -= Status(StatusType.Brake) * brakeMag * Time.deltaTime;
        }
        else
        {
            speed = 0;
        }
    }

    /// <summary>
    /// チャージダッシュ
    /// </summary>
    protected override void ChargeDash()
    {
        if(chargeAmount > defaultChargeAmount)
        {
            Accelerator();
            chargeAmount -= Status(StatusType.ChargeSpeed) * chargeDashMag * Time.deltaTime;
        }
        else
        {
            float nowBrakeSpeed = speed - Status(StatusType.Brake) * decelerationMag * Time.deltaTime;
            if (nowBrakeSpeed > 0)
            {
                speed -= Status(StatusType.Brake) * decelerationMag * Time.deltaTime;
            }
            else
            {
                speed = 0;
            }
            chargeAmount = 0;
        }

        chargeAudioSource.volume = 0;
        nowBrake = false;
    }
}
