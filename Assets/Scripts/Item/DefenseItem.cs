using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseItem : Item
{
    public override void CatchItem(Machine machine)
    {
        statusName = StatusName.Defence;
        base.CatchItem(machine);
    }
}
