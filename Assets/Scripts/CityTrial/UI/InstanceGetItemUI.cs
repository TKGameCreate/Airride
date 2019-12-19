using UnityEngine;

public class InstanceGetItemUI : MonoBehaviour
{
    [SerializeField] private OverHeadGetItemUI overHeadPrefab;
    [SerializeField] private Canvas canvas;

    public void UpItemImageDisplay(ItemName itemName, Machine machine)
    {
        var pref = Instantiate(overHeadPrefab) as OverHeadGetItemUI;
        pref.transform.SetParent(canvas.transform, false);
        pref.SetSprite(itemName);
        pref.Machine = machine;
    }
}