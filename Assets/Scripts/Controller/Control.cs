using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    [SerializeField] protected Rigidbody rbody;
    protected float horizontal = 0;
    protected float vertical = 0;
    protected Vector3 velocity;

    /// <summary>
    /// Updateで処理するメソッドを呼び出す
    /// </summary>
    public virtual void Controller()
    {
        Debug.Log("overrideしてください。");
    }

    protected virtual void Move()
    {
        horizontal = InputManager.Instance.Horizontal;
        vertical = InputManager.Instance.Vertical;
    }
}