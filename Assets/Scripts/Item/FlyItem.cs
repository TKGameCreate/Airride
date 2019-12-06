public class FlyItem : Item
{
    public override void CatchItem(Machine machine)
    {
        if(mode == ItemMode.Buff)
        {
            //重さを下げる
            //滑空速度を上げる
        }
        else
        {
            //重さを上げる
            //滑空速度を下げる
        }
        base.CatchItem(machine);
    }
}
