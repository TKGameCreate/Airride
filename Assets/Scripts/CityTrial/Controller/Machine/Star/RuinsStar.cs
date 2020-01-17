using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuinsStar : Machine
{
    protected override void Start()
    {
        base.Start();
        chargeAmount = 0;
    }

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

    protected override void ChargeDash()
    {
        //チャージダッシュはチャージが一定以上でなければ発動しない
        if (chargeAmount >= chargeDashPossible)
        {
            speed += Status(StatusType.Acceleration) * chargeAmount;
        }
        chargeAudioSource.volume = 0;
        StartCoroutine(PitchResetOneFlameLater());
        //チャージ量をリセット
        chargeAmount = 0;
        nowBrake = false;
    }

    protected override void GetOff()
    {
        //スティック下+Aボタンかつ接地しているとき
        if (InputManager.Instance.InputA(InputType.Hold)
            && vertical < exitMachineVertical
            && onGround)
        {
            //アイテムを落とす
            DropItem();
            getOffPossible = false;
            vcamera.Priority = 1;//マシンカメラの優先度を最低に
            chargeAmount = 0;
            //入力値のリセット
            vertical = 0;
            horizontal = 0;
            //speedを初期化
            speed = 0;
            //親子関係の解除
            transform.parent = null;
            //PlayerのConditionをMachineからHumanに
            Player.PlayerCondition = Player.Condition.Human;
            //マシンの割り当てを削除
            Player.Machine = null;
            Player = null;
            engineAudioSource.Stop();
            chargeAudioSource.volume = 0;
            chargeAudioSource.Stop();
            return;
        }
    }

    public override float NormalizeCharge()
    {
        //0~1の範囲に正規化
        float charge = chargeAmount / Status(StatusType.Charge);
        return charge;
    }

    public override void OnGround(Collider other)
    {
        if (other.transform.tag == "StageObject" || other.transform.tag == "NotBackSObject")
        {
            //空中から接地した瞬間
            if (!onGround)
            {
                //チャージ中じゃない場合
                if (!InputManager.Instance.InputA(InputType.Hold))
                {
                    //自動的に溜まったチャージをリセット
                    chargeAmount = 0;
                }
                //接地SEを再生
                AudioManager.Instance.PlaySE(groundSE);
            }
            onGround = true;
        }
    }
}
