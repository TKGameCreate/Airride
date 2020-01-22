using System;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Collections;

public class Machine : Control
{
    #region const
    private const float boundPower = 1000.0f; //バウンド
    private const float chargeUnderPower = 50000.0f; //charge中に下に加える力
    private const float flyWeightMag = 100f; //滑空時の落下倍率
    private const float dashBoardMag = 2.5f; //ダッシュボード倍率
    private const float itemInsPlusYPos = 2.5f; //降りた時アイテムがインスタンス化されるY軸ポジションのプラス値
    private const float maxPitchSpeed = 200.0f; //最高ピッチ速度
    private const float getOffCoolTime = 2.0f; //降りることができるまでのクールダウン
    private const int defaultStatus = -2; //アイテム取得数デフォルト値
    protected const int maxGenerate = 4; //最大アイテム生成数
    protected const float maxPitch = 2.0f; //最高ピッチ
    protected const float flyChargeSpeed = 1500f; //滑空中の自動チャージ速度分率
    protected const float exitMachineVertical = -0.9f; //降車時スティック最低入力量
    protected const float chargeDashPossible = 0.75f; //チャージダッシュ可能量
    public const int limitStatus = 16; //アイテム取得数下限上限
    #endregion

    #region Serialize
    [SerializeField] protected bool debug = false;
    [SerializeField] protected AudioSource engineAudioSource;
    [SerializeField] protected AudioSource chargeAudioSource;
    [SerializeField] protected AudioClip groundSE;
    [SerializeField] protected MachineStatus status;
    [SerializeField] protected CinemachineVirtualCamera vcamera;
    [SerializeField] private ItemList itemList;
    [SerializeField] private DebugText dText;
    [SerializeField] private Transform ridePosition;
    [SerializeField] protected float defaultChargeAmount = 1;
    #endregion

    #region 変数
    protected float speed = 0; //現在の速度
    protected float chargeAmount = 1; //チャージ量
    protected List<int> getNumberList = new List<int>(); //取得したアイテムのNoリスト
    protected bool nowBrake = false; //charge中かどうか
    protected bool getOffPossible = false; //降りれるかどうか
    protected Vector3 chargePos;
    private List<int> getNumItemList = new List<int>();    //アイテムの取得状態
    private List<float> statusList = new List<float>();    //ステータスのバフ状態
    #endregion

    #region プロパティ
    public Player Player { protected set; get; }
    public MachineStatus MachineStatus
    {
        get
        {
            return status;
        }
    }
    public float SaveSpeed { get; private set; } = 0;
    public Vector3 RidePosition
    {
        get
        {
            if(ridePosition == null)
            {
                return Vector3.zero;
            }
            else
            {
                return ridePosition.localPosition;
            }
        }
    }
    #endregion

