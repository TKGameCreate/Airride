using System.Collections.Generic;
using UnityEngine;

public class RightGetItemDisplay : MonoBehaviour
{
    public static List<RightGetItemUI> RightGetItemUIs { set; get; } = new List<RightGetItemUI>();

    private void Start()
    {
        RightGetItemUIs.Clear();
    }

    private void Update()
    {
        if(!(RightGetItemUIs?.Count > 0))
        {
            return;
        }

        if(!RightGetItemUIs[0].gameObject.activeSelf)
        {
            RightGetItemUIs[0].gameObject.SetActive(true);
        }
    }
}