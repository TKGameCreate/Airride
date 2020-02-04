using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartUIAnimation : MonoBehaviour
{
    [SerializeField] private SpeedMater speedMater = default;
    [SerializeField] private TextMeshProUGUI timeText = default;
    [SerializeField] private Image[] rightGage = default;
    [SerializeField] private Image[] leftGage = default;
    [SerializeField] private Animator anim = default;
    [SerializeField] private AudioClip countDownSE = default;
    [SerializeField] private AudioClip startSE = default;
    private int nowCount = 0;
    public bool CountDown { private set; get; } = false;

    /// <summary>
    /// TimeLineでActiveをTrueにしたタイミングで呼び出し
    /// </summary>
    public void CountDownStart()
    {
        speedMater.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
        anim.SetTrigger("rotation");
        CountDown = true;
    }

    public void SetCountDownAnimation(int no)
    {
        rightGage[no].gameObject.SetActive(true);
        leftGage[no].gameObject.SetActive(true);
        anim.SetInteger("gageNo", no);
        PlaySE(no);
    }

    public void PlaySE(int count)
    {
        if(nowCount != count)
        {
            nowCount = count;
            if (nowCount <= 0)
            {
                AudioManager.Instance.PlaySE(startSE);
            }
            else
            {
                AudioManager.Instance.PlaySE(countDownSE);
            }
        }
    }
}