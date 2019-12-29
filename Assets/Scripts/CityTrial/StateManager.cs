using UnityEngine;
using TMPro;

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
        End,
        Pause
    }

    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private RectTransform pauseDisplayUI;
    [SerializeField] private float time = 180;
    [SerializeField] private StartUIAnimation startUI;

    private GameState pauseBeforeState = GameState.Game;
    private float countDown = 4;
    private int iCountStart = 0;

    private int minute;
    private int second;
    private float milliSecond;
    public static GameState State { get; private set; } = GameState.Start;

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
                Pause();

                //制限時間が0になった場合
                if (time <= 0)
                {
                    timeText.gameObject.SetActive(false);
                    State = GameState.End;
                }
                break;
            case GameState.End:
                Time.timeScale = 0;
                break;
            case GameState.Pause:
                Pause();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ポーズ処理
    /// </summary>
    /// <param name="state">現在のスエート</param>
    private void Pause()
    {
        if (InputManager.Instance.Pause)
        {
            bool pauseUIActive = true;
            if (State == GameState.Pause)
            {
                //pause画面を解除する
                pauseUIActive = false;
                Time.timeScale = 1;
                State = pauseBeforeState;
            }
            else
            {
                //pause画面にする
                pauseUIActive = true;
                Time.timeScale = 0;
                pauseBeforeState = State;
                State = GameState.Pause;
            }
            pauseDisplayUI.gameObject.SetActive(pauseUIActive);
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
