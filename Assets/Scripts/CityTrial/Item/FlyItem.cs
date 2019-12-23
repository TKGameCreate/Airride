public class FlyItem : Item
{
    private ItemMode reverceWeight = ItemMode.None;

    public override void CatchItem(Machine machine)
    {
        itemName = ItemName.Fly;
        base.CatchItem(machine);
        if (!limit)
        {
            ChangeStatus(machine, mode);
        }
    }

    public override void ChangeStatus(Machine machine, ItemMode itemMode)
    {
        reverceWeight = ReverseBuff();
        machine.ChangeStatus(StatusType.FlySpeed, mode);
        machine.ChangeStatus(StatusType.Weight, reverceWeight);
    }
}