using UnityEngine;

public class WeightItem : Item
{

    public override void CatchItem(Machine machine)
    {
        itemName = ItemName.Weight;
        base.CatchItem(machine);
        if (!limit)
        {
            ChangeStatus(machine, mode);
        }
    }

    //public override void ChangeStatus(Machine machine, ItemMode itemMode)
    //{
    //    ItemMode reverceMode = ReverseBuff(itemMode);
    //    machine.ChangeStatus(StatusType.Weight, itemMode);
    //    machine.ChangeStatus(StatusType.Brake, itemMode);
    //    machine.ChangeStatus(StatusType.MaxSpeed, itemMode, 0.1f);
    //    machine.ChangeStatus(StatusType.FlySpeed, reverceMode);
    //    machine.ChangeStatus(StatusType.Acceleration, reverceMode, 0.3f);
    //}
}