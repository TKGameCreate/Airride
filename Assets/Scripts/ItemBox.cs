using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    [SerializeField] private Rigidbody rbody;
    [SerializeField] private Renderer myRenderer;
    [SerializeField] private List<Material> materialList = new List<Material>(); //セットするマテリアルのリスト
    [Range(1.1f, 5.0f)][SerializeField] private float firstMaterialDiv; //2段階目マテリアルを判定する際に割る数
    [Range(1.1f, 5.0f)][SerializeField] private float secondMaterialDiv; //３段階目マテリアルを判定する際に割る数
    [SerializeField] private float defHP; //初期HP
    [SerializeField] private float rotSpeed;
    [SerializeField] private float boundUpPower;
    [SerializeField] private List<GameObject> itemList = new List<GameObject>();
    private float hitPoint; //現在のHP

    private void Start()
    {
        hitPoint = defHP;
    }

    private void Update()
    {
        transform.Rotate(0, rotSpeed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Machine" || other.gameObject.tag == "Front")
        {
            Machine machine = other.transform.parent.gameObject.GetComponent<Machine>();
            hitPoint -= machine.Status(StatusType.MaxSpeed) / 2;
            SetTexture();
            if(other.gameObject.tag == "Front")
            {
                rbody.AddForce(Vector3.up * machine.SaveSpeed * boundUpPower);
            }
        }
    }

    private void SetTexture()
    {
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
