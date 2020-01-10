using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMachineColider : MonoBehaviour
{
    [SerializeField] private Machine machine;

    private void OnTriggerStay(Collider other)
    {
        machine.OnGround(other);
    }

    private void OnTriggerExit(Collider other)
    {
        machine.ExitGround(other);
    }
}
