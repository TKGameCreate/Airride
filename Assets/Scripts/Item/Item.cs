using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected ItemMode mode = ItemMode.Buff;

    /// <summary>
    /// 取得時効果
    /// </summary>
    public virtual void CatchItem(Machine machine)
    {
        Debug.Log("override");
    }
}
