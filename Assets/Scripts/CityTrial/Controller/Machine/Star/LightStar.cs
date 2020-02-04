using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStar : Machine
{
    [SerializeField] private Player defaultPlayer;

    public override void Controller()
    {
        CheckPauseSound();
        if (StateManager.State == StateManager.GameState.Game)
        {
            Ground();
            Move();

            if (getOffPossible)
            {
                GetOff();
            }

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
        getOffPossible = true;
        base.Start();
    }
}
