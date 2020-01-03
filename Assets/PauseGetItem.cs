public class PauseGetItem : GetItemListDisplay
{
    public override void SetDisplay()
    {
        if(StateManager.State != StateManager.GameState.Pause)
        {
            return;
        }
        base.SetDisplay();
    }
}
