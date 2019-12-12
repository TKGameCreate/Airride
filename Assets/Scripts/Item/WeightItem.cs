using UnityEngine;

public class WeightItem : Item
{
    private ItemMode reverceMode = ItemMode.None;

    public override void CatchItem(Machine machine)
    {
        itemName = ItemName.Weight;
        reverceMode = ReverseBuff();
        base.CatchItem(machine);
        if (!limit)
        {
            machine.ChangeStatus(StatusType.Weight, mode);
            machine.ChangeStatus(StatusType.Brake, mode);
            machine.ChangeStatus(StatusType.MaxSpeed, mode, 0.1f);
            machine.ChangeStatus(StatusType.FlySpeed, reverceMode);
            machine.ChangeStatus(StatusType.Acceleration, reverceMode);
        }
    }
}