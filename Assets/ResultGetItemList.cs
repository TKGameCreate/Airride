using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultGetItemList : GetItemListDisplay
{
    public override void SetDisplay()
    {
        //画像のセット
        Machine machine = player.LastRideMachine;
        for (int i = 0; i < numText.Length; i++)
        {
            numText[i].text = machine.GetItemTotal(i).ToString();
            numGage[i].fillAmount = machine.NormalizeGetItemTotal(i);
        }
    }
}
