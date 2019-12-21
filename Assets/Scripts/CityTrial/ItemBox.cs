using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    private const float machineBound = 1000.0f;
    private const float boundUpPower = 0.05f;

    [SerializeField] private Rigidbody rbody;
    [SerializeField] private Renderer myRenderer;
    [SerializeField] private List<Material> materialList = new List<Material>(); //セットするマテリアルのリスト
    [Range(1.1f, 5.0f)][SerializeField] private float firstMaterialDiv; //2段階目マテリアルを判定する際に割る数
    [Range(1.1f, 5.0f)][SerializeField] private float secondMaterialDiv; //３段階目マテリアルを判定する際に割る数
    [SerializeField] private float defHP; //初期HP
    [SerializeField] private int maxGenerate;
    [SerializeField] private float destroyTime;
    [SerializeField] private float[] flashTime;
    [SerializeField] private float[] flashInterval;
    [SerializeField] private List<Item> itemList = new List<Item>();
    private float time;
    private float hitPoint; //現在のHP
    private bool generate = false; //アイテム生成フラグ
    private Flash flash = new Flash();

    private void Start()
    {
        hitPoint = defHP;
    }

    private void Update()
    {
        FlashAndDestroy();

        if (generate)
        {
            int generateNum = Random.Range(1, maxGenerate + 1); //生成する数をランダムに決める
            for (int i = 0; i < generateNum; i++)
            {
                int index = Random.Range(0, itemList.Count); //生成するアイテムを決める
                var obj = Instantiate(itemList[index], transform.position, Quaternion.identity) as Item; //アイテムの生成
                obj.InstanceVelocity(generateNum, i);
                Destroy(gameObject);
            }
        }
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
                machine.Bound(machineBound, true);
                rbody.AddForce(Vector3.up * machine.SaveSpeed * boundUpPower, ForceMode.Impulse);
            }
        }
    }

    private void FlashAndDestroy()
    {
        time += Time.deltaTime;
         if(time > destroyTime)
        {
            Destroy(gameObject);
        }
        else if (time > flashTime[2])
        {
            flash.Flashing(myRenderer, flashInterval[2]);
        }
        else if (time > flashTime[1])
        {
            flash.Flashing(myRenderer, flashInterval[1]);
        }
        else if (time > flashTime[0])
        {
            flash.Flashing(myRenderer, flashInterval[0]);
        }
    }

    private void SetTexture(Machine machine)
    {
        if (hitPoint < 1)
        {
            generate = true;
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