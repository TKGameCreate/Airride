public class ChargeItem : Item
{
    public override void CatchItem(Machine machine)
    {
        itemName = ItemName.Charge;
        base.CatchItem(machine);
        if (!limit)
        {
            ChangeStatus(machine, mode);
        }
    }

    //public override void ChangeStatus(Machine machine, ItemMode itemMode)
    //{
    //    machine.ChangeStatus(itemName, itemMode);
    //}
}
