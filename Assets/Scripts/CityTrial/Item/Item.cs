using UnityEngine;

public abstract class Item : MonoBehaviour
{
    #region const
    private const float yItemUpPower = 1000.0f;
    private const float xzItemUpPower = 1000.0f;
    #endregion

    #region SerializeField
    [SerializeField] private Rigidbody rbody;
    [SerializeField] protected ItemMode mode = ItemMode.Buff;
    #endregion

    #region 変数
    protected ItemName itemName;
    protected bool limit = false; //下限上限に達しているか
    #endregion

    #region Property
    public InstanceGetItemUI InstanceOverHeadUI { set; private get; }
    public Machine Machine { set; private get; }
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
        InstanceOverHeadUI.UpItemImageDisplay(itemName, Machine);
        limit = machine.ItemCount(itemName, mode);
    }

    public void UpAddForce()
    {
        float forceX = Random.Range(-1.0f, 1.0f);
        float forceZ = Random.Range(-1.0f, 1.0f);
        rbody.AddForce(new Vector3(forceX * xzItemUpPower,
            yItemUpPower,
            forceZ * xzItemUpPower));
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