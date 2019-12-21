using UnityEngine;

public abstract class Item : MonoBehaviour
{
    private enum VelocityPattern
    {
        Up,
        Front,
        Back,
        LeftFront,
        LeftBack,
        RightFront,
        RightBack
    }

    #region const
    private const float yPower = 8.0f;
    private const float xzPower = 2.75f;
    private const float destroyTime = 45.0f;
    private readonly float[] intervalTime = { 1, 0.5f, 0.25f };
    private readonly float[] flashingTime = { 30.0f, 35.0f, 40.0f };
    #endregion

    #region SerializeField
    [SerializeField] private Canvas canvas;
    [SerializeField] private OverHeadGetItemUI overHeadPrefab;
    [SerializeField] private Rigidbody rbody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite defSprite;
    [SerializeField] protected ItemMode mode = ItemMode.Buff;
    #endregion

    #region 変数
    private float time;
    private Flash flash = new Flash();
    protected ItemName itemName;
    protected bool limit = false; //下限上限に達しているか
    #endregion

    private void Update()
    {

        PlayerFrontRotation();
    }

    private void PlayerFrontRotation()
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

    /// <param name="num">生成数</param>
    /// <param name="no">生成する番号</param>
    public void InstanceVelocity(int num, int no)
    {
        Vector3 velocity = new Vector3();
        switch (num)
        {
            case 1:
                velocity = VelocityMode(VelocityPattern.Up);
                break;
            case 2:
                if (no == 1)
                {
                    velocity = VelocityMode(VelocityPattern.Front);
                }
                else
                {
                    velocity = VelocityMode(VelocityPattern.Back);
                }
                break;
            case 3:
                if (no == 1)
                {
                    velocity = VelocityMode(VelocityPattern.Front);
                }
                else if (no == 2)
                {
                    velocity = VelocityMode(VelocityPattern.RightBack);
                }
                else
                {
                    velocity = VelocityMode(VelocityPattern.LeftBack);
                }
                break;
            case 4:
                if (no == 1)
                {
                    velocity = VelocityMode(VelocityPattern.RightFront);
                }
                else if (no == 2)
                {
                    velocity = VelocityMode(VelocityPattern.RightBack);
                }
                else if (no == 3)
                {
                    velocity = VelocityMode(VelocityPattern.LeftBack);
                }
                else
                {
                    velocity = VelocityMode(VelocityPattern.LeftFront);
                }
                break;
            default:
                break;
        }
        rbody.velocity = velocity;
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

    /*
x 1→前　-1→後ろ
z 1→右　-1→左
*/
    private Vector3 VelocityMode(VelocityPattern velocityPattern)
    {
        switch (velocityPattern)
        {
            case VelocityPattern.Up:
                return Vector3.up * yPower;
            case VelocityPattern.Front:
                return new Vector3(xzPower, yPower, 0);
            case VelocityPattern.Back:
                return new Vector3(-xzPower, yPower, 0);
            case VelocityPattern.LeftFront:
                return new Vector3(xzPower, yPower, xzPower);
            case VelocityPattern.LeftBack:
                return new Vector3(-xzPower, yPower, xzPower);
            case VelocityPattern.RightFront:
                return new Vector3(xzPower, yPower, -xzPower);
            case VelocityPattern.RightBack:
                return new Vector3(-xzPower, yPower, -xzPower);
            default:
                return Vector3.zero;
        }
    }

    private void FlashAndDestroy()
    {
        time += Time.deltaTime;
        if (time > destroyTime)
        {
            Destroy(gameObject);
        }
        else if (time > flashingTime[2])
        {
            flash.Flashing(spriteRenderer, defSprite, intervalTime[2]);
        }
        else if (time > flashingTime[1])
        {
            flash.Flashing(spriteRenderer, defSprite, intervalTime[1]);
        }
        else if (time > flashingTime[0])
        {
            flash.Flashing(spriteRenderer, defSprite, intervalTime[0]);
        }
    }
}