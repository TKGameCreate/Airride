using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager
{
    #region シングルトン
    private static AudioManager instance = new AudioManager();

    //コンストラクタ
    private AudioManager()
    {
        Debug.Log("Create AudioManager");
    }

    //インスタンスを取得
    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion


}