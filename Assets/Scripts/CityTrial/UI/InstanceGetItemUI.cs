using UnityEngine;

public class InstanceGetItemUI : MonoBehaviour
{
    [SerializeField] private OverHeadGetItemUI overHeadPrefab;

    public void UpItemImageDisplay(ItemName itemName, Player player)
    {
        var pref = Instantiate(overHeadPrefab) as OverHeadGetItemUI;
        pref.SetSprite(itemName);
        pref.Player = player;
    }
}