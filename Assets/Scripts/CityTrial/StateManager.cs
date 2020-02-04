using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// 時間の制御
/// </summary>
public class StateManager : MonoBehaviour
{
    public enum GameState
    {
        Start,
        Ready,
        Game,
        TimeUp,
        Result,
        End,
        Pause
    }

    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private RectTransform pauseDisplayUI;
    [SerializeField] private StartUIAnimation startUI;
    [SerializeField] private Pause pause;
    [SerializeField] private Result result;
    [SerializeField] private Animator timeUp;

    private GameState pauseBeforeState = GameState.Game;
    private float countDown = 4;
    private int iCountStart = 0;
    private bool pauseUIActive = false;

    private int minute;
    private int second;
    private float milliSecond;
    public static GameState State { get; private set; } = GameState.Start;
    public static float time = 180;

    private void Start()
    {
        State = GameState.Start;
        iCountStart = (int)countDown;
        DisplayTime();
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case GameState.Start:
                time = TimeSetting.LastSetting;
                DisplayTime();
                if (startUI.CountDown)
                {
                    countDownText.gameObject.SetActive(true);
                    State = GameState.Ready;
                }
                break;
            case GameState.Ready:
                //カウントダウンを開始
                countDown -= Time.deltaTime * 2;
                int iCountDown = (int)countDown;

               countDownText.text = iCountDown.ToString();

                startUI.SetCountDownAnimation(iCountDown);

                //カウントが0になった場合
                if (iCountDown <= 0)
                {
                    countDownText.text = "Start";
                    State = GameState.Game;
                }
                break;
            case GameState.Game:
                //制限時間の計算と時間表示の計算
                time -= Time.deltaTime;
                DisplayTime();

                //Pauseボタンを押したときの処理
                if (InputManager.Instance.Pause)
                {
                    ChangePause();
                }

                //制限時間が0になった場合
                if (time <= 0)
                {
                    timeText.gameObject.SetActive(false);
                    State = GameState.TimeUp;
                    StartCoroutine(ChangeResult());
                    time = TimeSetting.LastSetting;
                }
                break;
            case GameState.TimeUp:
                Time.timeScale = 0;
                timeUp.gameObject.SetActive(true);
                break;
            case GameState.Result:
                result.gameObject.SetActive(true);
                result.DisplayResult();
                State = GameState.End;
                break;
            case GameState.End:
                if (InputManager.Instance.InputA(InputType.Down))
                {
                    AirrideSceneManager.Instance.LoadScene(AirrideScene.Title);
                }
                break;
            case GameState.Pause:
                PauseMove();
                pause.PauseDisplay();
                break;
            default:
                break;
        }
    }

    private IEnumerator ChangeResult()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        State = GameState.Result;
    }

    private void SelectContinue()
    {
        //pause画面を解除する
        pauseUIActive = false;
        Time.timeScale = 1;
        pause.EndPause();
        AudioManager.Instance.PlaySystemSE(0);
        AudioManager.Instance.UnPauseBGM();
        State = pauseBeforeState;
        pauseDisplayUI.gameObject.SetActive(pauseUIActive);
    }

    private void ChangePause()
    {
        //pause画面にする
        pauseUIActive = true;
        Time.timeScale = 0;
        pause.ResetPause();
        pauseBeforeState = State;
        State = GameState.Pause;
        AudioManager.Instance.PauseBGM();
        pauseDisplayUI.gameObject.SetActive(pauseUIActive);
    }

    /// <summary>
    /// ポーズ処理
    /// </summary>
    /// <param name="state">現在のステート</param>
    private void PauseMove()
    {
        if (InputManager.Instance.Pause)
        {
            SelectContinue();
        }
        //Aボタンかポーズボタンを押したときの処理
        if (InputManager.Instance.InputA(InputType.Down))
        {
            Pause.Mode mode = pause.SelectMode();
            if (mode == Pause.Mode.Continue)
            {
                SelectContinue();
            }
            else
            {
                AirrideSceneManager.Instance.LoadScene(AirrideScene.Title);
            }
        }
    }

    private void DisplayTime()
    {
        TimeConversion();
        //制限時間の画面表示
        timeText.text = minute.ToString("00")
            + "'"
            + second.ToString("00")
            + "\""
            + milliSecond.ToString("00");
    }

    private void TimeConversion()
    {
        minute = (int)time / 60;

        if (minute > 0)
        {
            second = (int)time % 60;
        }
        else
        {
            second = (int)time;
        }

        milliSecond = time % 1;
        milliSecond *= 100;

        if (milliSecond > 99)
        {
            milliSecond = 99;
        }
    }
}