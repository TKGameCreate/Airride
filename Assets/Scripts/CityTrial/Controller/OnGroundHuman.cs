using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundHuman : MonoBehaviour
{
    [SerializeField] private Human human;
    private void OnTriggerStay(Collider other)
    {
         human.OnGround = true;
        human.AnimationControl(Human.AnimationType.OnGround);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "StageObject" || other.transform.tag == "NotBackSObject")
        {
            human.OnGround = false;
        }
    }
}
