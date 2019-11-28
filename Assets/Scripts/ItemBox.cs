using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    [SerializeField] private Renderer myRenderer;
    [SerializeField] private List<Material> materialList = new List<Material>(); //セットするマテリアルのリスト
    [Range(1.1f, 5.0f)][SerializeField] private float firstMaterialDiv; //2段階目マテリアルを判定する際に割る数
    [Range(1.1f, 5.0f)][SerializeField] private float secondMaterialDiv; //３段階目マテリアルを判定する際に割る数
    [SerializeField] private float defHP = 30; //初期HP
    private float hitPoint = 30; //現在のHP

    private void Start()
    {
        hitPoint = defHP;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MachineFront")
        {
            Machine machine = other.transform.parent.gameObject.GetComponent<Machine>();
            hitPoint -= machine.Status.Attack * 2;
            SetTexture();
        }
    }

    void SetTexture()
    {
        Debug.Log("ItemBox HP : " + hitPoint);
        if (hitPoint < 1)
        {
            Destroy(gameObject);
        }
        else if (hitPoint < defHP / firstMaterialDiv && hitPoint > defHP / secondMaterialDiv)
        {
            myRenderer.material = materialList[1];
        }
        else if (hitPoint < defHP / secondMaterialDiv)
        {
            myRenderer.material = materialList[2];
        }
        else
        {
            myRenderer.material = materialList[0];
        }
    }
}
