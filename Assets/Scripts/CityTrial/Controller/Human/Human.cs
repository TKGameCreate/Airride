using System.Collections;
using UnityEngine;
using Cinemachine;

public class Human : Control
{
    public enum AnimationType
    {
        Ride,
        GetOff,
        OnGround
    }

    [SerializeField] private Animator anim;
    [SerializeField] private Player player;
    [SerializeField] private Machine DefaultMachine;
    [SerializeField] private OnGroundHuman onGroundCollider;
    [SerializeField] private CinemachineVirtualCamera humanCamera;
    [SerializeField] private float speed = 5.0f; //速度
    [SerializeField] private float rotSpeed = 10.0f; //回転速度
    [SerializeField] private float onGroundRTime;
    [SerializeField] private float jumpPower = 1.5f;
    [SerializeField] private float downPower;
    [SerializeField] private float getOffXPower;
    [SerializeField] private float getOffYPower;
    [SerializeField] private float runPossibleNum;

    private bool ridePossible = true;
    private bool jumpPushButton = false;
    private bool jump = false;
    private bool exitMachineProcess = false; //降車後処理をしたか
    private bool getOffForce = true;

    public bool OnGround
    {
        set
        {
            onGround = value;
        }
    }

    #region public
    public override void Controller()
    {
        if(StateManager.State != StateManager.GameState.Game)
        {
            return;
        }

        //降車時処理
        if (!exitMachineProcess)
        {
            GetOff();
        }
        MoveAnimationControl();
        Move();
    }

    public override void FixedController()
    {
        if (jumpPushButton)
        {
            rbody.AddForce(Vector3.up * jumpPower);
            ridePossible = false;
            jumpPushButton = false;
        }

        if (!getOffForce)
        {
            rbody.AddRelativeForce(
            getOffXPower,
            getOffYPower,
            0,
            ForceMode.Impulse);
            getOffForce = true;
        }

        rbody.AddForce(Vector3.down * downPower);
        rbody.velocity = velocity;
    }

    public void AnimationControl(AnimationType type)
    {
        switch (type)
        {
            //マシンに乗るアニメーション
            case AnimationType.Ride:
                anim.SetFloat("speed", 0);
                anim.SetBool("jump", false);
                anim.SetTrigger("ride");
                return;
            case AnimationType.GetOff:
                anim.SetBool("getOff", true);
                ridePossible = false;
                getOffForce = false;
                return;
            case AnimationType.OnGround:
                if (anim.GetBool("getOff"))
                {
                    anim.SetBool("getOff", false);
                    ridePossible = true;
                }
                return;
            default:
                return;
        }
    }
    #endregion

    #region protected
    protected override void Move()
    {
        base.Move();

        //移動処理
        //前進
        if (!anim.GetBool("getOff"))
        {
            //スティックの角度に回転
            //アナログスティックのグラつきを想定して±0.01以下をはじく
            if (Mathf.Abs(horizontal) + Mathf.Abs(vertical) > 0.1f)
            {
                //カメラからみたプレイヤーの方向ベクトル
                Vector3 camToPlayer = transform.position - Camera.
                    main.transform.position;
                // π/2 - atan2(x,y) == atan2(y,x)
                float inputAngle = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg;
                float cameraAngle = Mathf.Atan2(camToPlayer.x, camToPlayer.z) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0, inputAngle + cameraAngle, 0);
                float rotHuman = transform.localRotation.w;
                
                //deltaTimeを用いることで常に一定の速度になる
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
                               
                //移動処理
                Vector3 worldVelocity = Vector3.zero;
                if(Mathf.Abs(horizontal) > Mathf.Abs(0.5f))
                {
                    worldVelocity = new Vector3(0, 0, Mathf.Abs(horizontal) * speed);
                }
                else
                {
                    worldVelocity = new Vector3(0, 0, Mathf.Abs(vertical) * speed);
                }
                velocity = transform.InverseTransformDirection(worldVelocity);
                velocity = new Vector3(-velocity.x, velocity.y, velocity.z);
            }
            else
            {
                velocity = Vector3.zero;
            }

            //ジャンプ処理
            if (InputManager.Instance.InputA(InputType.Down))
            {
                jump = true;
                jumpPushButton = true;
            }
        }
    }

    protected override void GetOff()
    {
        AnimationControl(AnimationType.GetOff);
        rbody.constraints = RigidbodyConstraints.FreezeRotation;
        //親子関係をPlayerの子供に
        transform.parent = player.transform;
        StartCoroutine(OnGroundResurrection());
        humanCamera.Priority = 10;
        //降車後処理の完了
        exitMachineProcess = true;
    }
    #endregion

    #region private
    private void Start()
    {
        rbody.constraints = RigidbodyConstraints.FreezeAll;
        //乗車アニメーション
        AnimationControl(AnimationType.Ride);
    }

    private void LayerSetting(bool col)
    {
        int humanLayer = LayerMask.NameToLayer("Human");
        int machineLayer = LayerMask.NameToLayer("Machine");

        Physics.IgnoreLayerCollision(humanLayer, machineLayer, !col);
    }

    private void MoveAnimationControl(bool ride = false)
    {
        //移動アニメーション
        if (velocity.magnitude > 0.1f)
        {
            anim.SetFloat("speed", velocity.magnitude);
        }
        else
        {
            anim.SetFloat("speed", 0);
        }
        //ジャンプアニメーション
        if (jump)
        {
            anim.SetBool("jump", true);
        }
        else
        {
            anim.SetBool("jump", false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Machineの近くでAボタンを押す
        if (other.gameObject.tag == "RideTrigger" 
            && InputManager.Instance.InputA(InputType.Down) 
            && ridePossible 
            && player.PlayerCondition == Player.Condition.Human)
        {
            GameObject machineObject = other.transform.root.gameObject;
            //自身(人)をマシンの子オブジェクトにする
            transform.parent = machineObject.transform;
            //MachineをPlayerの子オブジェクトに
            machineObject.transform.parent = player.transform;
            //位置をマシンの中心に
            transform.localPosition = Vector3.zero;
            transform.localRotation = new Quaternion(0, 0, 0, 0);
            //PlayerのConditionをHumanからMachineに
            player.PlayerCondition = Player.Condition.Machine;
            //マシンを割り当て
            Machine machine = machineObject.GetComponent<Machine>();
            player.Machine = machine;
            //マシンのPlayerを割り当て
            machine.Player = player;
            //当たり判定をなくす
            LayerSetting(false);
            rbody.constraints = RigidbodyConstraints.FreezeAll;
            //アニメーションリセット
            AnimationControl(AnimationType.Ride);
            //地面の当たり判定を非表示
            onGroundCollider.gameObject.SetActive(false);
            //カメラの優先度を最低に
            humanCamera.Priority = 1;
            //マシンのカメラ優先度を上げる
            machine.MachineCamera.Priority = 10;
            //エンジン音の再生
            machine.EngineAudio.Play();
            player.LastRideMachine = machine;
            //降車後の処理フラグをFalseに
            exitMachineProcess = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "RideTrigger")
        {
            LayerSetting(true);
        }
    }

    private IEnumerator OnGroundResurrection()
    {
        float measureTime = 0f;

        while (measureTime < onGroundRTime)
        {
            measureTime += Time.deltaTime;
            yield return null;
        }

        onGroundCollider.gameObject.SetActive(true);
    }
    #endregion

    #region AnimationEvent
    public void JumpEndAnimation()
    {
        ridePossible = true;
        jump = false;
    }

    public void EndGetOffOnGround()
    {
        ridePossible = true;
    }
    #endregion
}