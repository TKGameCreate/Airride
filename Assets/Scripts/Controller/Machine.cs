using UnityEngine;
using TMPro;

public class Machine : Control
{
    #region const
    const float chargeDashPossible = 0.8f; //チャージダッシュ可能量
    const float upStatusMag = 0.23f; //ステータスバフ倍率
    const float maxStatus = 18; //ステータス上昇上限
    const float exitMachineVertical = -0.8f; //降車時スティック最低入力量
    const float chargeUnderPower = 10.0f; //charge中に下に加える力
    #endregion

    #region Serialize
    [SerializeField] private bool debug = false;
    [SerializeField] private MachineStatus status;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private GameObject debugTextObject; //Machineに関するテキストを表示するテキスト群
    [SerializeField] private TextMeshProUGUI debugText; //デバッグ用UpStatus表示Text
    #endregion

    #region 変数
    //アイテムの取得状態
    private float[] getItemNum = new float[8] 
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
    //ステータスのバフ状態
    private float[] upStatus = new float[8]
    {
        1, //Attack [0]
        1, //Defence [1]
        1, //MaxSpeed [2]
        1, //Acceleration [3]
        1, //Turning [4]
        1, //Brake [5]
        1, //Charge [6]
        1 //Weight [7]
    };
    private Player player;
    private float speed = 0; //現在の速度
    private float chargeAmount = 1; //チャージ量
    private bool nowCharge = false; //charge中かどうか
    private bool setSpeedMater = false;
    private Vector3 chargePos;
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

    #region public
    public override void Controller()
    {
        if (debug)
        {
            DebugTextDisplay();
        }
        SpeedAndChargeMater();
        Move();
    }

    public override void FixedController()
    {
        //移動量
        if (nowCharge)
        {
            rbody.velocity = chargePos * speed;
        }
        else
        {
            rbody.velocity = transform.forward * speed;
        }

        //チャージ中の下に力を入れる処理
        //if (nowCharge)
        //{
        //    rbody.AddForce(Vector3.down * chargeUnderPower);
        //}
    }

    /// <summary>
    /// アイテムを取得した際のステータス変動
    /// </summary>
    /// <param name="name">変動させるステータス</param>
    /// <param name="changeNum">変更値</param>
    public void ChangeStatus(StatusName name,  ItemMode mode)
    {
        //バフアイテム
        if(mode == ItemMode.Buff)
        {
            getItemNum[(int)name]++; //アイテムの取得数を増やす
            //上限チェック
            if (getItemNum[(int)name] > maxStatus)
            {
                getItemNum[(int)name] = maxStatus;
                return;
            }
            //ステータスの倍率を増加
            upStatus[(int)name] += upStatusMag;
        }
        //デバフアイテム
        else
        {
            getItemNum[(int)name]--; //アイテム取得数を減らす
            //下限チェック
            if(getItemNum[(int)name] < 0)
            {
                getItemNum[(int)name] = 0;
                return;
            }
            //ステータスの倍率を減少
            upStatus[(int)name] -= upStatusMag;
        }
    }

    /// <summary>
    /// 壁や、アイテムボックスにぶつかった時に跳ね返る処理
    /// </summary>
    public void BackForce()
    {
        //rbody.AddForce();
    }
    #endregion

    #region protected
    /// <summary>
    /// ブレーキとチャージ
    /// </summary>
    protected override void Move()
    {
        base.Move();

        //プレイヤー操作中か否かでRigidBodyの状態を切り替える
        if(player != null)
        {
            rbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            rbody.constraints = RigidbodyConstraints.FreezeAll;
        }

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
            //Rigidbodyをフリーズ
            rbody.constraints = RigidbodyConstraints.FreezeAll;
            //スピードメーターセットフラグを初期化
            setSpeedMater = false;
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

        transform.Rotate(0, horizontal * status.Turning * Time.deltaTime, 0);
        //transform.localPosition += velocity * Time.deltaTime;
    }

    protected virtual void BrakeAndCharge()
    {
        float brakeMag = StatusMag(StatusName.Brake);
        float chargeMag = StatusMag(StatusName.MaxCharge);

        if (!nowCharge)
        {
            chargePos = transform.forward;
            nowCharge = true;
        }

        //ブレーキ
        if (speed - status.Brake * brakeMag * Time.deltaTime > 0)
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
    }

    /// <summary>
    /// チャージダッシュ
    /// </summary>
    protected virtual void ChargeDash()
    {
        float chargeMag = StatusMag(StatusName.MaxCharge);

        //チャージダッシュはチャージが一定以上でなければ発動しない
        if (chargeAmount >= status.MaxCharge * chargeMag * chargeDashPossible)
        {
            float mag = StatusMag(StatusName.Acceleration);
            speed += status.Acceleration * mag * chargeAmount;
        }
        //チャージ量をリセット
        chargeAmount = 1;
        nowCharge = false;
    }

    /// <summary>
    /// 通常加速処理
    /// 最高速度以上の場合の減速処理も含む
    /// </summary>
    protected virtual void Accelerator()
    {
        //最高速度
        float speedMag = StatusMag(StatusName.MaxSpeed);
        float maxSpeed = status.MaxSpeed * speedMag;
        //加速
        float accMag = StatusMag(StatusName.Acceleration);
        //Maxスピードオーバーの許容範囲
        float tolerance = 1.0f;

        if (nowCharge)
        {
            nowCharge = false;
        }

        if (maxSpeed  > speed)
        {
            //自動加速
            speed += status.Acceleration * accMag * Time.deltaTime;
        }
        else if (maxSpeed + tolerance > speed)
        {
            //速度を一定に保つ
            speed = maxSpeed;
        }
        else
        {
            //徐々に速度を落とす
            speed -= status.Acceleration * accMag * Time.deltaTime;
        }
    }

    protected override void SpeedAndChargeMater()
    {
        if (!setSpeedMater)
        {
            speedMaterPlayerImage.SetActive(false);
            speedMaterTextObject.SetActive(true);
            setSpeedMater = true;
        }

        float moveSpeed = rbody.velocity.magnitude;
        float charge = (chargeAmount - 1) / (status.MaxCharge - 1); //0~1の範囲に正規化
        float intSpeed = moveSpeed - moveSpeed % 1; //整数部分のみ抽出
        float fewSpeed = moveSpeed % 1; //小数部分のみ抽出

        //chargeGageの表示
        foreach (var chargeGage in chargeGages)
        {
            chargeGage.fillAmount = charge;
        }
        //SpeedMaterの表示
        speedText.text = intSpeed.ToString("000")
            + "\n"
            + "<size=30>"
            + fewSpeed.ToString(".00")
            + "</size>";
    }
    #endregion

    #region private
    private float StatusMag(StatusName name)
    {
        return upStatus[(int)name];
    }

    /// <summary>
    /// デバッグテキスト処理
    /// </summary>
    private void DebugTextDisplay()
    {
        if(player == null)
        {
            debugTextObject.SetActive(false);
            return;
        }
        else
        {
            debugTextObject.SetActive(true);
        }

        debugText.text = "Attack : " + getItemNum[0]
            + " \nDefence : " + getItemNum[1]
            + "\nMaxSpeed : " + getItemNum[2]
            + "\nAcceleration : " + getItemNum[3]
            + "\nTurning : " + getItemNum[4]
            + "\nBrake : " + getItemNum[5]
            + "\nMaxCharge : " + getItemNum[6]
            + "\nWeight : " + getItemNum[7];
    }
    #endregion
}