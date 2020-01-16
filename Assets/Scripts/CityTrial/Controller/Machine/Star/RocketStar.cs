using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketStar : Machine
{
    [SerializeField] private float brakeMag = 2.0f;

    /// <summary>
    /// ブレーキ処理
    /// </summary>
    protected override void Brake()
    {
        if (!nowBrake)
        {
            chargePos = transform.forward;
            nowBrake = true;
        }

        //ブレーキ
        if (speed - Status(StatusType.Brake) * brakeMag * Time.deltaTime > 0)
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
        //チャージダッシュはチャージがMaxでないと発動しない
        if (chargeAmount >= Status(StatusType.Charge))
        {
            speed += Status(StatusType.Acceleration) * chargeAmount;
        }
        chargeAudioSource.volume = 0;
        StartCoroutine(PitchResetOneFlameLater());
        //チャージ量をリセット
        chargeAmount = 1;
        nowBrake = false;
    }
}
