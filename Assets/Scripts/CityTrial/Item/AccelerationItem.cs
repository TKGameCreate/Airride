/// <summary>
/// カソクアイテム
/// </summary>
public class AccelerationItem : Item
{
    public override void CatchItem(Machine machine)
    {
        itemName = ItemName.Acceleration;
        base.CatchItem(machine);
        if (!limit)
        {
            ChangeStatus(machine, mode);
        }
    }

    public override void ChangeStatus(Machine machine, ItemMode itemMode)
    {
        machine.ChangeStatus(StatusType.Acceleration, mode);
    }
}