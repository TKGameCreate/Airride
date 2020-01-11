using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStar : Machine
{
    [SerializeField] private Player defaultPlayer;

    public override void Controller()
    {
        ChackPauseSound();
        if (StateManager.State == StateManager.GameState.Game)
        {
            Move();
            GetOff();
            EngineSound();
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

        if (debug)
        {
            DebugDisplay();
        }
    }

    public void SetPlayer()
    {
        Player = defaultPlayer;
        Player.LastRideMachine = this;
        Player.Machine = this;
    }

    protected override void Start()
    {
        engineAudioSource.Play();
        chargeAudioSource.Play();
        SetPlayer();
        base.Start();
    }
}
