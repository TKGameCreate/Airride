using UnityEngine;
using UnityEngine.UI;

public abstract class Control : MonoBehaviour
{
    [SerializeField] protected Rigidbody rbody;
    protected float horizontal = 0;
    protected float vertical = 0;
    protected Vector3 velocity;
    protected bool onGround = true; //接地フラグ

    /// <summary>
    /// Updateで処理するメソッドを呼び出す
    /// </summary>
    public virtual void Controller() { }

    public virtual void FixedController() { }

    protected virtual void Move() { }

    protected virtual void GetOff() { }

    protected void Input()
    {
        horizontal = InputManager.Instance.Horizontal;
        vertical = InputManager.Instance.Vertical;
    }
}