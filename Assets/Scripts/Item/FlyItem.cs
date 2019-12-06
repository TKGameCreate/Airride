public class FlyItem : Item
{
    public override void CatchItem(Machine machine)
    {
        if(mode == ItemMode.Buff)
        {
            //重さを下げる
            machine.ChangeStatus(StatusName.Weight, ItemMode.Debuff);
            //滑空速度を上げる
            machine.ChangeStatus(StatusName.FlySpeed, ItemMode.Buff);
        }
        else
        {
            //重さを上げる
            machine.ChangeStatus(StatusName.Weight, ItemMode.Buff);
            //滑空速度を下げる
            machine.ChangeStatus(StatusName.FlySpeed, ItemMode.Debuff);
        }
        base.CatchItem(machine);
    }
}
