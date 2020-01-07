using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AirrideSceneManager
{
    #region シングルトン
    private static AirrideSceneManager instance = new AirrideSceneManager();

    //コンストラクタ
    private AirrideSceneManager()
    {
        Debug.Log("Create AirrideSceneManager");
    }

    //インスタンスを取得
    public static AirrideSceneManager Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    public void LoadScene(Scene scene)
    {
        if(Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadScene((int)scene);
    }
}

public enum Scene : int
{
    Title = 0,
    CityTrial = 1,
    Result = 2
}
