using System.Collections; 　　　　　　　
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    [SerializeField] private Camera mainCamera = null;
    [SerializeField] private Camera resultCamera = null;
    [SerializeField] private AudioClip resultBGM = null;
    [SerializeField] private Vector3 instancePosition = Vector3.zero;
    [SerializeField] private ResultGetItemList getItemList = null;
    [SerializeField] private Player player = null;
    [SerializeField] private float rotSpeed = 1;
    private Machine machine = default;

    public void DisplayResult()
    {
        //リザルトBGMの再生
        AudioManager.Instance.PlayBGM(resultBGM);

        GameObject[] mainGameUIs = GameObject.FindGameObjectsWithTag("MainGameUI");
        foreach(GameObject ui in mainGameUIs)
        {
            ui.SetActive(false);
        }

        InstanceMachine();
        SetResultCamera();
        //取得アイテムの表示
        getItemList.SetDisplay();
        Debug.Log("終了");
    }

    /// <summary>
    /// Playerのマシンの生成
    /// </summary>
    private void InstanceMachine()
    {
        //最後に乗っていたマシンの表示
        machine = player.LastRideMachine;
        Vector3 pos = new Vector3(resultCamera.transform.position.x + instancePosition.x,
            resultCamera.transform.position.y + instancePosition.y,
            resultCamera.transform.position.z + instancePosition.z);
        machine.transform.position = pos;
        machine.transform.rotation = Quaternion.identity;
        machine.transform.rotation = Quaternion.Euler(0, 180, 0);

        //Machine machineInstance = Instantiate(machine,
        //    pos,
        //    Quaternion.Euler(0, 180, 0)) as Machine;
        //machineInstance.enabled = false;
    }

    /// <summary>
    /// カメラの切り替え
    /// </summary>
    private void SetResultCamera()
    {
        resultCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        Debug.Log(machine);
        machine.transform.RotateAround(machine.transform.position,
        resultCamera.transform.position,
        Time.unscaledDeltaTime * rotSpeed);
    }
}