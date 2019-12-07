using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected ItemMode mode = ItemMode.Buff;
    protected ItemName itemName;
    protected bool limit = false; //下限上限に達しているか

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
        limit = machine.ItemCount(itemName, mode);
    }
}