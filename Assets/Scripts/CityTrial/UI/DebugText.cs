using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DebugText : MonoBehaviour
{
    public enum Position : int
    {
        Left = 0,
        Right = 1
    }

    [SerializeField] private GameObject debugTextObject; //Machineに関するテキストを表示するテキスト群
    [SerializeField] private List<TextMeshProUGUI> debug = new List<TextMeshProUGUI>(); //debugテキスト

    public void Debug(Position position, string debugText, Player player)
    {
        if (player == null)
        {
            debugTextObject.SetActive(false);
            return;
        }
        else
        {
            debugTextObject.SetActive(true);
        }

        debug[(int)position].text = debugText;
    }
}
