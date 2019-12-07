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
            machine.ChangeStatus(StatusType.Acceleration, mode);
        }
    }
}