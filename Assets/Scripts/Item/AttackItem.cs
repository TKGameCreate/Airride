using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackItem : Item
{
    public override void CatchItem(Machine machine)
    {
        statusName = StatusName.Attack;
        base.CatchItem(machine);
    }
}
