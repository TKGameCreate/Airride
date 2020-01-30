using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpObject : MonoBehaviour
{
    [SerializeField] private float jumpPowerY = 0;
    [SerializeField] private float jumpPowerZ = 0;

    public void Jump(Rigidbody rb)
    {
        rb.AddForce(0, jumpPowerY, jumpPowerZ);
    }
}
