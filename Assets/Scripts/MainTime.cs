using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 時間の制御
/// </summary>
public class MainTime : MonoBehaviour
{
    private enum GameState
    {
        Ready,
        Game,
        End
    }

    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject countDownTextObject;
    [SerializeField] private GameObject timeTextObject;
    [SerializeField] private float time = 180;
    private float countDown = 3;

    private GameState state = GameState.Ready;

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GameState.Ready:
                countDown -= Time.deltaTime;
                countDownText.text = countDown.ToString("0");
                if (countDown <= 0)
                {
                    countDownTextObject.SetActive(false);
                    timeTextObject.SetActive(true);
                    state = GameState.Game;
                }
                break;
            case GameState.Game:
                time -= Time.deltaTime;
                timeText.text = time.ToString();
                if(time <= 0)
                {
                    timeTextObject.SetActive(false);
                    state = GameState.End;
                }
                break;
            case GameState.End:
                break;
            default:
                break;
        }
    }
}
