using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlickStar : Machine
{
    protected override void Move()
    {
        Input();

        //Aボタンを押している
        if (InputManager.Instance.InputA(InputType.Hold))
        {
            Charge();
            Brake();
        }
        //Aボタンを離した
        else if (InputManager.Instance.InputA(InputType.Up))
        {
            ChargeDash();
        }
        Accelerator();

        transform.Rotate(0, horizontal * Status(StatusType.Turning) * Time.deltaTime, 0);
    }

    protected override void Brake()
    {
        if (!nowBrake)
        {
            chargePos = transform.forward;
            nowBrake = true;
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

        if (maxSpeed > speed)
        {
            //自動加速
            speed += Status(StatusType.Acceleration) * Time.deltaTime;
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
