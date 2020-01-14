using UnityEngine;

public class ItemList : MonoBehaviour
{
    [SerializeField] private Item[] itemPrefabs = { };
    public int ItemLength
    {
        get
        {
            return itemPrefabs.Length;
        }
    }

    public void ChangeStatusDropItem(int itemNo, Machine machine)
    {
        itemPrefabs[itemNo].ChangeStatus(machine, ItemMode.Debuff);
    }

    public Item GetItem(int itemNo)
    {
        return itemPrefabs[itemNo];
    }

    public void InstantiateItem(Vector3 instancePosition, int itemNo, int geneNum, int insNo)
    {
        var item = Instantiate(itemPrefabs[itemNo], instancePosition, Quaternion.identity) as Item;
        item.InstanceVelocity(geneNum, insNo);
    }
}