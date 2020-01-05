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
    public bool CountDown { private set; get; } = false;

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
    }
}
