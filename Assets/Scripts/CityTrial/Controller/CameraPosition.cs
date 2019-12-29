using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] private Player player;
    private Player.Condition condition = Player.Condition.Human;

    // Update is called once per frame
    void Update()
    {
        if (player.PlayerCondition == Player.Condition.Human)
        {
            
        }

        if(condition != player.PlayerCondition)
        {
            condition = player.PlayerCondition;
        }
    }
}
