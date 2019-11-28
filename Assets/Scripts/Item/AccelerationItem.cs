using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カソクアイテム
/// </summary>
public class AccelerationItem : Item
{
    public override void CatchItem(Machine machine)
    {
        switch (mode)
        {
            case ItemMode.Buff:
                Debug.Log("in");
                machine.ChangeStatus(StatusName.Acceleration, 5);
                return;
            case ItemMode.Debuff:
                machine.ChangeStatus(StatusName.Acceleration, -1);
                return;
            default:
                return;
        }
    }
}
