using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Player
{
    [SerializeField] private Rigidbody rbody;
    [SerializeField] private Transform controlObject;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float rotSpeed = 10.0f;

    private float horizontal;
    private float vertical;

    protected override void Operation()
    {
        //左スティック　キャラクターの移動
        //Aボタン　ジャンプ
        //Aボタン(マシンの近く)　マシンに乗る
        horizontal = InputManager.Instance.InputLeftStick(true);
        vertical = InputManager.Instance.InputLeftStick(false);

        //アナログスティックのグラつきを想定して±0.01以下をはじく
        if (Mathf.Abs(horizontal) + Mathf.Abs(vertical) > 0.1F)
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
    }

    private void FixedUpdate()
    {
        rbody.velocity = new Vector3(horizontal, 0, vertical);
    }
}
