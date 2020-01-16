using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpObject : MonoBehaviour
{
    [SerializeField] private float jumpPower = 0;
    public float JumpPower
    {
        get
        {
            return jumpPower;
        }
    }
}
