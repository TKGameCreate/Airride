using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuinsStar : Machine
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
        //Aボタンを押していない
        else
        {
            Accelerator();
        }

        //空中では方向転換ができる
        if (!onGround)
        {
            transform.Rotate(0, horizontal * Status(StatusType.Turning) * Time.deltaTime, 0);
        }
    }

    protected override void Brake()
    {
        if (!nowBrake)
        {
            chargePos = transform.forward;
            nowBrake = true;
        }

        //ブレーキ
        speed = 0;
        //ブレーキ中は方向転換ができる
        transform.Rotate(0, horizontal * Status(StatusType.Turning) * Time.deltaTime, 0);
    }
}
