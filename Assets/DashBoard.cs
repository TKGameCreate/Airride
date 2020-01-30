using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBoard : MonoBehaviour
{
    private const float dashBoardUpSpeed = 125.0f; //ダッシュボード倍率
    private bool coolDown = false;

    public float Dash()
    {
        if (!coolDown)
        {
            StartCoroutine(DashCoolDown());
            return dashBoardUpSpeed;
        }
        return 0;
    }

    private IEnumerator DashCoolDown()
    {
        coolDown = true;
        float interval = 10.0f;
        yield return new WaitForSeconds(interval);
        coolDown = false;
    }
}
