using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObjectInstance : MonoBehaviour
{
    [SerializeField] private Transform parentInstance; //生成ポジション(親)
    #region 生成オブジェクト
    [SerializeField] private Item[] item;
    [SerializeField] private ItemBox itemBox;
    #endregion
    [SerializeField] private int maxInstance; //最大同時生成数
    [SerializeField] private int simInstance; //一度に生成する量(最大値)
    [SerializeField] private float interval; //生成間隔
    //生成可能ポジションリスト
    private List<Transform> positions = new List<Transform>();

    /// <summary>
    /// オブジェクトがDestroyされた際に呼び出す
    /// </summary>
    /// <param name="position">ポジション</param>
    public void AddPosition(Transform position)
    {
        if (!positions.Contains(position))
        {
            positions.Add(position);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //ポジションリストの作成
        foreach (Transform pos in parentInstance)
        {
            positions.Add(pos);
        }
        //初期生成
        Instance(maxInstance);
        StartCoroutine(InstanceRegular());
    }

    private void Instance(int max)
    {
        for (int i = 0; i < max; i++)
        {
            if(positions?.Count < 1)
            {
                return;
            }
            
            int instancePositionNum = Random.Range(0, positions.Count);
            int instanceType = Random.Range(0, 2);
            Transform pos = positions[instancePositionNum];
            if (instanceType == 0)
            {
                ItemInstance(pos);
            }
            else
            {
                ItemBoxInstance(pos);
            }
            DeletePosition(instancePositionNum);
        }
    }

    /// <summary>
    /// 生成時に呼び出す
    /// </summary>
    /// <param name="id">生成ID</param>
    private void DeletePosition(int id)
    {
        positions.RemoveAt(id);
    }

    private void ItemInstance(Transform pos)
    {
        int itemType = Random.Range(0, item.Length);
        //アイテムの生成
        Item insItem = Instantiate(item[itemType], pos.position, Quaternion.identity) as Item;
        insItem.InstancePosition = pos; //生成ポジションの割り当て
        insItem.objInstance = this;
    }

    private void ItemBoxInstance(Transform pos)
    {
        //アイテムボックスの生成
        ItemBox insBox = Instantiate(itemBox, pos.position, Quaternion.identity) as ItemBox;
        insBox.InstancePosition = pos;
        insBox.objInstance = this;
    }

    private IEnumerator InstanceRegular()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Instance(simInstance);
        }
    }
}