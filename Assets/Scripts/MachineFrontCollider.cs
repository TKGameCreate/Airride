using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineFrontCollider : MonoBehaviour
{
    [SerializeField] private Machine machine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            Item item = other.gameObject.GetComponent<Item>();
            item.CatchItem(machine); //入手したときの処理
            Destroy(other.gameObject); //触れたアイテムの削除
        }
    }
}
