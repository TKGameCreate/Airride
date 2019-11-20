using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    private MachineStatus status;
    protected float speed = 0; //速度
    protected float chargeAmount = 0; //チャージ量

    private void OperationMachine()
    {
        //自動加速
        if (status.MaxSpeed < speed)
        {
            speed += status.Acceleration;
        }

        if (InputManager.Instance.InputAButton())
        {
            if (status.Charge < chargeAmount)
            {
                speed /= status.Brake;
                chargeAmount += 1 * status.ChargeSpeed;
            }
        }

        if (InputManager.Instance.InputAButtonUp())
        {

        }
    }
}
