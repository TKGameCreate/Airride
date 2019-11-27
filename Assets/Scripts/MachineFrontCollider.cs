using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineFrontCollider : MonoBehaviour
{
    [SerializeField] private Machine machine;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            Item item =collision.gameObject.GetComponent<Item>();
        }
    }
}
