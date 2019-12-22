using System.Collections;
using UnityEngine;

public class GetItemUI : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected RectTransform rectTransform;
    [SerializeField] protected float displayTime;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        DestroyCoroutine();
    }

    protected virtual void Update() { }

    protected void DestroyCoroutine()
    {
        StartCoroutine(DestroyMeasure());
    }

    /// <summary>
    /// 表示からオブジェクトを消すまでの測定
    /// </summary>
    private IEnumerator DestroyMeasure()
    {
        float measureTime = 0f;

        while (measureTime < displayTime)
        {
            measureTime += Time.deltaTime;
            yield return null;
        }

        anim.SetTrigger("Finish");
    }

    public virtual void FinishDestroy() { }
}
