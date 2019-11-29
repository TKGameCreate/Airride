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
        statusName = StatusName.Acceleration;
        base.CatchItem(machine);
    }
}
