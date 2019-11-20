using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    #region シングルトン
    private static InputManager instance = new InputManager();
    
    //コンストラクタ
    private InputManager()
    {
        Debug.Log("Create InputManager");
    }

    //インスタンスを取得
    public static InputManager Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    #region method
    /// <summary>
    /// メニュー画面の操作
    /// </summary>
    public void InputMenu()
    {

    }

    /// <summary>
    /// タイトル画面のインプット操作
    /// </summary>
    public void InputTitle()
    {

    }

    /// <summary>
    /// 左スティックの入力
    /// </summary>
    /// <param name="_horizontal">returnする値がHorizontalか</param>
    /// <returns>HorizontalかVertical</returns>
    public float InputLeftStick(bool _horizontal)
    {
        if (_horizontal)
        {
            return Input.GetAxis("Horizontal");
        }
        else
        {
            return Input.GetAxis("Vertical");
        }
    }

    /// <summary>
    /// ボタン入力
    /// </summary>
    /// <returns>入力しているかどうか</returns>
    public bool InputAButtonDown()
    {
        return Input.GetButtonDown("A");
    }

    public bool InputAButtonUp()
    {
        return Input.GetButtonUp("A");
    }

    public bool InputAButton()
    {
        return Input.GetButton("A");
    }
    #endregion
}
