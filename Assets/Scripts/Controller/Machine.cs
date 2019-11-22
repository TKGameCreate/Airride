using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : Control
{
    [SerializeField] private Rigidbody rbody;
    [SerializeField] private MachineStatus status;

    private float speed = 0; //速度
    private float chargeAmount = 1; //チャージ量

    private Vector3 velocity;

    private void Start()
    {
        rbody.mass = status.Weight;
    }

    public void Controller()
    {
        horizontal = InputManager.Instance.Horizontal;

        //Aボタンを押している
        if (InputManager.Instance.InputA(InputType.Hold))
        {
            BrakeAndCharge();
        }
        else if (InputManager.Instance.InputA(InputType.Up))
        {
            ChargeDash();
        }
        //Aボタンを押していない
        else
        {
            Accelerator();
        }

        //移動量
        velocity = new Vector3(speed, 0, 0);
        velocity = transform.TransformDirection(velocity);
        transform.localPosition += velocity * Time.deltaTime;
        //旋回
        transform.Rotate(0, horizontal * status.Turning * Time.deltaTime, 0);
    }

    /// <summary>
    /// ブレーキとチャージ
    /// </summary>
    protected virtual void BrakeAndCharge()
    {
        //ブレーキ
        if (speed > 0)
        {
            speed -= status.Brake * Time.deltaTime;
        }
        else
        {
            speed = 0;
        }
        //チャージ
        if (status.MaxCharge > chargeAmount)
        {
            chargeAmount += status.ChargeSpeed * Time.deltaTime;
        }
    }

    /// <summary>
    /// チャージダッシュ
    /// </summary>
    protected virtual void ChargeDash()
    {
        speed = status.Acceleration * chargeAmount;
    }

    /// <summary>
    /// 通常加速処理
    /// 最高速度以上の場合の減速処理も含む
    /// </summary>
    protected virtual void Accelerator()
    {
        if (status.MaxSpeed > speed)
        {
            //自動加速
            speed += status.Acceleration * Time.deltaTime;
        }
        else if (status.MaxSpeed + 1 > speed)
        {
            //速度を一定に保つ
            speed = status.MaxSpeed;
        }
        else
        {
            //徐々に速度を落とす
            speed -= status.Acceleration * Time.deltaTime;
        }
    }
}