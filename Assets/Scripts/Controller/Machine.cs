using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Machine : Control
{
    #region const
    const float chargeMass = 5.0f; //チャージ中の重力
    const float upStatusMag = 0.48f; //ステータスバフ倍率
    const float maxStatus = 18; //ステータス上昇上限
    const float exitMachineVertical = -0.8f; //降車時スティック最低入力量
    #endregion

    #region Serialize
    [SerializeField] private bool debug = false;
    [SerializeField] private MachineStatus status;
    [SerializeField] private GameObject textObject; //Machineに関するテキストを表示するテキスト群
    [SerializeField] private TextMeshProUGUI upStatusText; //デバッグ用UpStatus表示Text
    [SerializeField] private TextMeshProUGUI statusText; //速度とチャージ量を表示するText
    #endregion

    #region 変数
    //Statusのバフ状態
    private float[] upStatus = new float[8] 
    {
        0, //Attack [0]
        0, //Defence [1]
        0, //MaxSpeed [2]
        0, //Acceleration [3]
        0, //Turning [4]
        0, //Brake [5]
        0, //Charge [6]
        0 //Weight [7]
    };
    private Player player;
    private float speed = 0; //現在の速度
    private float chargeAmount = 1; //チャージ量
    private Vector3 force;
    #endregion

    #region プロパティ
    public Player Player
    {
        set
        {
            player = value;
        }
    }
    public MachineStatus Status { get { return status; } }
    #endregion

    private void Start()
    {
        rbody.mass = status.Weight;
    }

    #region public
    public override void Controller()
    {
        Move();
    }

    public override void FixedController()
    {
        //rbody.AddRelativeForce(velocity);
    }

    /// <summary>
    /// 画面に表示するテキスト処理
    /// </summary>
    public void TextDisplay()
    {
        if (debug)
        {
            upStatusText.text = "Attack : " + upStatus[0]
                + " \nDefence : " + upStatus[1]
                + "\nMaxSpeed : " + upStatus[2]
                + "\nAcceleration : " + upStatus[3]
                + "\nTurning : " + upStatus[4]
                + "\nBrake : " + upStatus[5]
                + "\nMaxCharge : " + upStatus[6]
                + "\nWeight : " + upStatus[7];
        }

        float charge = chargeAmount - 1;
        statusText.text =
            "Speed : " + speed.ToString("000.00") +
            "\nCharge : " + charge.ToString("F1");
    }

    /// <summary>
    /// アイテムを取得した際のステータス変動
    /// </summary>
    /// <param name="name">変動させるステータス</param>
    /// <param name="changeNum">変更値</param>
    public void ChangeStatus(StatusName name, float changeNum)
    {
        upStatus[(int)name] += changeNum;
        upStatus[(int)name] = UpStatusCheck(upStatus[(int)name]);
    }

    /// <summary>
    /// 壁や、アイテムボックスにぶつかった時に跳ね返る処理
    /// </summary>
    public void BackForce()
    {

    }
    #endregion

    #region protected
    /// <summary>
    /// ブレーキとチャージ
    /// </summary>
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

    protected virtual void BrakeAndCharge()
    {
        float brakeMag = MagCheck(StatusName.Brake);
        float chargeMag = MagCheck(StatusName.MaxCharge);

        //ブレーキ
        if (speed > 0)
        {
            speed -= status.Brake * brakeMag  * Time.deltaTime;
        }
        else
        {
            speed = 0;
        }

        //チャージ
        if (status.MaxCharge * chargeMag > chargeAmount)
        {
            chargeAmount += status.ChargeSpeed * chargeMag * Time.deltaTime;
        }

        //チャージ中は重くなる
        rbody.mass = chargeMass;
    }

    /// <summary>
    /// チャージダッシュ
    /// </summary>
    protected virtual void ChargeDash()
    {
        float mag = MagCheck(StatusName.Acceleration);
        speed = status.Acceleration * mag * chargeAmount;
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
        float speedMag = MagCheck(StatusName.MaxSpeed);
        float accMag = MagCheck(StatusName.Acceleration);
        float maxSpeed = status.MaxSpeed * speedMag;

        if (maxSpeed > speed)
        {
            //自動加速
            speed += status.Acceleration * accMag * Time.deltaTime;
        }
        else if (maxSpeed + 1 > speed)
        {
            //速度を一定に保つ
            speed = status.MaxSpeed;
        }
        else
        {
            //徐々に速度を落とす(ブレーキの2倍の速度)
            speed -= status.Acceleration * accMag * 2.0f * Time.deltaTime;
        }
    }
    #endregion

    #region private
    /// <summary>
    /// UpStatusの下限上限チェック
    /// </summary>
    /// <param name="up">チェックする値</param>
    /// <returns>チェック後の値</returns>
    private float UpStatusCheck(float up)
    {
        if(up >= maxStatus)
        {
            return maxStatus;
        }

        if(up < 1)
        {
            return 0;
        }

        return up;
    }

    /// <summary>
    /// 倍率チェック
    /// </summary>
    /// <param name="name">チェックするUpStatus</param>
    /// <returns>倍率</returns>
    private float MagCheck(StatusName name)
    {
        if (upStatus[(int)name] > 1)
        {
            return upStatus[(int)name] * upStatusMag;
        }
        else
        {
            return 1;
        }
    }
    #endregion
}