﻿using System.Collections;
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

    #region プロパティ
    public float Horizontal
    {
        get
        {
            return Input.GetAxis("Horizontal");
        }
    }

    public float Vertical
    {
        get
        {
            return Input.GetAxis("Vertical");
        }
    }

    public float HorizontalRaw
    {
        get
        {
            return Input.GetAxisRaw("Horizontal");
        }
    }

    public float VerticalRaw
    {
        get
        {
            return Input.GetAxisRaw("Vertical");
        }
    }

    public bool Pause
    {
        get
        {
            return Input.GetButtonDown("Pause");
        }
    }

    public bool Any
    {
        get
        {
            return Input.anyKeyDown;
        }
    }

    public bool InputB
    {
        get
        {
            return Input.GetButtonDown("B");
        }
    }
    #endregion

    #region method
    public bool InputA(InputType _inputType)
    {
        switch (_inputType)
        {
            case InputType.Hold:
                return Input.GetButton("A");
            case InputType.Down:
                return Input.GetButtonDown("A");
            case InputType.Up:
                return Input.GetButtonUp("A");
            default:
                return false;
        }
    }
    #endregion
}

public enum InputType
{
    Hold,
    Down,
    Up,
}