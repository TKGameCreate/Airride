using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundCollider : MonoBehaviour
{
    [SerializeField] private Human human;
    private void OnTriggerEnter(Collider other)
    {
        human.AnimationControl(Human.AnimationType.OnGround);
    }
}
