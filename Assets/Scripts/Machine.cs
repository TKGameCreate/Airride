using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    private MachineStatus status;
    protected float speed = 0; //速度
    protected float chargeAmount = 1; //チャージ量

    private void OperationMachine()
    {
        //チャージ状態の処理
        if (InputManager.Instance.InputAButton())
        {
            //チャージ
            if (status.Charge < chargeAmount)
            {
                chargeAmount += 1 * status.ChargeSpeed;
            }
            //ブレーキ
            if(status.MaxSpeed > speed)
            {
                speed /= status.Brake;
            }
        }
        //チャージ状態じゃない時の処理
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
