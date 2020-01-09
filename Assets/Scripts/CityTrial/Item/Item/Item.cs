using System.Collections;
using System.Collections.Generic;
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
    private const float destroyTime = 60.0f;
    private static readonly FlashTime[] flashTimeList =
    {
        new FlashTime(40.0f, 1.0f),
        new FlashTime(47.5f, 0.5f),
        new FlashTime(53.0f, 0.25f)
    };
    #endregion

    #region SerializeField
    [SerializeField] private Canvas canvas;
    [SerializeField] private OverHeadGetItemUI overHeadPrefab;
    [SerializeField] private RightGetItemUI rightGetItemUIPrefab;
    [SerializeField] private Rigidbody rbody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite defSprite;
    [SerializeField] protected ItemMode mode = ItemMode.Buff;
    #endregion

    #region 変数
    private Flash flash = new Flash(flashTimeList);
    private bool destroySet = false;
    protected ItemName itemName;
    protected bool limit = false; //下限上限に達しているか
    #endregion

    public Transform InstancePosition { set; private get; }
    public StageObjectInstance objInstance { set; private get; }

    private void Update()
    {
        if (StateManager.State == StateManager.GameState.Game && !destroySet)
        {
            StartCoroutine(LimitDestroy());
            destroySet = true;
        }
        flash.FlashTime(spriteRenderer);
        PlayerFrontRotation();
    }

    private void PlayerFrontRotation()
    {
        if (StateManager.State != StateManager.GameState.Game)
        {
            return;
        }
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
        DestroyObject();
        UpItemImageDisplay(machine);
        UpItemNameDisplay();
        limit = machine.ItemCount(itemName, mode);
    }

    public virtual void ChangeStatus(Machine machine, ItemMode itemMode){ }

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

    public void UpItemNameDisplay()
    {
        var pref = Instantiate(rightGetItemUIPrefab) as RightGetItemUI;
        var canvasPref = Instantiate(canvas) as Canvas;
        pref.transform.SetParent(canvasPref.transform, false);
    }

    protected ItemMode ReverseBuff(ItemMode itemMode)
    {
        if (itemMode == ItemMode.Buff)
        {
            return ItemMode.Debuff;
        }
        else if(itemMode == ItemMode.Debuff)
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

    IEnumerator LimitDestroy()
    {
        yield return new WaitForSeconds(destroyTime);
        DestroyObject();
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
        if(objInstance != null)
        {
            objInstance.AddPosition(InstancePosition);
        }
    }
}