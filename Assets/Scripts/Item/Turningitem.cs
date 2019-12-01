using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turningitem : Item
{
    public override void CatchItem(Machine machine)
    {
        statusName = StatusName.Turning;
        base.CatchItem(machine);
    }
}
