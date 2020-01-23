using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turningitem : Item
{
    public override void CatchItem(Machine machine)
    {
        itemName = ItemName.Turning;
        base.CatchItem(machine);
        if (!limit)
        {
            ChangeStatus(machine, mode);
        }
    }

    //public override void ChangeStatus(Machine machine, ItemMode itemMode)
    //{
    //    machine.ChangeStatus(StatusType.Turning, itemMode);
    //}
}