    #region public
    #region Control
    public override void Controller()
    {
        Move();
        ChackPauseSound();
        EngineSound();

        if(getOffPossible)
        {
            GetOff();
        }

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
    /// <param name="name">変動させるステータス</param>
    /// <param name="mode">変更値</param>
    /// <returns>上限下限か</returns>
    public bool ItemCount(ItemName name, ItemMode mode)
    {
        switch (mode)
        {
            //バフアイテム
            case ItemMode.Buff:
                if (mode == ItemMode.Buff)
                {
                    //上限チェック
                    if (getNumItemList[(int)name] < limitStatus)
                    {
                        getNumberList.Add((int)name);
                        getNumItemList[(int)name]++; //アイテムの取得数を増やす
                        return false;
                    }
                }
                return true;
            //デバフアイテム
            case ItemMode.Debuff:
                //下限チェック
                if (getNumItemList[(int)name] > -limitStatus)
                {
                    getNumberList.Remove((int)name);
                    getNumItemList[(int)name]--; //アイテム取得数を減らす
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
    public void ChangeStatus(StatusType name, ItemMode mode, float mag = 1)
    {
        bool plus;
        //Statusが基準ステータスより高かったら
        if(statusList[(int)name] >= status.GetStatus(name, MachineStatus.Type.Default))
        {
            //計算基準をPlus値で行う
            plus = true;
        }
        else
        {
            plus = false;
        }

        Debug.Log("statusList[(int)name]" + statusList[(int)name]);
        Debug.Log("status.GetStatus(name, MachineStatus.Type.Default)" + status.GetStatus(name, MachineStatus.Type.Default));
        Debug.Log("plus" + plus);

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
        float moveSpeed = Mathf.Clamp(rbody.velocity.magnitude, 0, 999);
        float intSpeed = moveSpeed - moveSpeed % 1; //整数部分のみ抽出
        float fewSpeed = moveSpeed % 1; //小数部分のみ抽出

        return intSpeed.ToString("000")
            + "\n"
            + "<size=30>"
            + fewSpeed.ToString(".00")
            + "</size>";
    }

    /// <summary>
    /// アイテムの取得数(0基準)
    /// </summary>
    /// <param name="num">アイテムの種類</param>
    /// <returns>取得数</returns>
    public int GetItemTotal(int type)
    {
        int getNum = getNumItemList[type] + -defaultStatus;
        if(getNum < 0)
        {
            getNum = 0;
        }
        return getNum;
    }

    /// <summary>
    /// アイテムの取得数ゲージ用正規化(0基準)
    /// </summary>
    /// <param name="type">アイテムの種類</param>
    /// <returns>取得数(正規化)</returns>
    public float NormalizeGetItemTotal(int type)
    {
        float max = limitStatus + -defaultStatus; //18基準
        float normalize = (getNumItemList[type] + -defaultStatus) / max;
        if (normalize < 0)
        {
            normalize = 0;
        }
        return normalize;
    }

    /// <summary>
    /// 正規化したチャージ量
    /// </summary>
    /// <returns>チャージ量</returns>
    public float NormalizeCharge()
    {
        //0~1の範囲に正規化
        float charge = (chargeAmount - defaultChargeAmount) / (Status(StatusType.Charge) - defaultChargeAmount);
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


    /// <summary>
    /// 接地判定
    /// </summary>
    /// <param name="other">地面</param>
    public void OnGround(Collider other)
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
                    chargeAmount = defaultChargeAmount;
                }
                //接地SEを再生
                AudioManager.Instance.PlaySE(groundSE);
            }
            onGround = true;
        }
    }

    public void ExitGround(Collider other)
    {
        if (other.transform.tag == "StageObject" || other.transform.tag == "NotBackSObject")
        {
            onGround = false;
        }
    }

    public void RideThisMachine(Player _player)
    {
        //マシンのPlayerを割り当て
        Player = _player;
        //マシンのカメラ優先度を上げる
        vcamera.Priority = 10;
        StartCoroutine(GetOffCoolTime());
        //エンジン音の再生
        engineAudioSource.Play();
        chargeAudioSource.Play();
    }
    #endregion
    #endregion

    #region protected
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

        float normalize = NormalizeCharge();
        chargeAudioSource.volume = normalize;
        chargeAudioSource.pitch = Mathf.Clamp(1 + normalize * 2, 1, maxPitch + 1);
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
        if (NormalizeCharge() >= chargeDashPossible)
        {
            speed += Status(StatusType.Acceleration) * chargeAmount;
        }
        chargeAudioSource.volume = 0;
        StartCoroutine(PitchResetOneFlameLater());
        //チャージ量をリセット
        chargeAmount = defaultChargeAmount;
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
            getOffPossible = false;
            vcamera.Priority = 1;//マシンカメラの優先度を最低に
            chargeAmount = defaultChargeAmount;
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

    protected void DropItem()
    {
        if (!(getNumberList?.Count > 0))
        {
            return;
        }

        int getItemSum = getNumberList.Count; //生成可能数合計値
        if (getItemSum > maxGenerate)
        {
            getItemSum = maxGenerate;
        }
        //生成数の決定
        int generateNum = UnityEngine.Random.Range(0, getItemSum + 1);
        for (int i = 0; i < generateNum; i++)
        {
            //どのアイテムを生成するか決定する
            int instanceNo = UnityEngine.Random.Range(0, getNumberList.Count - 1);
            int itemNo = getNumberList[instanceNo]; //アイテムの番号を取得
            #region Instance
            Vector3 instancePos = new Vector3
                (transform.position.x,
                transform.position.y + itemInsPlusYPos,
                transform.position.z);
            itemList.InstantiateItem(instancePos, itemNo, generateNum, i);
            #endregion
            //リストから生成したNoを削除
            getNumberList.Remove(itemNo);
            //生成したアイテムをGetItemNumから-1
            getNumItemList[itemNo]--;
            //ステータスを減少
            itemList.ChangeStatusDropItem(itemNo, this);
        }
    }

    protected void ChackPauseSound()
    {
        if (Time.timeScale != 1)
        {
            engineAudioSource.Pause();
            chargeAudioSource.Pause();
        }
        else
        {
            engineAudioSource.UnPause();
            chargeAudioSource.UnPause();
        }
    }

    protected void EngineSound()
    {
        engineAudioSource.pitch = Mathf.Clamp(1 + rbody.velocity.magnitude * (maxPitch / maxPitchSpeed), 1, maxPitch + 1);
    }

    /// <summary>
    /// デバッグテキスト処理
    /// </summary>
    protected void DebugDisplay()
    {
        dText.Debug(DebugText.Position.Right,
            "GET ITEM"
            + "\nMaxSpeed : " + getNumItemList[0]
            + "\nAcceleration : " + getNumItemList[1]
            + "\nTurning : " + getNumItemList[2]
            + "\nCharge : " + getNumItemList[3]
            + "\nWeight : " + getNumItemList[4]
            + "\nFly : " + getNumItemList[5]
            + "\nAll : " + getNumItemList[6],
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
            getNumItemList.Add(defaultStatus);//Defalut値の設定
        }

        chargeAmount = defaultChargeAmount;
    }

    protected IEnumerator PitchResetOneFlameLater()
    {
        yield return null;
        chargeAudioSource.pitch = 1;
    }
    #endregion

    #region private
    /// <summary>
    /// マシン影響オブジェクトに接触した際の処理
    /// </summary>
    /// <param name="other">接触した物体</param>
    private void OnTriggerEnter(Collider other)
    {
        if(Player == null)
        {
            return;
        }

        switch (other.gameObject.tag)
        {
            case "Item":
                Item item = other.gameObject.GetComponent<Item>();
                item.CatchItem(this); //入手したときの処理
                break;
            case "InfluenceObject":
                speed *= dashBoardMag;
                break;
            case "DamageObject": //ダメージオブジェクト
                float damageMag = 5.0f;
                //ダメージを受けるオブジェクトに触れた場合、アイテムを落とす
                Bound(boundPower * damageMag, true);
                DropItem();
                break;
            case "JumpObject": //ジャンプパッド
                float jumpPower = other.GetComponent<JumpObject>().JumpPower;
                rbody.AddForce(new Vector3(0, jumpPower, 0));
                break;
            default:
                break;
        }
    }

    private IEnumerator GetOffCoolTime()
    {
        yield return new WaitForSeconds(getOffCoolTime);
        getOffPossible = true;
    }
    #endregion
}