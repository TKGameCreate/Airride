using UnityEngine;

public class WeightItem : Item
{
    private ItemMode reverceMode = ItemMode.None;

    public override void CatchItem(Machine machine)
    {
        itemName = ItemName.Weight;
        base.CatchItem(machine);
        if (!limit)
        {
            ChangeStatus(machine, mode);
        }
    }

    public override void ChangeStatus(Machine machine, ItemMode itemMode)
    {
        reverceMode = ReverseBuff();
        machine.ChangeStatus(StatusType.Weight, mode);
        machine.ChangeStatus(StatusType.Brake, mode);
        machine.ChangeStatus(StatusType.MaxSpeed, mode, 0.1f);
        machine.ChangeStatus(StatusType.FlySpeed, reverceMode);
        machine.ChangeStatus(StatusType.Acceleration, reverceMode);
    }
}