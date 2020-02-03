using System;
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

    public void LoadScene(AirrideScene scene)
    {
        if(Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadSceneAsync((int)scene);
    }
    
    public int SceneIndex()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        foreach(var scene in Enum.GetValues(typeof(AirrideScene)))
        {
            if(scene.ToString() == sceneName)
            {
                return (int)scene;
            }
        }
        return 0;
    }
}

public enum AirrideScene : int
{
    Title = 0,
    CityTrial = 1,
    Result = 2
}
