using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] private Transform playerRootPosition;
    [SerializeField] private GameObject player;
    [SerializeField] private Player playerScript;
    [SerializeField] private Vector3 humanCameraPosition = new Vector3(0, 2, -3);
    [SerializeField] private Vector3 machineCameraPosition = new Vector3(0, 2, 3);
    private Player.Condition condition = Player.Condition.Human;

    // Update is called once per frame
    void Update()
    {
        if (playerScript.PlayerCondition == Player.Condition.Human)
        {
            transform.position = player.transform.position - humanCameraPosition;
        }

        if(condition != playerScript.PlayerCondition)
        {
            ParentSetting();
        }
    }

    private void ParentSetting()
    {
        condition = playerScript.PlayerCondition;
        if (playerScript.PlayerCondition == Player.Condition.Human)
        {
            //親をMachineではなくRootObjectに切り替え
            transform.parent = playerRootPosition;
            //回転座標を初期化
            transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            //親をMachineに
            transform.parent = playerScript.Machine.transform;
            //座標の初期化
            transform.localPosition = machineCameraPosition;
            transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
    }
}
