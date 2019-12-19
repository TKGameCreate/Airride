using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OverHeadGetItemUI : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] itemImages; //自機の上に表示するアイテムの画像
    [SerializeField] private float displayTime;
    [SerializeField] private float yPos;
    public Machine Machine { set; private get; } //アイテムを取得したプレイヤーの座標

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyMeasure());
    }

    // Update is called once per frame
    void Update()
    {
        if(Machine.Player != null)
        {
            Vector3 pos = Machine.transform.position;
            pos.y += yPos;

            rectTransform.position = RectTransformUtility.WorldToScreenPoint
                (Camera.main,
                pos);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 表示する画像のセット
    /// </summary>
    /// <param name="itemName">アイテム</param>
    public void SetSprite(ItemName itemName)
    {
        image.sprite = itemImages[(int)itemName];
    }

    /// <summary>
    /// 表示からオブジェクトを消すまでの測定
    /// </summary>
    private IEnumerator DestroyMeasure()
    {
        float measureTime = 0f;

        while(measureTime < displayTime)
        {
            measureTime += Time.deltaTime;
            yield return null;
        }

        Destroy(transform.root.gameObject); //Canvasを削除
        Destroy(gameObject); //自分自身を削除
    }
}
