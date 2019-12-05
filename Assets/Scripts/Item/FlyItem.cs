using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyItem : Item
{
    public override void CatchItem(Machine machine)
    {
        //滑空スピードを上げる
        statusName = StatusName.FlySpeed;
        //重さを下げる
        machine.ChangeStatus(StatusName.Weight, ItemMode.Debuff);
        base.CatchItem(machine);
    }
}
