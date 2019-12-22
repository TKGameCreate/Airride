using System;
using System.Collections.Generic;
using UnityEngine;

public class Flash
{
    private FlashTime[] flashTime;
    private float time = 0;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public Flash(FlashTime[] _flashTime)
    {
        flashTime = _flashTime;
    }

    /// <summary>
    /// 点滅開始フラグ
    /// </summary>
    /// <param name="r">点滅させたいオブジェクトのレンダラー</param>
    public void FlashTime(Renderer r)
    {
        if (StateManager.State == StateManager.GameState.Game)
        {
            time += Time.deltaTime;
            //要素数分回す
            for (int i = flashTime.Length - 1; i >= 0; i--)
            {
                //点滅を開始するかどうかの判定
                if (flashTime[i].Start < time)
                {
                    //点滅処理
                    Flashing(r, flashTime[i].Interval);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 点滅処理
    /// </summary>
    /// <param name="comp">Componentクラス</param>
    /// <param name="_interval">表示間隔</param>
    private void Flashing(Renderer r, float _interval)
    {
        r.enabled = isDesplay(_interval);
    }

    /// <summary>
    /// 表示するかどうか
    /// </summary>
    /// <param name="_interval">表示間隔</param>
    /// <returns>表示</returns>
    private bool isDesplay(float _interval)
    {
        float f = 1.0f / _interval;
        float sin = Mathf.Sin(2 * Mathf.PI * f * Time.time);
        if(sin < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}