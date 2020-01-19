using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarboStar : Machine
{
    [SerializeField] private float brakeCap = 0;
    [SerializeField] private float posTolerance = 1.0f;
    [SerializeField] private float speedTolerance = 5.0f;
    [SerializeField] private float maxSpeedCapMag = 0.8f;
    [SerializeField] private float turningMag = 3.0f;

    protected override void Charge()
    {
        float chargePosZ = chargePos.z;
        float nowPosZ = transform.forward.z;
        float chargeMag = 1;
        //ほぼ正面を向いている場合
        if(chargePosZ < nowPosZ + posTolerance && chargePosZ > nowPosZ - posTolerance
            && speed < speedTolerance)
        {
            Debug.Log("IN");
            //ほぼチャージされない
            chargeMag = 0.1f;
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
        if (speed - Status(StatusType.Brake) * Time.deltaTime > brakeCap)
        {
            speed -= Status(StatusType.Brake) * Time.deltaTime;
        }
        else
        {
            //ブレーキ限界値を超えた場合、ブレーキの効果が2/1になる
            speed = Mathf.Clamp(speed - Status(StatusType.Brake) / 2 * Time.deltaTime, 0, brakeCap);
        }
    }

    protected override void Accelerator()
    {
        //最高速度
        float max = Status(StatusType.MaxSpeed);
        float maxSpeed = max;
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
