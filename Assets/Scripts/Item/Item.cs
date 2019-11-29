using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected ItemMode mode = ItemMode.Buff;
    protected StatusName statusName = StatusName.Attack;

    /// <summary>
    /// 取得時効果
    /// </summary>
    public virtual void CatchItem(Machine machine)
    {
        switch (mode)
        {
            case ItemMode.Buff:
                machine.ChangeStatus(statusName, 1);
                return;
            case ItemMode.Debuff:
                machine.ChangeStatus(statusName, -1);
                return;
            default:
                return;
        }
    }
}
