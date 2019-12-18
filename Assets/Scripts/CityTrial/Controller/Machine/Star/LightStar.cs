using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStar : Machine
{
    [SerializeField] private Player defaultPlayer;

    public override void Controller()
    {
        if (state.State == StateManager.GameState.Game)
        {
            Move();
            RideTimeCount();
            if (getOffPossible)
            {
                GetOff();
            }
            rbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            //チャージのみ可能
            if (InputManager.Instance.InputA(InputType.Hold))
            {
                Charge();
            }
            else if (InputManager.Instance.InputA(InputType.Up))
            {
                chargeAmount = 1;
            }
            rbody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    protected override void Start()
    {
        player = defaultPlayer;
        base.Start();
    }
}
