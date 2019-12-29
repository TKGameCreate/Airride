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
        End
    }

    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private float time = 180;
    [SerializeField] private StartUIAnimation startUI;
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

                //制限時間が0になった場合
                if (time <= 0)
                {
                    timeText.gameObject.SetActive(false);
                    State = GameState.End;
                }
                break;
            case GameState.End:
                Time.timeScale = 0;
                //State = GameState.Start;
                break;
            default:
                break;
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
