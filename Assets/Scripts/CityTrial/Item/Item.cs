using UnityEngine;

public abstract class Item : MonoBehaviour
{
    #region const
    private const float yItemUpPower = 250.0f;
    private const float xzItemUpPower = 250.0f;
    #endregion

    #region SerializeField
    [SerializeField] private Canvas canvas;
    [SerializeField] private OverHeadGetItemUI overHeadPrefab;
    [SerializeField] private Rigidbody rbody;
    [SerializeField] protected ItemMode mode = ItemMode.Buff;
    #endregion

    #region 変数
    protected ItemName itemName;
    protected bool limit = false; //下限上限に達しているか
    #endregion

    private void Update()
    {
        //カメラの方向に回転（常に正面を向くように）
        Quaternion lockRotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position, Vector3.up);

        lockRotation.z = 0;
        lockRotation.x = 0;

        transform.rotation = Quaternion.Lerp(transform.rotation, lockRotation, 0.1f);
    }

    /// <summary>
    /// 取得時効果
    /// </summary>
    public virtual void CatchItem(Machine machine)
    {
        Destroy(gameObject);
        UpItemImageDisplay(machine);
        limit = machine.ItemCount(itemName, mode);
    }

    public void UpAddForce(int num, int no)
    {
        //1→上
        //2→前後
        //3→三角
        //4→四角
        rbody.AddForce(new Vector3(xzItemUpPower,
            yItemUpPower,
            xzItemUpPower));
    }

    public void UpItemImageDisplay(Machine machine)
    {
        var pref = Instantiate(overHeadPrefab) as OverHeadGetItemUI;
        var canvasPref = Instantiate(canvas) as Canvas;
        pref.transform.SetParent(canvasPref.transform, false);
        pref.SetSprite(itemName);
        pref.Machine = machine;
    }

    protected ItemMode ReverseBuff()
    {
        if (mode == ItemMode.Buff)
        {
            return ItemMode.Debuff;
        }
        else if(mode == ItemMode.Debuff)
        {
            return ItemMode.Buff;
        }
        else
        {
            return ItemMode.None;
        }
    }
}