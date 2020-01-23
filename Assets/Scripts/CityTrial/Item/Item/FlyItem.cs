public class FlyItem : Item
{
    public override void CatchItem(Machine machine)
    {
        itemName = ItemName.Fly;
        base.CatchItem(machine);
        if (!limit)
        {
            ChangeStatus(machine, mode);
        }
    }

    //public override void ChangeStatus(Machine machine, ItemMode itemMode)
    //{
    //    ItemMode reverceWeight = ReverseBuff(itemMode);
    //    machine.ChangeStatus(StatusType.FlySpeed, itemMode);
    //    machine.ChangeStatus(StatusType.Weight, reverceWeight);
    //}
}