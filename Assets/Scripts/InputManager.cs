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

    #region public
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
    #endregion

    #region private
    /// <summary>
    /// 人状態のInput操作
    /// </summary>
    private void InputHuman()
    {
        float horizontal = InputLeftStick(true);
        float vertical = InputLeftStick(false);

        Vector3 velocity =  new Vector3(horizontal, 0, vertical);
        
    }

    /// <summary>
    /// マシンに乗っている状態のInput操作
    /// </summary>
    private void InputMachine()
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
    /// <returns></returns>
    private bool InputButton()
    {
        return Input.GetButton("A");
    }
    #endregion
}
