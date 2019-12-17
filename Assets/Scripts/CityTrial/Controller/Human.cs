﻿using UnityEngine;

public class Human : Control
{
    public enum AnimationType
    {
        Ride,
        GetOff,
        OnGround
    }

    [SerializeField] private Animator anim;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private Player player;
    [SerializeField] private GameObject onGroundColliderObject;
    [SerializeField] private Machine DefaultMachine;
    [SerializeField] private float speed = 5.0f; //速度
    [SerializeField] private float rotSpeed = 10.0f; //回転速度
    [SerializeField] private float jumpPower = 1.5f;
    [SerializeField] private float downPw;
    [SerializeField] private float rideCoolDownTime;

    private float rideCoolDownTimeCount = 0;
    private bool ridePossible = true;
    private bool jumpPushButton = false;
    private bool jump = false;
    private bool exitMachineProcess = false; //降車後処理をしたか

    #region public
    public override void Controller()
    {
        //降車時処理
        if (!exitMachineProcess)
        {
            GetOff();
        }

        //降車モーション状態の場合
        if (!ridePossible && anim.GetBool("getOff"))
        {
            //クールダウンの時間をカウント
            rideCoolDownTimeCount += Time.deltaTime;
            if(rideCoolDownTimeCount > rideCoolDownTime)
            {
                rideCoolDownTimeCount = 0;
                //接地判定のコライダーをTrueに
                onGroundColliderObject.SetActive(true);
            }
        }
        else
        {
            MoveAnimationControl();
            Move();
        }
    }

    public override void FixedController()
    {
        if (jumpPushButton)
        {
            rbody.AddForce(Vector3.up * jumpPower);
            ridePossible = false;
            jumpPushButton = false;
        }

        if(ridePossible && anim.GetBool("getOff"))
        {
            rbody.AddForce(Vector3.up);
            rbody.AddForce(Vector3.back);
        }

        rbody.AddForce(Vector3.down * downPw);
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
                return;
            case AnimationType.OnGround:
                anim.SetBool("getOff", false);
                onGroundColliderObject.SetActive(false);
                if(capsuleCollider.isTrigger == true)
                {
                    capsuleCollider.isTrigger = false;
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
        velocity = new Vector3(horizontal * speed, 0, vertical * speed);

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
            //deltaTimeを用いることで常に一定の速度になる
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }

        //ジャンプ処理
        if (InputManager.Instance.InputA(InputType.Down))
        {
            jump = true;
            jumpPushButton = true;
        }
    }

    protected override void GetOff()
    {
        AnimationControl(AnimationType.GetOff);
        //当たり判定の復活
        capsuleCollider.enabled = true;
        capsuleCollider.isTrigger = true;
        rbody.constraints = RigidbodyConstraints.FreezeRotation;
        //親子関係をPlayerの子供に
        transform.parent = player.transform;
        //高さの調整
        Vector3 pos = transform.position;
        Vector3 newPos = new Vector3(pos.x, 0.7f, pos.z);
        transform.position = newPos;
        //降車後処理の完了
        exitMachineProcess = true;
    }
    #endregion

    #region private
    private void Start()
    {
        capsuleCollider.enabled = false;
        rbody.constraints = RigidbodyConstraints.FreezeAll;
        //乗車アニメーション
        AnimationControl(AnimationType.Ride);
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
            capsuleCollider.enabled = false;
            rbody.constraints = RigidbodyConstraints.FreezeAll;
            //アニメーションリセット
            AnimationControl(AnimationType.Ride);
            //降車後の処理フラグをFalseに
            exitMachineProcess = false;
        }

        //クールダウンが終わってから、何か物に接触した場合、TriggerがTrueのままだったら
        if (rideCoolDownTimeCount > rideCoolDownTime && capsuleCollider.isTrigger == true)
        {
            //衝突判定を復活
            capsuleCollider.isTrigger = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Machine")
        {
            capsuleCollider.isTrigger = false;
        }
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