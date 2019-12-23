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