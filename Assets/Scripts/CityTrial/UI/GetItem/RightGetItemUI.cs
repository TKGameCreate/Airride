public class RightGetItemUI : GetItemUI
{
    private bool startCol = false;

    protected override void Start()
    {
        gameObject.SetActive(false);
        RightGetItemDisplay.RightGetItemUIs.Add(this);
    }

    protected override void Update()
    {
        if (!startCol)
        {
            //リストのサイズが多い場合（アイテムを多く取得していた場合）、
            //そのリストサイズに合わせて表示時間を減らす
            displayTime /= RightGetItemDisplay.RightGetItemUIs.Count;
            DestroyCoroutine();
            anim.SetTrigger("Start");
            startCol = true;
        }
    }

    public override void FinishDestroy()
    {
        Destroy(transform.root.gameObject); //Canvasを削除
        Destroy(gameObject); //自分自身を削除
        RightGetItemDisplay.RightGetItemUIs.RemoveAt(0);
    }
}