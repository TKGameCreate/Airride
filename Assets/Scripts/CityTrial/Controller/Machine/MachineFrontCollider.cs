using UnityEngine;

public class MachineFrontCollider : MonoBehaviour
{
    private const float bound = 500.0f;
    [SerializeField] private Machine machine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item" && machine.Player != null)
        {
            Item item = other.gameObject.GetComponent<Item>();
            item.CatchItem(machine); //入手したときの処理
        }

        if (other.gameObject.tag == "StageObject")
        {
            machine.Bound(bound, false);
        }
    }
}
