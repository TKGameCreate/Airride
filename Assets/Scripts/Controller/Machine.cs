using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : Control
{
    [SerializeField] private MachineStatus status;
    [SerializeField] private float exitMachineVertical = -0.8f;
    private Player player;
    private float speed = 0; //現在の速度
    private float chargeAmount = 1; //チャージ量

    #region プロパティ
    public Player Player
    {
        set
        {
            player = value;
        }
    }
    public MachineStatus Status { get { return status; } }
    public float Speed { get { return speed; } }
    public float Charge { get { return chargeAmount; } }
    #endregion

    private void Start()
    {
        rbody.mass = status.Weight;
    }

    public override void Controller()
    {
        Move();
    }

    protected override void Move()
    {
        base.Move();

        //Machineから降りる
        if (InputManager.Instance.InputA(InputType.Down) && vertical < exitMachineVertical)
        {
            //speedを初期化
            speed = 0;
            //親子関係の解除
            transform.parent = null;
            //PlayerのConditionをMachineからHumanに
            player.PlayerCondition = Player.Condition.Human;
            //マシンの割り当てを削除
            player.Machine = null;
            Player = null;
        }

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
        velocity = new Vector3(0, 0, speed);
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

        //チャージ中は重くなる
        rbody.mass = 1.0f;
    }

    /// <summary>
    /// チャージダッシュ
    /// </summary>
    protected virtual void ChargeDash()
    {
        speed = status.Acceleration * chargeAmount;
        //チャージ量をリセット
        chargeAmount = 1;
        //重量のリセット
        rbody.mass = status.Weight;
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