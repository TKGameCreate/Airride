public class FlyItem : Item
{
    private ItemMode reverceWeight = ItemMode.None;

    public override void CatchItem(Machine machine)
    {
        itemName = ItemName.Fly;
        reverceWeight = ReverseBuff();
        base.CatchItem(machine);
        if (!limit)
        {
            machine.ChangeStatus(StatusType.FlySpeed, mode);
            machine.ChangeStatus(StatusType.Weight, reverceWeight);
        }
    }
}