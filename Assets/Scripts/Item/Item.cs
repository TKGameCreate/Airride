using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected ItemMode mode = ItemMode.Buff;
    protected StatusName statusName = StatusName.Attack;

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
    }

    protected void ChangeStatus(Machine machine, float upNum = 0)
    {
        switch (mode)
        {
            case ItemMode.Buff:
                machine.ChangeStatus(statusName, ItemMode.Buff);
                break;
            case ItemMode.Debuff:
                machine.ChangeStatus(statusName, ItemMode.Debuff);
                break;
            case ItemMode.None:
                machine.ChangeStatus(statusName, ItemMode.None, upNum);
                break;
            default:
                break;
        }
    }
}
