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
    [SerializeField] private RectTransform optionScene = null;
    [SerializeField] private ModeSelect modeSelect = null;
    [SerializeField] private TimeSetting timeSetting = null;

    private Scene scene = Scene.Title;

    private void Update()
    {
        switch (scene)
        {
            case Scene.Title:
                //ModeSelect画面に
                if (InputManager.Instance.Any && 
                    !InputManager.Instance.InputB)
                {
                    AudioManager.Instance.PlaySystemSE(0);
                    ChangeToModeSelect();
                }
                break;
            case Scene.ModeSelect:
                //戻る
                if (InputManager.Instance.InputB)
                {
                    AudioManager.Instance.PlaySystemSE(0);
                    ChangeToTitle();
                    break;
                }
                //モード選択
                modeSelect.Select();
                break;
            case Scene.Option:
                //戻る
                if (InputManager.Instance.InputB)
                {
                    AudioManager.Instance.PlaySystemSE(0);
                    timeSetting.LastTimeSetting();
                    ChangeScene(optionScene, modeSelectScene);
                    scene = Scene.ModeSelect;
                }
                //Timeの設定
                timeSetting.Setting();
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

    public void ChangeToOption()
    {
        ChangeScene(modeSelectScene, optionScene);
        scene = Scene.Option;
    }

    private void ChangeScene(RectTransform from, RectTransform to)
    {
        from.gameObject.SetActive(false);
        to.gameObject.SetActive(true);
    }
}
