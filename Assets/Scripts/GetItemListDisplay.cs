using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GetItemListDisplay : MonoBehaviour
{
    [SerializeField] protected Player player;
    [SerializeField] protected TextMeshProUGUI[] numText;
    [SerializeField] protected Image[] numGage;

    public virtual void SetDisplay()
    {
        if(player.Machine == null)
        {
            return;
        }

        //画像のセット
        Machine machine = player.LastRideMachine;
        for (int i = 0; i < numText.Length; i++)
        {
            numText[i].text = machine.GetItemTotal(i).ToString();
            numGage[i].fillAmount = machine.NormalizeGetItemTotal(i);
        }
    }
}