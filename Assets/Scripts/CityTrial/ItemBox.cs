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
    [SerializeField] private int maxGenerate;
    [SerializeField] private List<Item> itemList = new List<Item>();
    private float hitPoint; //現在のHP
    private bool generate = false; //アイテム生成フラグ
    private Machine hitMachine;

    private void Start()
    {
        hitPoint = defHP;
    }

    private void Update()
    {
        if (generate)
        {
            int GenerateNum = Random.Range(1, maxGenerate); //生成する数をランダムに決める
            for (int i = 0; i < GenerateNum; i++)
            {
                int index = Random.Range(0, itemList.Count); //生成するアイテムを決める
                var obj = Instantiate(itemList[index], transform.position, Quaternion.identity) as Item; //アイテムの生成
                obj.UpAddForce();
                Destroy(gameObject);
            }
        }

        transform.Rotate(0, rotSpeed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Front")
        {
            Machine machine = other.transform.parent.gameObject.GetComponent<Machine>();
            if (machine.Player != null)
            {
                hitPoint -= machine.Status(StatusType.MaxSpeed) / 2;
                SetTexture(machine);
                rbody.AddForce(Vector3.up * machine.SaveSpeed * boundUpPower, ForceMode.Impulse);
            }
        }
    }

    private void SetTexture(Machine machine)
    {
        if (hitPoint < 1)
        {
            generate = true;
            hitMachine = machine;
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