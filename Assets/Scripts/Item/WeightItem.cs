public class WeightItem : Item
{
    public override void CatchItem(Machine machine)
    {
        statusName = StatusName.Weight;
        base.CatchItem(machine);
    }
}