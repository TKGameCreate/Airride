using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Control
{
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rbody;
    [SerializeField] private float speed = 2.5f;
    [SerializeField] private float rotSpeed = 10.0f;
    [SerializeField] private float jumpPower = 1.5f;

    private Vector3 velocity;
    private bool jump = false;

    #region public
    public override void Controller()
    {
        AnimationControl();
        Move();
    }

    public void FixedController()
    {
        rbody.velocity = velocity;
    }
    #endregion

    #region private and protected
    private void AnimationControl()
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

        //マシンに乗るアニメーション
    }

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
            rbody.AddForce(Vector3.up * jumpPower);
        }
    }
    #endregion

    #region AnimationEvent
    public void JumpEndAnimation()
    {
        jump = false;
    }
    #endregion
}
