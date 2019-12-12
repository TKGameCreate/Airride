using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Machine : Control
{
    #region const
    const float chargeDashPossible = 0.75f; //チャージダッシュ可能量
    const float exitMachineVertical = -0.8f; //降車時スティック最低入力量
    const float chargeUnderPower = 35000.0f; //charge中に下に加える力
    const float flyWeightMag = 100f; //滑空時の落下倍率
    const float flyChargeSpeed = 1500f; //滑空中の自動チャージ速度分率
    const float dashBoardMag = 2.5f; //ダッシュボード倍率
    const float boundPower = 300f; //跳ね返る力
    public const float limitStatus = 16; //ステータス下限上限
    #endregion

    #region Serialize
    [SerializeField] private bool debug = false;
    [SerializeField] private MachineStatus status;
    [SerializeField] private GameObject debugTextObject; //Machineに関するテキストを表示するテキスト群
    [SerializeField] private TextMeshProUGUI debugTextGetItem; //(デバッグ用)UpStatus表示Text
    [SerializeField] private TextMeshProUGUI debugTextStatus; //(デバッグ用)ステータス倍率表示Text
    #endregion

    #region 変数
    private List<float> getItemList = new List<float>();    //アイテムの取得状態
    private List<float> statusList = new List<float>();    //ステータスのバフ状態
    private Player player;
    private float speed = 0; //現在の速度
    private float chargeAmount = 1; //チャージ量
    private bool nowCharge = false; //charge中かどうか
    private bool onGround = true; //接地フラグ
    private bool bound = false; //跳ね返り処理を行うフラグ
    private Vector3 chargePos;
    private float saveSpeed = 0; //衝突時のスピードを保存する
    #endregion

    #region プロパティ
    public Player Player
    {
        set
        {
            player = value;
        }
    }
    public MachineStatus MachineStatus
    {
        get
        {
            return status;
        }
    }
    public float SaveSpeed
    {
        get
        {
            return saveSpeed;
        }
    }
    #endregion

    #region public
    public override void Controller()
    {
        Move();

        if (debug)
        {
            DebugTextDisplay();
        }
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

        //空中時の処理
        if (!onGround)
        {
            rbody.AddForce(Vector3.down * Status(StatusType.Weight) * flyWeightMag);
            //チャージ中の下に力を入れる処理
            if (nowCharge)
            {
                rbody.AddForce(Vector3.down * chargeUnderPower);
            }
        }
    }

    /// <summary>
    /// 取得したアイテムのカウント
    /// </summary>
    /// <param name="item">変動させるステータス</param>
    /// <param name="mode">変更値</param>
    /// <returns>上限下限か</returns>
    public bool ItemCount(ItemName item, ItemMode mode)
    {
        switch (mode)
        {
            //バフアイテム
            case ItemMode.Buff:
                if (mode == ItemMode.Buff)
                {
                    //上限チェック
                    if (getItemList[(int)item] < limitStatus)
                    {
                        getItemList[(int)item]++; //アイテムの取得数を増やす
                        return false;
                    }
                }
                return true;
            //デバフアイテム
            case ItemMode.Debuff:
                //下限チェック
                if (getItemList[(int)item] > -limitStatus)
                {
                    getItemList[(int)item]--; //アイテム取得数を減らす
                    return false;
                }
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// ステータスの変動
    /// </summary>
    /// <param name="name">変動させるステータス</param>
    /// <param name="up">上昇か下降か</param>
    public void ChangeStatus(StatusType name, ItemMode mode, float mag = 0)
    {
        bool plus;
        //Statusが基準ステータスより高かったら
        if(statusList[(int)name] <= status.GetStatus(name, MachineStatus.Type.Default))
        {
            //計算基準をPlus値で行う
            plus = true;
        }
        else
        {
            plus = false;
        }

        //Debug.Log(plus);

        if (plus)
        {
            //ステータスを上昇
            if (mode == ItemMode.Buff)
            {
                statusList[(int)name] += status.PlusStatus(name, mag);
            }
            //ステータスを下降
            else
            {
                statusList[(int)name] -= status.PlusStatus(name, mag);
            }
        }
        else
        {
            if (mode == ItemMode.Buff)
            {
                statusList[(int)name] += status.MinusStatus(name, mag);
            }
            else
            {
                statusList[(int)name] -= status.MinusStatus(name, mag);
            }
        }
    }

    /// <summary>
    /// SpeedMaterに表示するSpeed
    /// </summary>
    /// <returns>Textに表示するSpeed</returns>
    public string SpeedMaterText()
    {
        float moveSpeed = rbody.velocity.magnitude;
        float intSpeed = moveSpeed - moveSpeed % 1; //整数部分のみ抽出
        float fewSpeed = moveSpeed % 1; //小数部分のみ抽出

        return intSpeed.ToString("000")
            + "\n"
            + "<size=30>"
            + fewSpeed.ToString(".00")
            + "</size>";
    }

    /// <summary>
    /// 正規化したチャージ量
    /// </summary>
    /// <returns>チャージ量</returns>
    public float NormalizeCharge()
    {
        //0~1の範囲に正規化
        float charge = (chargeAmount - 1) / (Status(StatusType.Charge) - 1);
        return charge;
    }

    /// <summary>
    /// 壁や、アイテムボックスにぶつかった時に跳ね返る処理
    /// </summary>
    public void Bound()
    {
        saveSpeed = speed;
        speed /= 2;
        rbody.AddRelativeForce(
            (-Vector3.forward * saveSpeed * boundPower) 
            / Status(StatusType.Weight),
            ForceMode.Impulse);
    }

    public float Status(StatusType name)
    {
        return statusList[(int)name];
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
        //スティック下+Aボタンかつ接地しているとき
        if (InputManager.Instance.InputA(InputType.Down) && vertical < exitMachineVertical && onGround)
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
            return;
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

        transform.Rotate(0, horizontal * Status(StatusType.Turning) * Time.deltaTime, 0);
    }

    /// <summary>
    /// Aボタンを押した時の処理
    /// </summary>
    protected virtual void BrakeAndCharge()
    {
        if (!nowCharge)
        {
            chargePos = transform.forward;
            nowCharge = true;
        }

        //ブレーキ
        if (speed - Status(StatusType.Brake) * Time.deltaTime > 0)
        {
            speed -= Status(StatusType.Brake) * Time.deltaTime;
        }
        else
        {
            speed = 0;
        }

        //チャージ
        if (Status(StatusType.MaxSpeed) > chargeAmount)
        {
            chargeAmount += Status(StatusType.ChargeSpeed) * Time.deltaTime;
        }
    }

    /// <summary>
    /// チャージダッシュ
    /// </summary>
    protected virtual void ChargeDash()
    {
        //チャージダッシュはチャージが一定以上でなければ発動しない
        if (chargeAmount >= Status(StatusType.Charge) * chargeDashPossible)
        {
            speed += Status(StatusType.Acceleration) * chargeAmount;
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

        if (nowCharge)
        {
            nowCharge = false;
        }

        if (maxSpeed  > speed)
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
    #endregion

    #region private
    private void Start()
    {
        //ステータス倍率リストの初期化
        Array statusType = Enum.GetValues(typeof(StatusType));
        for(int i = 0; i < statusType.Length; i++)
        {
            statusList.Add(status.StartStatus((StatusType)i)); //初期値
        }

        //アイテム取得リストの初期化
        var itemType = Enum.GetValues(typeof(ItemName));
        foreach(var item in itemType)
        {
            getItemList.Add(-2); //初期値は-2
        }
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

        debugTextGetItem.text = "GET ITEM"
            + "\nMaxSpeed : " + getItemList[0]
            + "\nAcceleration : " + getItemList[1]
            + "\nTurning : " + getItemList[2]
            + "\nCharge : " + getItemList[3]
            + "\nWeight : " + getItemList[4]
            + "\nFly : " + getItemList[5]
            + "\nAll : " + getItemList[6];

        debugTextStatus.text = "STATUS"
            + "\nMaxSpeed : " + statusList[0]
            + "\nAcceleration : " + statusList[1]
            + "\nTurning : " + statusList[2]
            + "\nBrake : " + statusList[3]
            + "\nCharge : " + statusList[4]
            + "\nChargeSpeed : " + statusList[5]
            + "\nWeight : " + statusList[6]
            + "\nFlySpeed : " + statusList[7];
    }

    /// <summary>
    /// マシン影響オブジェクトに接触した際の処理
    /// </summary>
    /// <param name="other">接触した物体</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            Item item = other.gameObject.GetComponent<Item>();
            item.CatchItem(this); //入手したときの処理
        }

        if (other.gameObject.tag == "InfluenceObject")
        {
            //ダッシュボードに触れたときの速度に倍率をかける
            speed *= dashBoardMag;
        }
    }

    /// <summary>
    /// 接地判定
    /// </summary>
    /// <param name="collision">地面</param>
    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.tag == "StageObject" || collision.transform.tag == "NotBackSObject")
        {
            onGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "StageObject" || collision.transform.tag == "NotBackSObject")
        {
            onGround = false;
        }
    }
    #endregion
}