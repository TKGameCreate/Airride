using System;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Machine : Control
{
    #region const
    const float chargeDashPossible = 0.75f; //チャージダッシュ可能量
    const float exitMachineVertical = -0.9f; //降車時スティック最低入力量
    const float chargeUnderPower = 50000.0f; //charge中に下に加える力
    const float flyWeightMag = 100f; //滑空時の落下倍率
    const float flyChargeSpeed = 1500f; //滑空中の自動チャージ速度分率
    const float dashBoardMag = 2.5f; //ダッシュボード倍率
    const float GetOffPossibleTime = 2.0f; //乗車してから降車可能までの時間
    const float itemInsPlusYPos = 2.5f; //降りた時アイテムがインスタンス化されるY軸ポジションのプラス値
    const int defaultStatus = -2; //アイテム取得数デフォルト値
    public const int limitStatus = 16; //アイテム取得数下限上限
    #endregion

    #region Serialize
    [SerializeField] protected bool debug = false;
    [SerializeField] protected StateManager state;
    [SerializeField] protected MachineStatus status;
    [SerializeField] private CinemachineVirtualCamera vcamera;
    [SerializeField] private ItemList itemList;
    [SerializeField] private DebugText dText;
    [SerializeField] private int maxGenerate = 4; 
    #endregion

    #region 変数
    protected float speed = 0; //現在の速度
    protected float chargeAmount = 1; //チャージ量
    private List<int> getItemList = new List<int>();    //アイテムの取得状態
    private List<float> statusList = new List<float>();    //ステータスのバフ状態
    private bool nowBrake = false; //charge中かどうか
    private Vector3 chargePos;
    #endregion

    #region プロパティ
    public Player Player { set; get; }
    public MachineStatus MachineStatus
    {
        get
        {
            return status;
        }
    }
    public float SaveSpeed { get; private set; } = 0;
    public CinemachineVirtualCamera MachineCamera
    {
        get
        {
            return vcamera;
        }
    }
    #endregion

    #region public
    #region Control
    public override void Controller()
    {
        if (!vcamera.gameObject.activeSelf)
        {
            vcamera.Priority = 10;
        }

        Move();
        GetOff();

        if (debug)
        {
            DebugDisplay();
        }
    }

    public override void FixedController()
    {
        //移動量
        if (nowBrake)
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
            if (nowBrake)
            {
                rbody.AddForce(Vector3.down * chargeUnderPower);
            }
        }
    }
    #endregion

    #region アイテム処理
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
    #endregion

    #region SpeedMater
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
    #endregion

    #region other
    /// <summary>
    /// 壁や、アイテムボックスにぶつかった時に跳ね返る処理
    /// </summary>
    public void Bound(float power, bool zero)
    {
        SaveSpeed = speed;
        if (zero)
        {
            speed = 0;
        }
        else
        {
            speed /= 2;
        }
        rbody.AddRelativeForce(
            (Vector3.back * SaveSpeed * power) 
            / Status(StatusType.Weight) * 0.5f,
            ForceMode.Impulse);
    }

    public float Status(StatusType name)
    {
        return statusList[(int)name];
    }
    #endregion
    #endregion

    #region protected
    protected override void Move()
    {
        base.Move();

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

        transform.Rotate(0, horizontal * Status(StatusType.Turning) * Time.deltaTime, 0);
    }

    /// <summary>
    /// チャージ処理
    /// </summary>
    protected virtual void Charge()
    {
        //チャージ
        if (Status(StatusType.Charge) > chargeAmount)
        {
            chargeAmount += Status(StatusType.ChargeSpeed) * Time.deltaTime;
        }
    }

    /// <summary>
    /// ブレーキ処理
    /// </summary>
    protected virtual void Brake()
    {
        if (!nowBrake)
        {
            chargePos = transform.forward;
            nowBrake = true;
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
        nowBrake = false;
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

        if (nowBrake)
        {
            nowBrake = false;
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

    /// <summary>
    /// 降車処理
    /// </summary>
    protected override void GetOff()
    {
        //スティック下+Aボタンかつ接地しているとき
        if (InputManager.Instance.InputA(InputType.Hold) 
            && vertical < exitMachineVertical 
            && onGround)
        {
            //アイテムを落とす
            DropItem();
            //マシンのカメラを非表示に
            vcamera.Priority = 1;//優先度を最低に
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
            return;
        }
    }

    /// <summary>
    /// デバッグテキスト処理
    /// </summary>
    protected void DebugDisplay()
    {
        dText.Debug(DebugText.Position.Right,
            "GET ITEM"
            + "\nMaxSpeed : " + getItemList[0]
            + "\nAcceleration : " + getItemList[1]
            + "\nTurning : " + getItemList[2]
            + "\nCharge : " + getItemList[3]
            + "\nWeight : " + getItemList[4]
            + "\nFly : " + getItemList[5]
            + "\nAll : " + getItemList[6],
            Player);

        dText.Debug(DebugText.Position.Left,
            "STATUS"
            + "\nMaxSpeed : " + statusList[0]
            + "\nAcceleration : " + statusList[1]
            + "\nTurning : " + statusList[2]
            + "\nBrake : " + statusList[3]
            + "\nCharge : " + statusList[4]
            + "\nChargeSpeed : " + statusList[5]
            + "\nWeight : " + statusList[6]
            + "\nFlySpeed : " + statusList[7],
            Player);
    }

    protected virtual void Start()
    {
        //ステータス倍率リストの初期化
        Array statusType = Enum.GetValues(typeof(StatusType));
        for (int i = 0; i < statusType.Length; i++)
        {
            statusList.Add(status.StartStatus((StatusType)i)); //初期値
        }

        //アイテム取得リストの初期化
        var itemType = Enum.GetValues(typeof(ItemName));
        foreach (var item in itemType)
        {
            getItemList.Add(defaultStatus);//Defalut値の設定
        }
    }
    #endregion

    #region private
    private void DropItem()
    {
        //アイテムの生成
        List<int> instancePossibleItems = new List<int>(); //生成可能アイテムの配列番号を格納するリスト
        int getItemSum = 0; //生成可能数合計値
        //アイテム獲得リストの大きさ分回す
        for (int i = 0; i < getItemList.Count; i++)
        {
            //アイテム獲得数が、デフォルト値を超えていた場合
            if (getItemList[i] > defaultStatus)
            {
                getItemSum += getItemList[i] + (-defaultStatus); //基準を0にして足す
                instancePossibleItems.Add(i);//生成できるアイテムの番号
            }
        }
        //アイテムの取得合計数が最大生成可能数を超えていた場合、
        //取得合計数を最大生成可能数に合わせる
        if (getItemSum > maxGenerate)
        {
            getItemSum = maxGenerate;
        }
        
        //リストが空じゃない場合
        if (instancePossibleItems?.Count > 0)
        {
            //生成数の決定
            int generateNum = UnityEngine.Random.Range(0, getItemSum + 1);
            for (int i = 0; i < generateNum; i++)
            {
                //どのアイテムを生成するか決定する
                int instanceNo = UnityEngine.Random.Range(0, instancePossibleItems.Count);
                int itemNo = instancePossibleItems[instanceNo]; //生成するアイテムの配列番号
                //生成
                Vector3 instancePos = new Vector3
                    (transform.position.x,
                    transform.position.y + itemInsPlusYPos,
                    transform.position.z);
                itemList.InstantiateItem(instancePos, itemNo, generateNum, i);
                //生成したアイテムをGetItemNumから-1
                getItemList[itemNo]--;
                Item item = itemList.GetItem(itemNo);
                item.ChangeStatus (this, ItemMode.Debuff);
                //アイテム獲得数がデフォルト値を下回った
                if (getItemList[itemNo] < defaultStatus)
                {
                    //アイテム生成可能リストから該当アイテムの配列番号を削除
                    instancePossibleItems.Remove(instanceNo);
                }
            }
        }
    }

    /// <summary>
    /// マシン影響オブジェクトに接触した際の処理
    /// </summary>
    /// <param name="other">接触した物体</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item" && Player != null)
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
    protected virtual void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "StageObject" || collision.transform.tag == "NotBackSObject")
        {
            onGround = true;
        }
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "StageObject" || collision.transform.tag == "NotBackSObject")
        {
            onGround = false;
        }
    }

    #endregion
}