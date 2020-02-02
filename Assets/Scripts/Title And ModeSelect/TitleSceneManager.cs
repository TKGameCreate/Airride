using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneManager : MonoBehaviour
{
    private enum Scene
    {
        Title,
        ModeSelect,
        Option
    }

    [SerializeField] private RectTransform titleScene = null;
    [SerializeField] private RectTransform modeSelectScene = null;
    [SerializeField] private ModeSelect modeSelect = null;

    private Scene scene = Scene.Title;

    private void Update()
    {
        switch (scene)
        {
            case Scene.Title:
                if (InputManager.Instance.Any)
                {
                    AudioManager.Instance.PlaySystemSE(0);
                    ChangeToModeSelect();
                }
                break;
            case Scene.ModeSelect:
                if (InputManager.Instance.InputB)
                {
                    AudioManager.Instance.PlaySystemSE(0);
                    ChangeToTitle();
                    break;
                }
                modeSelect.Select();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// タイトル画面に画面移動
    /// </summary>
    private void ChangeToTitle()
    {
        ChangeScene(modeSelectScene, titleScene);
        scene = Scene.Title;
    }

    /// <summary>
    /// モードセレクト画面に
    /// </summary>
    private void ChangeToModeSelect()
    {
        ChangeScene(titleScene, modeSelectScene);
        modeSelect.ResetSelect();
        scene = Scene.ModeSelect;
    }

    private void ChangeScene(RectTransform from, RectTransform to)
    {
        from.gameObject.SetActive(false);
        to.gameObject.SetActive(true);
    }
}
