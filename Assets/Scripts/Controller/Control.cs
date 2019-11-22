using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    protected float horizontal = 0;
    protected float vertical = 0;

    public virtual void Controller()
    {

    }

    protected virtual void Move()
    {
        horizontal = InputManager.Instance.Horizontal;
        vertical = InputManager.Instance.Vertical;
    }
}
