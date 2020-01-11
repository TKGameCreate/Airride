using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テスト用Sceneチェンジスクリプト
/// </summary>
public class TestLoad : MonoBehaviour
{
    public void InputButtonSceneChange(int no)
    {
        AirrideScene scene = (AirrideScene)no;
        AirrideSceneManager.Instance.LoadScene(scene);
    }
}
