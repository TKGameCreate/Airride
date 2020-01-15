using System.Collections.Generic;
using UnityEngine;

public class RightGetItemDisplay : MonoBehaviour
{
    public static List<RightGetItemUI> RightGetItemUIs { set; get; } = new List<RightGetItemUI>();

    private void Start()
    {
        //リストの初期化
        RightGetItemUIs.Clear();
    }

    private void Update()
    {
        //リストが空だったら処理しない
        if(!(RightGetItemUIs?.Count > 0))
        {
            return;
        }

        if(!RightGetItemUIs[0].gameObject.activeSelf)
        {
            //リストの先頭を画面上に表示
            RightGetItemUIs[0].gameObject.SetActive(true);
        }
    }
}