using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineFrontCollider : MonoBehaviour
{
    [SerializeField] private MachineStatus machineStatus;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            Item item =collision.gameObject.GetComponent<Item>();
            item.CatchItem(ref machineStatus);
        }
    }
}
