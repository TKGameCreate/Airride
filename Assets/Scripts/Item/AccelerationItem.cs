using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カソクアイテム
/// </summary>
public class AccelerationItem : Item
{
    public override void CatchItem(ref MachineStatus status)
    {
        switch (mode)
        {
            case ItemMode.Buff:
                status.Acceleration++;
                return;
            case ItemMode.Debuff:
                status.Acceleration--;
                return;
            default:
                return;
        }
    }
}
