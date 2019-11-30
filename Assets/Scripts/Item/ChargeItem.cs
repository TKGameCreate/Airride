using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeItem : Item
{
    public override void CatchItem(Machine machine)
    {
        statusName = StatusName.MaxCharge;
        base.CatchItem(machine);
    }
}
