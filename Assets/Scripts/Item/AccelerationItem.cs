using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カソクアイテム
/// </summary>
public class AccelerationItem : Item
{
    public override void CatchItem()
    {
        switch (mode)
        {
            case ItemMode.Buff:
                return;
            case ItemMode.Debuff:
                return;
            default:
                return;
        }
    }
}
