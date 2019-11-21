using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : Player
{
    [SerializeField] private Rigidbody rbody;
    [SerializeField] private MachineStatus status;
    private float speed = 0; //速度
    private float chargeAmount = 1; //チャージ量

    private float horizontal = 0;
    private Vector3 velocity;

    private void Start()
    {
        rbody.mass = status.Weight;
    }

    private void Update()
    {
        horizontal = InputManager.Instance.InputLeftStick(true);

        //自動加速
        if (status.MaxSpeed > speed)
        {
            speed += status.Acceleration * chargeAmount;
        }
        else
        {
            //徐々に速度を落とす
        }
        Debug.Log("速度" + speed);
        //移動量
        velocity = new Vector3(speed, 0, horizontal * status.Turning);
        velocity = transform.TransformDirection(velocity);
        transform.localPosition += velocity * Time.deltaTime;
    }

    protected void OperationMachine()
    {
        //チャージ時じゃない時の処理
        if (!InputManager.Instance.InputAButton())
        {
            //チャージ
            if (status.MaxCharge < chargeAmount)
            {
                chargeAmount += 1 * status.ChargeSpeed;
            }
            //ブレーキ
            if(status.MaxSpeed > speed)
            {
                speed /= status.Brake;
            }
        }
        //チャージ時の処理
        else
        {
            //自動加速
            if (status.MaxSpeed < speed)
            {
                speed += status.Acceleration * chargeAmount;
            }
        }
    }
}
