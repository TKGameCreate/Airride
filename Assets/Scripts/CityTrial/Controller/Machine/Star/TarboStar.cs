using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarboStar : Machine
{
    private bool front = true; //チャージ時に正面を向いているか

    [SerializeField] private float brakeCapMag = 0.2f; //Brakeが効くキャップ値
    [SerializeField] private float posTolerance = 1.0f; //正面向いているかどうかの判定をする判定余裕値
    [SerializeField] private float speedTolerance = 5.0f; //
    [SerializeField] private float maxSpeedCapMag = 0.8f;
    [SerializeField] private float chargeTurningMag = 3.0f;
    [SerializeField] private float notFrontBrakeMag = 0.3f;
    [SerializeField] private float frontChargeMag = 0.2f;
    private float turningMag = 1.0f;

    private void CheckFront()
    {
        float chargePosZ = chargePos.z;
        float nowPosZ = transform.forward.z;

        //ほぼ正面を向いている場合
        if (nowPosZ < chargePosZ + posTolerance &&
            nowPosZ > chargePosZ - posTolerance)
        {
            front = true;
        }
        else
        {
            front = false;
        }
    }

    protected override void Move()
    {
        Input();

        //Aボタンを押している
        if (InputManager.Instance.InputA(InputType.Hold))
        {
            Charge();
            CheckFront();
            Brake();
        }
        //Aボタンを離した
        else if (InputManager.Instance.InputA(InputType.Up))
        {
            ChargeDash();
        }
        //Aボタンを押していない
        else
        {
            Accelerator();
        }

        transform.Rotate(0, horizontal * Status(StatusType.Turning) * turningMag * Time.deltaTime, 0);
    }

    protected override void Charge()
    {
        //charge倍率
        float chargeMag = 1;
        //旋回倍率を変更
        turningMag = chargeTurningMag;

        //正面を向いている場合 or 指定速度以下の場合
        if (front || speed < speedTolerance)
        {
            //チャージ倍率が低くなる
            chargeMag = frontChargeMag;
        }

        //チャージ
        if (Status(StatusType.Charge) > chargeAmount)
        {
            chargeAmount += Status(StatusType.ChargeSpeed) * chargeMag * Time.deltaTime;
        }

        float normalize = NormalizeCharge();
        chargeAudioSource.volume = normalize;
        chargeAudioSource.pitch = Mathf.Clamp(1 + normalize * 2, 1, maxPitch + 1);
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
        //正面以外を向いている場合もしくは、現在の速度がブレーキキャップ値を下回った場合
        if (!front || nowBrakeSpeed < brakeCapSpeed)
        {
            //倍率の変更
            brakeMag = notFrontBrakeMag;
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

    protected override void ChargeDash()
    {
        base.ChargeDash();
        turningMag = 1.0f;
    }

    protected override void Accelerator()
    {
        //最高速度
        float maxSpeed = Status(StatusType.MaxSpeed);
        //地面に接地していないときの処理
        if (!onGround)
        {                               
            maxSpeed = Status(StatusType.FlySpeed);
            //徐々にチャージがたまる
            chargeAmount += Status(StatusType.ChargeSpeed) / flyChargeSpeed;
        }

        //Maxスピードオーバーの許容範囲
        float tolerance = 1.0f;
        float speedMag = 1.0f;

        if (maxSpeed > speed)
        {
            //キャップ値を超えた場合
            if (maxSpeed * maxSpeedCapMag < speed)
            {
                speedMag = 0.1f;
            }
            //自動加速
            speed += Status(StatusType.Acceleration) * speedMag * Time.deltaTime;
        }
        else if (maxSpeed + tolerance > speed)
        {
            //速度を一定に保つ
            speed = maxSpeed;
        }
        else
        {
            if (speed > 999)
            {
                speed = 999;
            }
            //徐々に速度を落とす
            speed -= Status(StatusType.Brake) * Time.deltaTime;
        }
    }
}
