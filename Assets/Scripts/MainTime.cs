using UnityEngine;
using TMPro;

/// <summary>
/// 時間の制御
/// </summary>
public class MainTime : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject countDownTextObject;
    [SerializeField] private GameObject timeTextObject;
    [SerializeField] private float time = 180;
    private float countDown = 3;

    private int minute;
    private int second;
    private float milliSecond;

    private GameState state = GameState.Ready;
    public GameState State
    {
        get
        {
            return state;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GameState.Ready:
                //操作を無効
                //カウントダウンを開始
                player.enabled = false;
                countDown -= Time.deltaTime;
                countDownText.text = countDown.ToString("0");

                //カウントが0になった場合
                if (countDown <= 0)
                {
                    player.enabled = true;
                    countDownTextObject.SetActive(false);
                    timeTextObject.SetActive(true);
                    state = GameState.Game;
                }
                break;
            case GameState.Game:
                //制限時間の計算と時間表示の計算
                time -= Time.deltaTime;
                TimeConversion();

                //制限時間の画面表示
                timeText.text = minute.ToString("00") 
                    + ":"
                    + second.ToString("00")
                    + ":"
                    + milliSecond.ToString("00");

                //制限時間が0になった場合
                if (time <= 0)
                {
                    timeTextObject.SetActive(false);
                    state = GameState.End;
                }
                break;
            case GameState.End:
                Time.timeScale = 0;
                break;
            default:
                break;
        }
    }

    void TimeConversion()
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
