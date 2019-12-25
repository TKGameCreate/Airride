using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] private Transform playerRootPosition;
    [SerializeField] private Player player;
    [SerializeField] private Human human;
    [SerializeField] private Vector3 humanCameraPosition = new Vector3(0, 2, -3);
    [SerializeField] private Vector3 machineCameraPosition = new Vector3(0, 2, 3);
    private Player.Condition condition = Player.Condition.Human;

    // Update is called once per frame
    void Update()
    {
        if (player.PlayerCondition == Player.Condition.Human)
        {
            transform.position = human.transform.position - humanCameraPosition;
        }

        if(condition != player.PlayerCondition)
        {
            ParentSetting();
        }
    }

    private void ParentSetting()
    {
        condition = player.PlayerCondition;
        if (player.PlayerCondition == Player.Condition.Human)
        {
            //親をMachineではなくRootObjectに切り替え
            transform.parent = playerRootPosition;
            //回転座標を初期化
            transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            //親をMachineに
            transform.parent = player.Machine.transform;
            //座標の初期化
            transform.localPosition = machineCameraPosition;
            transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
    }
}
