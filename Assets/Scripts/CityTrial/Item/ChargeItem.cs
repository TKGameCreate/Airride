public class ChargeItem : Item
{
    public override void CatchItem(Machine machine)
    {
        itemName = ItemName.Charge;
        base.CatchItem(machine);
        if (!limit)
        {
            machine.ChangeStatus(StatusType.Charge, mode);
            machine.ChangeStatus(StatusType.ChargeSpeed, mode);
        }
    }
}
