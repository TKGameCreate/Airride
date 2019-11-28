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
                machine.ChangeStatus(StatusName.Acceleration, 1);
                return;
            case ItemMode.Debuff:
                machine.ChangeStatus(StatusName.Acceleration, -1);
                return;
            default:
                return;
        }
    }
}
