using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStar : Machine
{
    [SerializeField] private Player defaultPlayer;

    protected override void Start()
    {
        player = defaultPlayer;
        base.Start();
    }
}
