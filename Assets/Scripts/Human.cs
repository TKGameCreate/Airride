using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Player
{
    [SerializeField] private Rigidbody rbody;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float rotSpeed = 10.0f;
    [SerializeField] private float jumpPower = 1.5f;
    [SerializeField] private Animator anim;

    private Vector3 velocity;
    private float horizontal = 0;
    private float vertical = 0;
    private bool jump = false;

    private void Update()
    {
        AnimationControl();
        Operation();
    }

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

    protected override void Operation()
    {
        //左スティック　キャラクターの移動
        //Aボタン　ジャンプ
        //Aボタン(マシンの近く)　マシンに乗る
        horizontal = InputManager.Instance.InputLeftStick(true);
        vertical = InputManager.Instance.InputLeftStick(false);

        //移動量
        velocity = new Vector3(horizontal * speed, 0, vertical * speed);

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
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed);
        }

        if (InputManager.Instance.InputAButtonDown())
        {
            jump = true;
            rbody.AddForce(Vector3.up * jumpPower);
        }
    }

    private void FixedUpdate()
    {
        rbody.velocity = velocity;
    }

    //AnimationEvent
    public void JumpEndAnimation()
    {
        jump = false;
    }
}
