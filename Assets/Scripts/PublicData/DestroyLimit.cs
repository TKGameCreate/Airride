using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash
{
    public Flash(){}

    public void Flashing(Renderer renderer,  float _interval)
    {
        if (isDesplay(_interval))
        {
            renderer.enabled = true;
        }
        else
        {
            renderer.enabled = false;
        }
    }

    public void Flashing(SpriteRenderer spriteRenderer, Sprite defSprite, float _interval)
    {
        if (isDesplay(_interval))
        {
            spriteRenderer.sprite = defSprite;
        }
        else
        {
            spriteRenderer.sprite = null;
        }
    }

    private bool isDesplay(float _interval)
    {
        float f = 1.0f / _interval;
        float sin = Mathf.Sin(2 * Mathf.PI * f * Time.time);
        if(sin < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
