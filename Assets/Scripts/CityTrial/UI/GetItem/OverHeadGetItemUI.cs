﻿using UnityEngine;
using UnityEngine.UI;

public class OverHeadGetItemUI : GetItemUI
{
    [SerializeField] private Image image;
    [SerializeField] private float yPos;
    public Machine Machine { set; private get; } //アイテムを取得したプレイヤーの座標

    // Update is called once per frame
    protected override void LateUpdate()
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
    public void SetSprite(ItemName itemName, Sprite itemSprite)
    {
        image.sprite = itemSprite;
    }

    public override void FinishDestroy()
    {
        Destroy(transform.root.gameObject); //Canvasを削除
        Destroy(gameObject); //自分自身を削除
    }
}
