using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxSpeedItem : Item
{
    public override void CatchItem(Machine machine)
    {
        itemName = ItemName.MaxSpeed;
        base.CatchItem(machine);
        if (!limit)
        {
            ChangeStatus(machine, mode);
        }
    }

    //public override void ChangeStatus(Machine machine, ItemMode itemMode)
    //{
    //    machine.ChangeStatus(StatusType.MaxSpeed, itemMode, 0.9f);
    //    machine.ChangeStatus(StatusType.FlySpeed, itemMode);
    //    machine.ChangeStatus(StatusType.Brake, itemMode);
    //}
}